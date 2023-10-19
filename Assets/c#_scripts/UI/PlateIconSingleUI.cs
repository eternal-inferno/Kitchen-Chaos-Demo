using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image Icon;    

    public void SetPlateSprite(KitchenObjectSO kitchenObjectSO)
    {
        Icon.sprite = kitchenObjectSO.sprite;
    }
}
