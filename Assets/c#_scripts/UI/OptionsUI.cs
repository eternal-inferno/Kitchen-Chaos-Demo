using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    private float volumeDisplay = 10f;

    private void Awake()
    {
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects Button: " + Mathf.Round(SoundManager.Instance.GetVolume() * volumeDisplay);
        musicText.text = "Music Button: " + Mathf.Round(SoundManager.Instance.GetVolume() * volumeDisplay);
    }
}
