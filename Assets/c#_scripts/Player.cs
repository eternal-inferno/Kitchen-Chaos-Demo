using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    // This Singleton contains every public method in the class Player
    public static Player Instance {  get; private set; }    
    
    public event EventHandler<OnSelectedCounterChangedArgs> OnSelectedCounterChanged;
    public event EventHandler OnObjectPickup;
    public class OnSelectedCounterChangedArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    // accessible stuff from the editor :)
    [SerializeField]private float moveSpeed;
    [SerializeField]private GameInput gameInput;
    [SerializeField]private LayerMask counterLayerMask;
    [SerializeField] private Transform KitchenObjectHoldPoint;

    //animation thing for walking ig
    public bool isWalking;
    //locks in the interaction
    private Vector3 lastInteractDir;
    //has the logic for a selected counter
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        //singleton patter bs i guess
        if (Instance != null)
        {
            // also meant to check whether there's more than one player, other wise shit hit the fan
            // also make sure to run this in awake,
            // otherwise there will be a null ref error cause of the selected
            // counter visual that uses it, KEEP IN MIND.

            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        //referencing the event for the key input (E) which is a interaction :O
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        //referencing the event for the key input (F) which is a interaction :O
        gameInput.OnAlternateInteractAction += GameInput_OnAlternateInteractAction;
    }

    private void GameInput_OnAlternateInteractAction(object sender, EventArgs e)
    {
        // In order to run the event we gotta have a check for whether it's null or not
        // Otherwise null reference exception is gonna be thrown :|
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
        
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        // In order to run the event we gotta have a check for whether it's null or not
        // Otherwise null reference exception is gonna be thrown :|
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
        
    }

    private void Update(){
        // it runs the code every single frame, hopefully no lag :|
        HandleMovement();
        HandleInteraction();
    }
    private void HandleInteraction()
    {
        // Uses the method we made at game input, more info there
        // But TLDR; is that it does movement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        //only moves in the x & z directions not y, since the vector 2,
        //takes in two axes we have to specify those directions, otherwise
        //we end up moving up in the sky and to the sides only. lol 
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //locks in the Interaction
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactionDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactionDistance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {                    
                    selectedCounter = baseCounter;

                    SetSelectedCounter(selectedCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
        //Debug.Log(selectedCounter);
    }
    private void HandleMovement() 
    {
        //handles movement and normalizes it 
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        //only moves in the x & z directions not y, we float then :|
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRaidus = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaidus, moveDir, moveDistance);

        if (!canMove)
        {
            //Cannot move towards moveDir

            //Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaidus, moveDirX, moveDistance);

            if (canMove)
            {
                //Can move only in X
                moveDir = moveDirX;
            }
            else
            {
                //Cannot move only in X

                //Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaidus, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    //Cannot move in any direction
                    moveDir = Vector3.zero;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;        

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedArgs
        {
            selectedCounter = selectedCounter
        });
    }

    // methods that the interface needs to implement in the code
    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null)
        {
            OnObjectPickup?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
