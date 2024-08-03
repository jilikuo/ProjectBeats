using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOptionTwo : MonoBehaviour
{
    public float baseSpeed = 0.5f;
    public Vector3 playerAcc = new Vector3(0, 0, 0.01f);
    public float playerDcc = 0.5f; // Atualmente não efetivo, apesar de estar no código. O problema é que só é acionada quando o jogador solta o botão, mas o Axis é zerado imediatamente quando isso acontece, zerando imediatamente a velocidade do jogador no próximo fixedupdate.
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

    void Start()
    {
        maxSpeed = this.gameObject.GetComponent<PlayerStats>().CalculateMaxSpeed();
        playerAcc.z = this.gameObject.GetComponent<PlayerStats>().CalculateAcceleration();

        negativeSpeed = baseSpeed * -1;
        negativeMaxSpeed = maxSpeed * -1;
    }

    void FixedUpdate()
    {
        movementVector = ReadInputs();
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
            if (movingLeft)
            {
                if ((horizontalSpeed + playerDcc) > negativeSpeed)
                {
                    horizontalSpeed = 0;
                    movingLeft = false;
                }
                else
                {
                    horizontalSpeed += playerDcc;
                }
            }
            if (movingRight)
            {
                if ((horizontalSpeed - playerDcc) < baseSpeed)
                {
                    horizontalSpeed = 0;
                    movingRight = false;
                }
                else
                {
                    horizontalSpeed -= playerDcc;
                }
            }
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
            if (movingDown)
            {
                if ((verticalSpeed + playerDcc) > negativeSpeed)
                {
                    verticalSpeed = 0;
                    movingDown = false;
                }
                else
                {
                    verticalSpeed += playerDcc;
                }

            }
            if (movingUp)
            {
                if ((verticalSpeed - playerDcc) < baseSpeed)
                {
                    verticalSpeed = 0;
                    movingUp = false;
                }
                else
                {
                    verticalSpeed -= playerDcc;
                }
            }
        }
    }

    Vector3 CalculateAccelerations(Vector3 input)
    {
        float totalInput = input.x + input.y;
        playerAcc = new Vector3(input.x, input.y, playerAcc.z);


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
        if (xyz != new Vector3(0,0,0) || float.IsNaN(xyz.x) || float.IsNaN(xyz.y))
        {
            float xSpeed = xyz.x * horizontalSpeed;
            float ySpeed = xyz.y * verticalSpeed;
            transform.Translate(xSpeed, ySpeed, 0);
        }
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
