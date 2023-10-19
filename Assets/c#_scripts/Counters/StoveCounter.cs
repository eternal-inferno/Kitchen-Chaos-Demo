using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter,IHasProgress
{
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] BurningRecipeSO[] burningRecipeSOsArray;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private float fryingTimer;
    private float burningTimer;
    private State state;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {        
        GetCookingStateMachine();
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //there is no kitchen object
            if (player.HasKitchenObject())
            {
                //the player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // the player is carrying a cuttable item
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;

                    fryingTimer = 0f;

                    // event code inside a method
                    OnStateEvent(state);
                    OnProgressNormalizedEvent(fryingTimer, fryingRecipeSO.fryingTimerProgress);
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
                //the player is carrying something
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

                        state = State.Idle;
                        // event code inside a method
                        OnStateEvent(state);
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                //the player is carrying nothing
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
                // event code inside a method
                OnStateEvent(state);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetInputToOutput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipe in burningRecipeSOsArray)
        {
            if (burningRecipe.input == inputKitchenObjectSO)
            {
                return burningRecipe;
            }
        }
        return null;
    }

    private void GetCookingStateMachine()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:

                    fryingTimer += Time.deltaTime;
                    // event code inside a method
                    OnProgressNormalizedEvent(fryingTimer, fryingRecipeSO.fryingTimerProgress);

                    if (fryingTimer > fryingRecipeSO.fryingTimerProgress)
                    {
                        //fried!!

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        burningTimer = 0f;
                        state = State.Fried;
                        // event code inside a method
                        OnStateEvent(state);

                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    }

                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressNormalizedEvent(burningTimer, burningRecipeSO.BurningTimerProgress);

                    if (burningTimer > burningRecipeSO.BurningTimerProgress)
                    {
                        //burnt!!

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;
                        // event code inside a methos
                        OnStateEvent(state);

                        //setting the progressTimer to 0 so that it hides the bar lmfao                        
                        //OnProgressNormalizedEvent(resetTimer,resetTimer);
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });

                    }
/*
                    Debug.Log(fryingTimer);*/

                    break;
                case State.Burned:
                    break;
            }/*
            Debug.Log(state);*/
        }
    }
    private void OnStateEvent(State state)
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
    }
    private void OnProgressNormalizedEvent(float fryingTimer, float fryingTimerProgress)
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = fryingTimer / fryingTimerProgress
        });
    }
}
