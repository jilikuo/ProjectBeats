using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOptionTwo : MonoBehaviour
{
    public float baseSpeed = 0.5f;
    public float playerAcc = 0.01f;
    public float playerDcc = 0.5f;
    public float maxSpeed  = 1.0f;

    private float horizontalSpeed;
    private float verticalSpeed;

    private bool movingLeft  = false;
    private bool movingRight = false;
    private bool movingUp    = false;
    private bool movingDown  = false;


    void Start()
    {
        horizontalSpeed = 0;
        verticalSpeed = 0;
    }


    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    Vector3 MovePlayer()
    {
        float inputX   = Input.GetAxis("Horizontal");
        float inputY   = Input.GetAxis("Vertical");

        if (movingLeft)
        {
            horizontalSpeed -= playerAcc;
        }
        if (movingRight)
        {
            horizontalSpeed += playerAcc;
        }
        if (movingUp)
        {
            verticalSpeed += playerAcc;
        }
        if (movingDown)
        {
            verticalSpeed -= playerAcc;
        }

        if (inputX < 0)
        {
            horizontalSpeed = -baseSpeed;
            movingLeft = true;
            movingRight = false;
        }
        if (inputX > 0)
        {
            horizontalSpeed = baseSpeed;
            movingRight = true;
            movingLeft = false;
        }
        if (inputX == 0)
        {
            movingLeft = false;
            movingRight = false;
        }
        if (inputY < 0)
        {
            verticalSpeed = -baseSpeed;
            movingDown = true;
            movingUp = false;
        }
        if (inputY > 0)
        {
            verticalSpeed = baseSpeed;
            movingUp = true;
            movingDown = false;
        }
        if (inputY == 0)
        {
            movingDown = false;
            movingUp = false;
        }

        inputX = MathF.Abs(inputX);
        inputY = MathF.Abs(inputY);


        Vector3 result = new(inputX, inputY, 0);
        
        return result;
    }

    void TranslatePlayer(Vector3 xyz)
    {
        float xSpeed = xyz.x * horizontalSpeed;
        float ySpeed = xyz.y * verticalSpeed;
        transform.Translate(xSpeed, ySpeed, 0);
    }

    void UpdatePlayerSpeed()
    {
    
    }
   

    
}
