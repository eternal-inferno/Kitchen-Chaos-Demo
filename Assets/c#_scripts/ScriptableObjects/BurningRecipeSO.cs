using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{

    public KitchenObjectSO output;
    public KitchenObjectSO input;
    public float BurningTimerProgress;

}