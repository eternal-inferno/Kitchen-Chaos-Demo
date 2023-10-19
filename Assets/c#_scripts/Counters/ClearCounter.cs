using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //there is no kitchen object
            if (player.HasKitchenObject())
            {
                //the player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                    }
                }                
                else
                {
                    //the counter has a kitchen object
                    if (GetKitchenObject().TryGetPlate( out plateKitchenObject))
                    {
                        //the counter has a plate
                        plateKitchenObject = GetKitchenObject() as PlateKitchenObject;
                        //casting the kitchen object the counter is holding as a plate kitchen object
                        if (plateKitchenObject.TryAddIngrident(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            //we're now adding to TryAddIngrdient Check PlateKitchenObject for info
                            player.GetKitchenObject().DestroySelf();
                            //then destroying the kitchen object in the player :O
                        }
                    }                    
                }
            }
            else
            {
                //the player is carrying nothing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }    

    }
}
