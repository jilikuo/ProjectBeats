using System;
using UnityEngine;

public class PlayerControllerOptionTwo : MonoBehaviour
{
    public float baseSpeed = 0.5f;
    public Vector3 playerAcc = new Vector3(0, 0, 0.01f);
    public float decelerationRate = 0.8f; // Taxa de desaceleração suave
    public float maxSpeed;

    public float horizontalSpeed = 0;
    public float verticalSpeed   = 0;

    private float negativeSpeed;
    private float negativeMaxSpeed;

    private bool movingLeft  = false;
    private bool movingRight = false;
    private bool movingUp    = false;
    private bool movingDown  = false;

    private Vector3 movementAmplitude = new(0, 0, 0);
    private Vector3 movementVector    = new(0, 0, 0);

    private Rigidbody2D playerRb;
    private EntityStats entityStats;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        entityStats = GetComponent<EntityStats>();

        // Certifique-se de que o Rigidbody não é cinemático e a gravidade está desativada para movimento controlado manualmente
        playerRb.isKinematic = false;
        playerRb.gravityScale = 0;


        maxSpeed = entityStats.CalculateMaxSpeed();
        baseSpeed = maxSpeed/10;
        playerAcc.z = entityStats.CalculateAcceleration();

        negativeSpeed = baseSpeed * -1;
        negativeMaxSpeed = maxSpeed * -1;
    }

    private void Update()
    {
        movementVector = ReadInputs();
    }

    void FixedUpdate()
    {

        HandleDirectionChange(movementVector);


        movementAmplitude = ReadAmplitude(movementVector);
        AcceleratePlayer(CalculateAccelerations(movementAmplitude));
        NormalizeMaxSpeed();
        MovePlayer(movementAmplitude);
    }

    Vector3 ReadInputs()
    {
        Vector3 input = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        return input;
    }

    Vector3 ReadAmplitude(Vector3 input)
    {
        input = new(MathF.Abs(input.x), MathF.Abs(input.y));
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
            verticalSpeed = -baseSpeed;
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

    Vector3 CalculateAccelerations(Vector3 input)
    {
        float totalInput = input.x + input.y;
        if (totalInput == 0)
        {
            playerAcc.x = 0;
            playerAcc.y = 0;
            return playerAcc;
        }

        //racionalizar baseado nos valores de input
        playerAcc.x = playerAcc.z * (input.x/totalInput);
        playerAcc.y = playerAcc.z * (input.y/totalInput);

        //multiplicar por time.fixeddeltatime para evitar acelerar demais em pouco tempo
        playerAcc.x *= Time.fixedDeltaTime;
        playerAcc.y *= Time.fixedDeltaTime;

        return playerAcc;
    }

    void AcceleratePlayer(Vector3 acceleration)
    {
        if (movingLeft)
        {
            horizontalSpeed -= acceleration.x;
        }
        if (movingRight)
        {
            horizontalSpeed += acceleration.x;
        }
        if (movingUp)
        {
            verticalSpeed += acceleration.y;
        }
        if (movingDown)
        {
            verticalSpeed -= acceleration.y;
        }
    }

    void MovePlayer(Vector3 xyz)
    {
        float xSpeed = horizontalSpeed;
        float ySpeed = verticalSpeed;
        playerRb.velocity = new Vector2(xSpeed, ySpeed);
    }

    void NormalizeMaxSpeed()
    {
        if (horizontalSpeed > maxSpeed)
            horizontalSpeed = maxSpeed;
        if (verticalSpeed > maxSpeed)
            verticalSpeed = maxSpeed;
        if (horizontalSpeed < negativeMaxSpeed)
            horizontalSpeed = negativeMaxSpeed;
        if (verticalSpeed < negativeMaxSpeed)
            verticalSpeed = negativeMaxSpeed;
    }
}
