using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconImage;

    private void Awake()
    {
        iconImage.gameObject.SetActive(false);
    }
    public void SetDeliveryRecipeSO(DeliveryRecipeSO deliveryRecipeSO)
    {
        recipeNameText.text = deliveryRecipeSO.recipeName;

        foreach( Transform child in iconContainer)
        {
            if (child == iconImage) continue;
            Destroy(child.gameObject);
        }
        foreach( KitchenObjectSO kitchenObjectSO in deliveryRecipeSO.kitchenObjectSOList )
        {
            Transform iconTransform = Instantiate(iconImage, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
