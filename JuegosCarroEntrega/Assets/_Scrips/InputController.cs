using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController instance;
    private InputAction moveInput;
    private InputAction brakeInput;

    [HideInInspector] public Vector2 movementVector;
    [HideInInspector] public bool isBraking;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        
        }
        else
        {
            Destroy(this);
        }

        moveInput = InputSystem.actions.FindAction("Move");
        brakeInput = InputSystem.actions.FindAction("Interact");

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       isBraking = brakeInput.IsPressed();
        GetInputMovement();
    }

    public void GetInputMovement()
    {
        movementVector = moveInput.ReadValue<Vector2>();
    }
}
