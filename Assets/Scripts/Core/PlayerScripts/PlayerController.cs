using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Vector3 inputVector = playerInput.ReadInput();
        playerMovement.HandleDirectionChange(inputVector);
    }

    private void FixedUpdate()
    {
        playerMovement.AcceleratePlayer();
        playerMovement.MovePlayer();
    }
}
