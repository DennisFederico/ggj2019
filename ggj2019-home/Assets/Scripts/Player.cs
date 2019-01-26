using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class Player : MonoBehaviour
{
    public PlayerInput PlayerInputControls;
    public float speed;
    public float turnSpeed;

    private Vector2 _move;

    private void Awake()
    {
        PlayerInputControls.Player.Shot.performed += _ => Shot();
        PlayerInputControls.Player.Movement.performed += OnMovementDone;
        
        foreach(InputDevice device in InputSystem.devices)
        {
            Debug.Log("device:" + device.name);
        }
    }

    private void OnEnable()
    {
        PlayerInputControls.Enable();
    }

    private void OnMovementDone(InputAction.CallbackContext ctx)
    {
        _move = ctx.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        PlayerInputControls.Disable();
    }

    private void Update()
    {
        Move(_move);
    }

    private void Shot()
    {
        Debug.Log("SHOT");
    }

    private void Move(Vector2 inputMove)
    {
        Vector3 forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));
        Vector3 move = inputMove.y * forward + inputMove.x * Camera.main.transform.right;

        if (move != Vector3.zero)
        {
            move.Normalize();
            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, Vector3.up);
            float turnAmount = Mathf.Atan2(move.x, move.z);

            // Move
            transform.Translate(move * speed * Time.deltaTime);
            // Rotate
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }
    }

}
