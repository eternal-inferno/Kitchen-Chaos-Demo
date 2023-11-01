using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    public static SoundManager Instance { get; private set; }

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    {        
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFail += DeliveryManager_OnRecipeFail;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnObjectPickup += Player_OnObjectPickup;
        BaseCounter.OnAnyObjectDrop += BaseCounter_OnObjectDrop;
        TrashContainer.OnAnyObjectTrashed += TrashContainer_OnAnyObjectTrashed;
    }

    private void TrashContainer_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashContainer trashContainer = (TrashContainer)sender;
        PlaySound(audioClipRefsSO.trash, trashContainer.transform.position);
    }

    private void BaseCounter_OnObjectDrop(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnObjectPickup(object sender, System.EventArgs e)
    {
        Player player = (Player)sender;
        PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }
    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {        
        CuttingCounter cuttingCounter = (CuttingCounter)sender;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFail(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);        
    }
    
    public void PlayFootstepSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footStep, position, volume);
    }    

    public void PlayCountDownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);
    }    

    public void PlaySoundEffectsSound()
    {
        PlaySound(audioClipRefsSO.objectPickup, Vector3.zero);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if(volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

}
