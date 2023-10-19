using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngridentAddedEventArgs> OnIngridentAdded;
    public class OnIngridentAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectList; 
    private List<KitchenObjectSO> kitchenObjectSOList;
    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngrident(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectList.Contains(kitchenObjectSO))
        {
            //checks if there's no valid ingrident
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //checks if there's a kitchen object of this type already
            return false;
        }
        else
        {
            //else if there isn't 
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngridentAdded?.Invoke(this, new OnIngridentAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
