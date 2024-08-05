using System;
using UnityEngine;

public class PlayerControllerOptionTwo : MonoBehaviour
{
    public float baseSpeed        = 0.5f;
    public float decelerationRate = 0.8f; //taxa de desaceleração de 80% da velocidade atual a cada fixed update
    public float maxSpeed;

    public float playerAcc       = 0;
    public float horizontalSpeed = 0;
    public float verticalSpeed   = 0;

    private bool movingLeft  = false;
    private bool movingRight = false;
    private bool movingUp    = false;
    private bool movingDown  = false;

    private Vector3 movementVector = new Vector3(0, 0, 0);

    private Rigidbody2D playerRb;
    private EntityStats entityStats;
    bool inputRead = false;

    void Start()
    {
        playerRb    = GetComponent<Rigidbody2D>();
        entityStats = GetComponent<EntityStats>();

        // Ensure the Rigidbody is not kinematic and gravity is disabled for manual control
        playerRb.isKinematic  = false;
        playerRb.gravityScale = 0;

        playerAcc = entityStats.CalculateAcceleration();
        maxSpeed  = entityStats.CalculateMaxSpeed();
        baseSpeed = maxSpeed * 0.6f;
    }

    private void Update()
    {
        movementVector = ReadInputs();
    }

    void FixedUpdate()
    {
        if (inputRead)
        {
            HandleDirectionChange(movementVector);
            inputRead = false;
        }
        AcceleratePlayer();
        MovePlayer();
    }

    Vector3 ReadInputs()
    {
        Vector3 input = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if (input.magnitude > 1)
        {
            input.Normalize();
        }
        inputRead = true;

        return input;
    }

    void HandleDirectionChange(Vector3 input)
    {
        if (input.x < 0 && !movingLeft)
        {
            horizontalSpeed = -baseSpeed;
            movingLeft = true;
            movingRight = false;
        }
        else if (input.x > 0 && !movingRight)
        {
            horizontalSpeed = baseSpeed;
            movingRight = true;
            movingLeft = false;
        }
        else if (input.x == 0)
        {
            movingLeft = movingRight = false;
            horizontalSpeed *= decelerationRate;
            if (Mathf.Abs(horizontalSpeed) < 0.01f) horizontalSpeed = 0;
        }

        if (input.y < 0 && !movingDown)
        {
            verticalSpeed = -baseSpeed;
            movingDown = true;
            movingUp = false;
        }
        else if (input.y > 0 && !movingUp)
        {
            verticalSpeed = baseSpeed;
            movingUp = true;
            movingDown = false;
        }
        else if (input.y == 0)
        {
            movingUp = movingDown = false;
            verticalSpeed *= decelerationRate;
            if (Mathf.Abs(verticalSpeed) < 0.01f) verticalSpeed = 0;
        }
    }

    void AcceleratePlayer()
    {
        if (movingLeft)
        {
            horizontalSpeed = Mathf.Min(horizontalSpeed - (playerAcc * Time.fixedDeltaTime), -baseSpeed + (playerAcc * Time.fixedDeltaTime));
        }
        if (movingRight)
        {
            horizontalSpeed = Mathf.Max(horizontalSpeed + (playerAcc * Time.fixedDeltaTime), baseSpeed - (playerAcc * Time.fixedDeltaTime));
        }
        if (movingUp)
        {
            verticalSpeed = Mathf.Max(verticalSpeed + (playerAcc * Time.fixedDeltaTime), baseSpeed - (playerAcc * Time.fixedDeltaTime));
        }
        if (movingDown)
        {
            verticalSpeed = Mathf.Min(verticalSpeed - (playerAcc * Time.fixedDeltaTime), -baseSpeed + (playerAcc * Time.fixedDeltaTime));
        }

        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);
        verticalSpeed = Mathf.Clamp(verticalSpeed, -maxSpeed, maxSpeed);
    }

    void MovePlayer()
    {
        Vector2 velocity = new Vector2(horizontalSpeed, verticalSpeed);
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        playerRb.velocity = velocity;
    }
}
