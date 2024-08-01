using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOptionOne : MonoBehaviour
{
    public float baseSpeed = 0.5f;
    public float playerAcc = 0.01f;
    public float playerDcc = 0.5f;
    public float maxSpeed  = 1.0f;

    private float horizontalSpeed;
    private float verticalSpeed;
    private Vector3 inputTracker = new(0,0,0);
    public Vector3 trackerVolatility = new(0.0025f, 0.0025f, 0);

    private bool volatileTrackerX;
    private bool volatileTrackerY;

    void Start()
    {
        horizontalSpeed = baseSpeed;
        verticalSpeed = baseSpeed;
    }


    void Update()
    {
        
    }

    void FixedUpdate()
    {
        TranslatePlayer(ReadInputs());
        UpdateInputTracker(ReadInputs());
        UpdatePlayerSpeed();
    }

    Vector3 ReadInputs()
    {
        float inputX   = Input.GetAxis("Horizontal");
        float inputY   = Input.GetAxis("Vertical");
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
        if (!volatileTrackerX)
        {
            if (horizontalSpeed < maxSpeed)
            {
                horizontalSpeed += playerAcc;
            }
        }
        if (!volatileTrackerY)
        {
            if (verticalSpeed < maxSpeed)
            {
                verticalSpeed += playerAcc;
            }
        }
        if (volatileTrackerX)
        {
            if (horizontalSpeed > baseSpeed)
            {
                horizontalSpeed -= playerDcc;
            }
        }
        if (volatileTrackerY)
        {
            if (verticalSpeed > baseSpeed)
            {
                verticalSpeed -= playerDcc;
            }
        }
    }

    void UpdateInputTracker(Vector3 xyz)
    {
        Vector3 restingInput = new(0,0,0);
        
        if (inputTracker == xyz)
        {
            return;
        }

        if (inputTracker == restingInput)
        {
            inputTracker = xyz;
        }

        if (((inputTracker.x > 0) && (xyz.x < 0))
            || ((inputTracker.x < 0) && (xyz.x > 0)))
        {
            volatileTrackerX = true;
        }
        else
        {
            volatileTrackerX = false;
        }

        if (((inputTracker.y > 0) && (xyz.y < 0))
            || ((inputTracker.y < 0) && (xyz.y > 0)))
        {
            volatileTrackerY = true;
        }
        else
        {
            volatileTrackerY = false;
        }

        if (volatileTrackerX)
        {
            float diffX = inputTracker.x - xyz.x;
            if (diffX > 0)
            {
                volatileTrackerX = true;
                if (diffX > trackerVolatility.x)
                {
                    inputTracker.x -= trackerVolatility.x;
                }
                else
                {
                    inputTracker.x -= diffX;
                    volatileTrackerX = false;
                }
            }
            else if (diffX < 0)
            {
                volatileTrackerX = true;
                if (diffX < trackerVolatility.x)
                {
                    inputTracker.x += trackerVolatility.x;
                }
                else
                {
                    inputTracker.x += diffX;
                    volatileTrackerX = false;
                }
            }
            else if (diffX == 0)
            {
                volatileTrackerX = false;
            }
        }

        if (volatileTrackerY)
        {
            float diffY = inputTracker.y - xyz.y;
            if (diffY > 0)
            {
                volatileTrackerY = true;
                if (diffY > trackerVolatility.y)
                {
                    inputTracker.y -= trackerVolatility.y;
                }
                else
                {
                    inputTracker.y -= diffY;
                    volatileTrackerY = false;
                }
            }
            else if (diffY < 0)
            {
                volatileTrackerY = true;
                if (diffY < trackerVolatility.y)
                {
                    inputTracker.y += trackerVolatility.y;
                }
                else
                {
                    inputTracker.y += diffY;
                    volatileTrackerY = false;
                }
            }
            else if (diffY == 0)
            {
                volatileTrackerY = false;
            }
        }
    }
}
