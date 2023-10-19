using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool soundActive = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying;
        if(soundActive)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }

    }
}
