using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnBindingChanged;

    // Intializing a new event publisher
    public event EventHandler OnInteractAction;
    public event EventHandler OnAlternateInteractAction;
    public event EventHandler OnPauseAction;
    // player Input actions is a new input system script
    private InputActions playerInputActions;

    public enum Bindings
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause
    }

    private void Awake()
    {
        Instance = this;
        // Intializing the class
        playerInputActions = new InputActions();
        
        // Setting the player bindings from the save binding jason method ( i really don't know what i'm doing
        // but i'm trying... )
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
        // Reaching the action map called Player, and enabling it.
        playerInputActions.Player.Enable();
        // Through the action keys, we're accessing the event that listens to key presses here.
        playerInputActions.Player.Interactions.performed += Interactions_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interactions.performed -= Interactions_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // This event will listen for the escape key for pausing.
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // The event OnAlternateInteractAction we made, listens to the key presses through this publisher.
        OnAlternateInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interactions_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // The event OnInteractAction we made, listens to the key presses through this publisher.
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    // this public method is being used in the player class script 
    public Vector2 GetMovementVectorNormalized()
    {
        // Vector 2, that has the value of our action map called (Move) which
        // turns the value to a vector 2 { poggies :) }
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        // normalizes the vector 2
        inputVector = inputVector.normalized;        
        return inputVector;
    }

    public string GetBingingsText(Bindings bindings)
    {
        switch (bindings){
            default:
            case Bindings.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Bindings.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Bindings.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Bindings.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Bindings.Interact:
                return playerInputActions.Player.Interactions.bindings[0].ToDisplayString();
            case Bindings.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Bindings.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBindings(Bindings bindings, Action OnActionRebound)
    {

        InputAction inputAction;
        int BindingIndex;
        switch (bindings)
        {
            default:
                case Bindings.Move_Up:
                inputAction = playerInputActions.Player.Move;
                BindingIndex = 1;
                break;
                case Bindings.Move_Down:
                inputAction = playerInputActions.Player.Move;
                BindingIndex = 2;
                break;
                case Bindings.Move_Left:
                inputAction = playerInputActions.Player.Move;
                BindingIndex = 3;
                break;
                case Bindings.Move_Right:
                inputAction = playerInputActions.Player.Move;
                BindingIndex = 4;
                break;
                case Bindings.Interact:
                inputAction = playerInputActions.Player.Interactions;
                BindingIndex = 0;
                break;
                case Bindings.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                BindingIndex = 0;
                break;
                case Bindings.Pause:
                inputAction = playerInputActions.Player.Pause;
                BindingIndex = 0;
                break;
        }
        playerInputActions.Disable();
        inputAction.PerformInteractiveRebinding(BindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            playerInputActions.Enable();
            OnActionRebound();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnBindingChanged?.Invoke(this, EventArgs.Empty);
        })
        .Start();
    }
}
