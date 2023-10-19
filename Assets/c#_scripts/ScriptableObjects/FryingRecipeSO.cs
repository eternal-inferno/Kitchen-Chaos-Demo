using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{

    public KitchenObjectSO output;
    public KitchenObjectSO input;
    public float fryingTimerProgress;

}