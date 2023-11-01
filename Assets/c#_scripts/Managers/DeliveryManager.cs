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
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            //So we're containing DeliveryRecipeSOList DeliveryRecipeSO's inside the waitingDeliveryRecipeSO,
            //which would have a random burger recipe inside of it, and adding it to waitingDeliveryRecipeSOList          
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingDeliveryRecipeMax > waitingDeliveryRecipeSOList.Count)
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
        int oneTime = 1;
        for (int i = 0; i < oneTime;)
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

                    
                    //Cycling through all the kitchen objects inside the kitchen plate

                    if (recipeKitchenObjectSO == GetPlateKitchenObjectSO(plateKitchenObject))
                    {
                        // The ingridents match
                        ingridentsFound = true;
                        break;
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
            break;
        }
    }

    public KitchenObjectSO GetPlateKitchenObjectSO(PlateKitchenObject playerPlate)
    {
        foreach(KitchenObjectSO kitchenObjectSO in playerPlate.GetKitchenObjectSOList())
        {
            return kitchenObjectSO;
        }
        return null;
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
