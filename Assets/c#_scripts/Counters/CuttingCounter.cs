using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public static event EventHandler OnAnyCut;
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        //there is no kitchen object
        if (!HasKitchenObject())
        {
            //the player is carrying something
            if (player.HasKitchenObject())
            {
                // the player is carrying a cuttable item
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {                    
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
            else
            {
                //the player is carrying nothing
            }
        }
        else
        {
            // there is a kitchen object
            if (player.HasKitchenObject())
            {
                //the player is carrying a plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //the player has a plate
                    plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    //casting the kitchen object the player is holding as a plate kitchen object
                    if (plateKitchenObject.TryAddIngrident(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        //we're now adding to TryAddIngrdient Check PlateKitchenObject for info
                        GetKitchenObject().DestroySelf();
                        //then destroying the kitchen object in the counter :O
                    }
                }
            }
            else
            {
                //the player is carrying nothing
                GetKitchenObject().SetKitchenObjectParent(player);


                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        // if there is a kitchen object & is a cuttable object
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {            
            // contains how many times it should take to cut the object
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if ( cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                // stores the values for the correct recipe SO
                KitchenObjectSO outputKitchenObjectSO = GetOutputToInput(GetKitchenObject().GetKitchenObjectSO());
                //destroys the kitchen object when interacted with (F)
                GetKitchenObject().DestroySelf();
                //spawns the correct recipe for the object interacted
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);


            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputToInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

}
