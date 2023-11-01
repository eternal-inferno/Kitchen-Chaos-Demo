using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    // we need to connect the UI text to our script
    // we need to access the gameInput bindings text and update it
    // we need to make sure that when we change our bindings while the tutorial is open that we change it
    // we need to only have the tutorial open, the first few seconds of the game, and then close when the gameplay starts

    [SerializeField] private TextMeshProUGUI moveUpKey;
    [SerializeField] private TextMeshProUGUI moveDownKey;
    [SerializeField] private TextMeshProUGUI moveRightKey;
    [SerializeField] private TextMeshProUGUI moveLeftKey;
    [SerializeField] private TextMeshProUGUI InteractKey;
    [SerializeField] private TextMeshProUGUI InteractAltKey;
    [SerializeField] private TextMeshProUGUI PauseKey;


    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        GameInput.Instance.OnBindingChanged += GameInput_OnBindingChanged;

        UpdateVisual();
        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsWaitingToStart())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void GameInput_OnBindingChanged(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        moveUpKey.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Move_Up);
        moveDownKey.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Move_Down);
        moveRightKey.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Move_Right);
        moveLeftKey.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Move_Left);
        InteractKey.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Interact);
        InteractAltKey.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.InteractAlternate);
        PauseKey.text = GameInput.Instance.GetBingingsText(GameInput.Bindings.Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
