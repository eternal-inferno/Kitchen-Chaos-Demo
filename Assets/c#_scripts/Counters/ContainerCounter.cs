using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        //Basically we're spawning a kitchen object Prefab once we interact with a ClearCounter at local pos. 
        //(AKA: parent position) to counterTopPoint
        if (!player.HasKitchenObject())
        {
            // player is not carrying anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        else
        {
/*            // player is carrying something
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
            }*/
        }
    }
}