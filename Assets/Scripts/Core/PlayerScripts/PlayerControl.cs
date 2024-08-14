using UnityEngine;

[RequireComponent(typeof(PlayerInputRead), typeof(PlayerMovement))]
public class PlayerControl : MonoBehaviour
{
    private PlayerInputRead playerInput;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerInput = GetComponent<PlayerInputRead>();
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
