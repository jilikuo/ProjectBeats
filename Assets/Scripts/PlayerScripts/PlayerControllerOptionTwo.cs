using System;
using UnityEngine;

public class PlayerControllerOptionTwo : MonoBehaviour
{
    public float baseSpeed        = 0.5f;
    public float decelerationRate = 0.8f; //taxa de desaceleração de 80% da velocidade atual a cada fixed update
    public float maxSpeed;

    public Vector3 playerAcc     =  Vector3.zero;
    public float horizontalSpeed = 0;
    public float verticalSpeed   = 0;

    private float negativeSpeed;
    private float negativeMaxSpeed;

    private bool movingLeft  = false;
    private bool movingRight = false;
    private bool movingUp    = false;
    private bool movingDown  = false;

    private Vector3 movementVector = new Vector3(0, 0, 0);

    private Rigidbody2D playerRb;
    private EntityStats entityStats;

    void Start()
    {
        playerRb    = GetComponent<Rigidbody2D>();
        entityStats = GetComponent<EntityStats>();

        // Ensure the Rigidbody is not kinematic and gravity is disabled for manual control
        playerRb.isKinematic  = false;
        playerRb.gravityScale = 0;

        playerAcc.z = entityStats.CalculateAcceleration();
        maxSpeed    = entityStats.CalculateMaxSpeed();
        baseSpeed   = maxSpeed * 70 / 100;

        negativeSpeed    = -baseSpeed;
        negativeMaxSpeed = -maxSpeed;
    }

    private void Update()
    {
        movementVector = ReadInputs();
    }

    void FixedUpdate()
    {
        HandleDirectionChange(movementVector);
        AcceleratePlayer(movementVector);
        MovePlayer();
    }

    Vector3 ReadInputs()
    {
        Vector3 input = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        return input;
    }

    void HandleDirectionChange(Vector3 input)
    {
        if (input.x < 0 && !movingLeft)
        {
            horizontalSpeed = negativeSpeed;
            movingLeft = true;
            movingRight = false;
        }
        if (input.x > 0 && !movingRight)
        {
            horizontalSpeed = baseSpeed;
            movingRight = true;
            movingLeft = false;
        }
        if (input.x == 0)
        {
            movingLeft = movingRight = false;
            horizontalSpeed *= decelerationRate;
            if (Mathf.Abs(horizontalSpeed) < 0.01f) horizontalSpeed = 0;
        }

        if (input.y < 0 && !movingDown)
        {
            verticalSpeed = negativeSpeed;
            movingDown = true;
            movingUp = false;
        }
        if (input.y > 0 && !movingUp)
        {
            verticalSpeed = baseSpeed;
            movingUp = true;
            movingDown = false;
        }
        if (input.y == 0)
        {
            movingUp = movingDown = false;
            verticalSpeed *= decelerationRate;
            if (Mathf.Abs(verticalSpeed) < 0.01f) verticalSpeed = 0;
        }
    }

    void AcceleratePlayer(Vector3 input)
    {
        if (movingLeft)
        {
            horizontalSpeed = Mathf.Max(horizontalSpeed - playerAcc.z * Time.fixedDeltaTime, negativeMaxSpeed);
        }
        if (movingRight)
        {
            horizontalSpeed = Mathf.Min(horizontalSpeed + playerAcc.z * Time.fixedDeltaTime, maxSpeed);
        }
        if (movingUp)
        {
            verticalSpeed = Mathf.Min(verticalSpeed + playerAcc.z * Time.fixedDeltaTime, maxSpeed);
        }
        if (movingDown)
        {
            verticalSpeed = Mathf.Max(verticalSpeed - playerAcc.z * Time.fixedDeltaTime, negativeMaxSpeed);
        }
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
