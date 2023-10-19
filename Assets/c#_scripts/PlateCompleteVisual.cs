using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngridentAdded += PlateKitchenObject_OnIngridentAdded;

        foreach (KitchenObjectSO_GameObject kichenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            kichenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngridentAdded(object sender, PlateKitchenObject.OnIngridentAddedEventArgs e)
    {
        foreach(KitchenObjectSO_GameObject kichenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            if(kichenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kichenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }    
}
