using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOptionTwo : MonoBehaviour
{
    public float baseSpeed = 0.5f;
    public float playerAcc = 0.01f;
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

    private Vector3 movementVector = new(0, 0, 0);

    void Start()
    {
        maxSpeed = this.gameObject.GetComponent<PlayerStats>().CalculateMaxSpeed();

        negativeSpeed = baseSpeed * -1;
        negativeMaxSpeed = maxSpeed * -1;
    }

    void FixedUpdate()
    {
        movementVector = MovePlayer();
        TranslatePlayer(movementVector);
    }

    Vector3 MovePlayer()
    {
        float inputX   = Input.GetAxis("Horizontal");
        float inputY   = Input.GetAxis("Vertical");

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


        //aqui cai na armadilha de que a aceleração não pode ser ajustada facilmente para a horizontal. A tentativa de corrigir apenas distribui a aceleração igualmente (com um pequeno bug) entre os dois eixos.
        if (movingLeft && horizontalSpeed > negativeMaxSpeed)
        {
            if (movingDown && verticalSpeed > negativeMaxSpeed)
            {
                float totalInput = inputX + inputY;
                float correctAccX = playerAcc * (inputX / totalInput);
                float correctAccY = playerAcc * (inputY / totalInput);

                horizontalSpeed -= correctAccX;
                verticalSpeed   -= correctAccY;
            }
            else if (movingUp && verticalSpeed < maxSpeed)
            {
                float totalInput = inputX + inputY;
                float correctAccX = playerAcc * (inputX / totalInput);
                float correctAccY = playerAcc * (inputY / totalInput);

                horizontalSpeed -= correctAccX;
                verticalSpeed   += correctAccY;
            }
            else if ((horizontalSpeed - playerAcc) > negativeMaxSpeed)
            {
                horizontalSpeed -= playerAcc;
            }
            else
            {
                horizontalSpeed = negativeMaxSpeed;
            }
        }
        else if (movingRight && horizontalSpeed < maxSpeed)
        {
            if (movingDown && verticalSpeed > negativeMaxSpeed)
            {
                float totalInput = inputX + inputY;
                float correctAccX = playerAcc * (inputX / totalInput);
                float correctAccY = playerAcc * (inputY / totalInput);

                horizontalSpeed += correctAccX;
                verticalSpeed -= correctAccY;
            }
            else if (movingUp && verticalSpeed < maxSpeed)
            {
                float totalInput = inputX + inputY;
                float correctAccX = playerAcc * (inputX / totalInput);
                float correctAccY = playerAcc * (inputY / totalInput);

                horizontalSpeed += correctAccX;
                verticalSpeed += correctAccY;
            }
            else if ((horizontalSpeed + playerAcc) < maxSpeed)
            {
                horizontalSpeed += playerAcc;
            }
            else
            {
                horizontalSpeed = maxSpeed;
            }
        }
        else if (movingUp && verticalSpeed < maxSpeed)
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
        else if (movingDown && verticalSpeed > negativeMaxSpeed)
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

    

}
