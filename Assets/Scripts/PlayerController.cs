using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 0.5f;
    public float playerAcc = 0.01f;
    public float playerDcc = 0.5f; //
    public float maxSpeed  = 1.0f;

    private float horizontalSpeed;
    private float verticalSpeed;
    private Vector3 inputTracker = new Vector3(0,0,0);
    private Vector3 trackerOperator = new Vector3(0.25f, 0.25f, 0);

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
        UpdatePlayerSpeed(ReadInputs());
    }

    Vector3 ReadInputs()
    {
        float inputX   = Input.GetAxis("Horizontal");
        float inputY   = Input.GetAxis("Vertical");
        Vector3 result = new Vector3(inputX, inputY, 0);
        
        return result;
    }

    void TranslatePlayer(Vector3 xyz)
    {
        float xSpeed = xyz.x * horizontalSpeed;
        float ySpeed = xyz.y * verticalSpeed;
        transform.Translate(xSpeed, ySpeed, 0);
    }

    void UpdatePlayerSpeed(Vector3 xyz)
    {
        if (xyz == inputTracker)
        {
            if (horizontalSpeed <= maxSpeed)
            {
                horizontalSpeed = horizontalSpeed + playerAcc;
            }
            if (verticalSpeed <= maxSpeed)
            {
                verticalSpeed = verticalSpeed + playerAcc;
            }
        }

        else
        {
            if (xyz.x > inputTracker.x) { }

        }
    }

    void UpdateInputTracker(Vector3 xyz)
    {
        Vector3 restingInput = new Vector3(0,0,0);
        
        if (inputTracker == xyz)
        {
            return;
        }

        if (inputTracker == restingInput)
        {
            inputTracker = xyz;
        }

        if (inputTracker.x != xyz.x)
        {
            volatileTrackerX = true;
        }

        if (inputTracker.y != xyz.y)
        {
            volatileTrackerY = true;
        }

        if (volatileTrackerX)
        {
            float diffX = inputTracker.x - xyz.x;
            if (diffX > 0)
            {
                volatileTrackerX = true;
                if (diffX > trackerOperator.x)
                {
                    inputTracker.x = inputTracker.x - trackerOperator.x;
                }
                else
                {
                    inputTracker.x = inputTracker.x - diffX;
                    volatileTrackerX = false;
                }
            }
            else if (diffX < 0)
            {
                volatileTrackerX = true;
                if (diffX < trackerOperator.x)
                {
                    inputTracker.x = inputTracker.x + trackerOperator.x;
                }
                else
                {
                    inputTracker.x = inputTracker.x + diffX;
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
                if (diffY > trackerOperator.y)
                {
                    inputTracker.y = inputTracker.y - trackerOperator.y;
                }
                else
                {
                    inputTracker.y = inputTracker.y - diffY;
                    volatileTrackerY = false;
                }
            }
            else if (diffY < 0)
            {
                volatileTrackerY = true;
                if (diffY < trackerOperator.y)
                {
                    inputTracker.y = inputTracker.y + trackerOperator.y;
                }
                else
                {
                    inputTracker.y = inputTracker.y + diffY;
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
