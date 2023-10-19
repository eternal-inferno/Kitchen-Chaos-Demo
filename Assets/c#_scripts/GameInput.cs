using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    // Intializing a new event publisher
    public event EventHandler OnInteractAction;
    public event EventHandler OnAlternateInteractAction;
    public event EventHandler OnPauseAction;
    // player Input actions is a new input system script
    private InputActions playerInputActions;

    private void Awake()
    {
        Instance = this;
        // Intializing the class
        playerInputActions = new InputActions();
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
}
