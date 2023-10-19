using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    // This is for the SFX in the Sound Manager :\
    public event EventHandler OnRecipeFail;
    public event EventHandler OnRecipeSuccess;
    // This is for the UI elements :|
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private DeliveryListSO deliveryListSO;
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private float spawnRecipeTimerMax = 4f;
    [SerializeField] private int waitingDeliveryRecipeMax = 4;
    private int successfulRecipeAmount;
    
    private List<DeliveryRecipeSO> waitingDeliveryRecipeSOList;

    private float spawnRecipeTimer;
    private void Awake()
    {
        Instance = this;

        waitingDeliveryRecipeSOList = new List<DeliveryRecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            //So we're containing DeliveryRecipeSOList DeliveryRecipeSO's inside the waitingDeliveryRecipeSO,
            //which would have a random burger recipe inside of it, and adding it to waitingDeliveryRecipeSOList          
            if(waitingDeliveryRecipeMax > waitingDeliveryRecipeSOList.Count)
            {
                DeliveryRecipeSO waitingDeliveryRecipeSO = deliveryListSO.deliveryRecipeSOList[Random.Range( 0, deliveryListSO.deliveryRecipeSOList.Count)];                
                
                waitingDeliveryRecipeSOList.Add(waitingDeliveryRecipeSO);

                // Event meant for listening when a recipe is taken
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0; i < waitingDeliveryRecipeSOList.Count; i++)
        {
            // Goes through the recipe, Example Recipe: Cheese Burger
            DeliveryRecipeSO waitingDeliveryRecipeSO = waitingDeliveryRecipeSOList[i];            
            
            if (waitingDeliveryRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // has the same number of ingridents

                bool plateIngridentsMatchesPlate = true;

                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingDeliveryRecipeSO.kitchenObjectSOList)
                {
                    // Cycling through all the kitchen objects inside the recipe, Example Recipe: Plain Burger

                    bool ingridentsFound = false;

                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //Cycling through all the kitchen objects inside the kitchen plate

                        if (recipeKitchenObjectSO == plateKitchenObjectSO)
                        {
                            // The ingridents match
                            ingridentsFound = true;
                            break;
                        }
                    }
                    if (!ingridentsFound)
                    {
                        // This recipe ingrident was not found on the plate
                        plateIngridentsMatchesPlate = false;
                    }
                }
                if (plateIngridentsMatchesPlate)
                {
                    // The Player delivered the correct recipe
                    
                    waitingDeliveryRecipeSOList.RemoveAt(i);
                    successfulRecipeAmount++;
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
            // No matches found!
            //the player did not Deliver the correct Recipe
            OnRecipeFail?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<DeliveryRecipeSO> GetWaitingDeliveryRecipeSOList()
    {
        return waitingDeliveryRecipeSOList;
    }
    public int GetSuccessfulRecipeAmount()
    {
        return successfulRecipeAmount;
    }

}
