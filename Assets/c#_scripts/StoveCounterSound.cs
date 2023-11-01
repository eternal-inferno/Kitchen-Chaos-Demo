using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private AudioSource audioSource;
    private float warningSoundTimer;
    bool playWarningSoundTimer;
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnWarningSoundTimer = 0.2f;
        playWarningSoundTimer = e.progressNormalized >= burnWarningSoundTimer && stoveCounter.IsFried();
    }
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool soundActive = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying;
        if (soundActive)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }

    }
    private void Update()
    {
        // this is a condition ensuring that this logic will run when the warning visual shows up
        if (playWarningSoundTimer)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0)
            {
                // this loop will run 5 times a second
                float warningSoundTimerMax = 0.2f;
                warningSoundTimer = warningSoundTimerMax;
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);

            }
        }
    }


}
