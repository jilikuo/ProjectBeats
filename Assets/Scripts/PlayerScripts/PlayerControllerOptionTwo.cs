using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOptionTwo : MonoBehaviour
{
    public float baseSpeed = 0.5f;
    public float playerAcc = 0.01f;
    public float playerDcc = 0.5f; // Atualmente não efetivo, apesar de estar no código. O problema é que só é acionada quando o jogador solta o botão, mas o Axis é zerado imediatamente quando isso acontece, zerando imediatamente a velocidade do jogador no próximo fixedupdate.
    public float maxSpeed  = 1.0f;

    public float horizontalSpeed;
    public float verticalSpeed;

    private float negativeSpeed;
    private float negativeMaxSpeed;

    private bool movingLeft  = false;
    private bool movingRight = false;
    private bool movingUp    = false;
    private bool movingDown  = false;


    void Start()
    {
        negativeSpeed = baseSpeed * -1;
        negativeMaxSpeed = maxSpeed * -1;

        horizontalSpeed = 0;
        verticalSpeed = 0;
    }


    void Update()
    {
        
    }

    void FixedUpdate()
    {
        TranslatePlayer(MovePlayer());
    }

    Vector3 MovePlayer()
    {
        float inputX   = Input.GetAxis("Horizontal");
        float inputY   = Input.GetAxis("Vertical");

        if (movingLeft)
        {
            if ((horizontalSpeed - playerAcc) > negativeMaxSpeed)
            {
                horizontalSpeed -= playerAcc;
            }
            else
            {
                horizontalSpeed = negativeMaxSpeed;
            }
        }
        if (movingRight)
        {
            if ((horizontalSpeed + playerAcc) < maxSpeed)
            {
                horizontalSpeed += playerAcc;
            }
            else
            {
                horizontalSpeed = maxSpeed;
            }
        }
        if (movingUp)
        {
            if ((verticalSpeed + playerAcc) < maxSpeed)
            {
                verticalSpeed += playerAcc;
            }
            else
            {
                verticalSpeed = maxSpeed;
            }
        }
        if (movingDown)
        {
            if ((verticalSpeed - playerAcc) > negativeMaxSpeed)
            {
                verticalSpeed -= playerAcc;
            }
            else
            {
                verticalSpeed = negativeMaxSpeed;
            }
        }

        if (inputX < 0 && !movingLeft)
        {
            horizontalSpeed = negativeSpeed; 
            movingLeft = true;
            movingRight = false;
        }
        if (inputX > 0 && !movingRight)
        {
            horizontalSpeed = baseSpeed;
            movingRight = true;
            movingLeft = false;
        }

        if (inputX == 0)
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

        if (inputY < 0 && !movingDown)
        {
            verticalSpeed = -baseSpeed;
            movingDown = true;
            movingUp = false;
        }
        if (inputY > 0 && !movingUp)
        {
            verticalSpeed = baseSpeed;
            movingUp = true;
            movingDown = false;
        }
        if (inputY == 0)
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

        inputX = MathF.Abs(inputX);
        inputY = MathF.Abs(inputY);

        Vector3 result = new(inputX, inputY, 0);
        
        return result;
    }

    void TranslatePlayer(Vector3 xyz)
    {
        if (xyz != new Vector3(0,0,0))
        {
            float xSpeed = xyz.x * horizontalSpeed;
            float ySpeed = xyz.y * verticalSpeed;
            transform.Translate(xSpeed, ySpeed, 0);
        }
    }

    void UpdatePlayerSpeed()
    {
    
    }
   

    
}
