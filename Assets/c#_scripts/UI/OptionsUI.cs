using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button gamePausedButton;
    [SerializeField] private Button closeButton;

    /// <summary>
    /// this is for managing the buttons keyBinds (btw not gonna lie this is sad that i can't use buttons as SO ).
    /// </summary>
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    /// <summary>
    /// also this is meant for the text fields when the key changes (i wanna make it dynamically adjusting the length 
    /// of the button text, for; Escape, Enter, whatever other buttons there are).
    /// </summary>
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;
    // this is meant for the button volume
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Transform pressToRebindKeyTransform;
    private float volumeDisplay = 10f;
    private Action OnOpenButtonAction;


    private void Awake()
    {
        Instance = this;
        // just updates the button and dynamicaly adjusts the voulme of the sound effects 
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            SoundManager.Instance.PlaySoundEffectsSound();
            UpdateVisual();
        });
        // just updates the button and dynamicallu adjusts the volume of the music
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        // as the name implies it just goes back to the pause button
        gamePausedButton.onClick.AddListener(() =>
        {
            Hide();
            OnOpenButtonAction();            
        });
        // this closeButton just closes both at a time
        closeButton.onClick.AddListener(() =>
        {
            Hide(); 
            GamePausedUI.Instance.Hide();
            KitchenGameManager.Instance.TogglePauseGame();
        });

        /// <summary>
        /// this is all the button rebinding logic just know that
        /// </summary>
        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Move_Right); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Interact); });
        interactAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Pause); });

    }
    private void Start()
    {
        UpdateVisual();



        Hide();
        HidePressToRebindKeyTransform();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects Button: " + Mathf.Round(SoundManager.Instance.GetVolume() * volumeDisplay);
        musicText.text = "Music Button: " + Mathf.Round(MusicManager.Instance.GetVolume() * volumeDisplay);

        moveUpText.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Move_Up);
        moveDownText.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Move_Down);
        moveRightText.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Move_Right);
        moveLeftText.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Move_Left);
        interactText.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Interact);
        interactAlternateText.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Pause);
    }

    private void RebindBinding(GameInput.Bindings bindings)
    {
        ShowPressToRebindKeyTransform();
        GameInput.Instance.RebindBindings(bindings,() => {
            HidePressToRebindKeyTransform();
            UpdateVisual();
        });  
    }

    private void ShowPressToRebindKeyTransform()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }
    private void HidePressToRebindKeyTransform()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }
    public void Show(Action OnOpenButtonAction)
    {
        KitchenGameManager.Instance.isGameObjectActive = true;
        this.OnOpenButtonAction = OnOpenButtonAction;
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        KitchenGameManager.Instance.isGameObjectActive = false;
        gameObject.SetActive(false);
    }
}
