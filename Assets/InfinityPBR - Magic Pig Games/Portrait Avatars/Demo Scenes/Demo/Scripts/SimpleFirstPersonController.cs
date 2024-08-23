using UnityEngine;

namespace MagicPigGames
{
    [RequireComponent(typeof(CharacterController))]
    public class SimpleFirstPersonController : MonoBehaviour
    {
        public float moveSpeed = 3.0f;
        public float turnSpeed = 300.0f;
        public float tiltSpeed = 200.0f;
        public float maxTilt = 40.0f;

        private CharacterController characterController;
        private float tilt = 0.0f;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            //Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            // Mouse Look Control
            float turn = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
            transform.Rotate(0, turn, 0);

            tilt -= Input.GetAxis("Mouse Y") * tiltSpeed * Time.deltaTime;
            tilt = Mathf.Clamp(tilt, -maxTilt, maxTilt);
            Camera.main.transform.localRotation = Quaternion.Euler(tilt, 0, 0);

            // Movement Control
            float moveDirectionY = characterController.velocity.y;
            Vector3 forwardMovement = transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * moveSpeed;
            Vector3 rightMovement = transform.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal") * moveSpeed;
            Vector3 move = forwardMovement + rightMovement;

            if (!characterController.isGrounded)
            {
                move.y = moveDirectionY;
            }

            characterController.SimpleMove(move);
        }
    }
}
