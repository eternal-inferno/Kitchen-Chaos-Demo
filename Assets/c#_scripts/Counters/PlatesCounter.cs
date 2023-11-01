using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateDestroyed;

    [SerializeField] KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int spawnPlateAmount;
    private int spawnPlateAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;            
            if (KitchenGameManager.Instance.IsGamePlaying() && spawnPlateAmount < spawnPlateAmountMax)
            {
                spawnPlateAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //the player is not carrying anything
            if(spawnPlateAmount > 0)
            {
                //if there is at least 1 plate in the counter
                spawnPlateAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateDestroyed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
