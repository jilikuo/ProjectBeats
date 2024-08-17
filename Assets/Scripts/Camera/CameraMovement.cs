using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera thisCamera;
    private float defaultOrthographicSize;
    public GameObject target;
    public PlayerMovement movement;
    public Transform targetTransform;
    public Transform mainCamera;

    public int MoveRatio = 5; // == 1/10th of fixedDeltaTime

    private Vector3 playerPos;
    private Vector3 cameraPos;

    public float horizontalBoundary = 3;
    public float verticalBoundary = 2;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
        defaultOrthographicSize = thisCamera.orthographicSize;
        target = GameObject.FindGameObjectWithTag("Player");
        movement = target.GetComponent<PlayerMovement>();
        targetTransform = target.transform;
        playerPos = targetTransform.position;
        cameraPos = mainCamera.position;
        mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y, cameraPos.z);
    }

    void FixedUpdate()
    {
        TryToCenterScreen();
    }

    void LateUpdate()
    {
        playerPos = targetTransform.position;
        cameraPos = mainCamera.position;

        float moveX = 0;
        float moveY = 0;

        if (playerPos.x > cameraPos.x + horizontalBoundary)
        {
            moveX = playerPos.x - (cameraPos.x + horizontalBoundary);
        }
        else if (playerPos.x < cameraPos.x - horizontalBoundary)
        {
            moveX = playerPos.x - (cameraPos.x - horizontalBoundary);
        }

        if (playerPos.y > cameraPos.y + verticalBoundary)
        {
            moveY = playerPos.y - (cameraPos.y + verticalBoundary);
        }
        else if (playerPos.y < cameraPos.y - verticalBoundary)
        {
            moveY = playerPos.y - (cameraPos.y - verticalBoundary);
        }

        cameraPos.x += moveX;
        cameraPos.y += moveY;
        mainCamera.position = cameraPos;
        float hSpeed = target.GetComponent<PlayerMovement>().horizontalSpeed;
        float vSpeed = target.GetComponent<PlayerMovement>().verticalSpeed;
        float maxSpeed = target.GetComponent<PlayerMovement>().maxSpeed;
        thisCamera.orthographicSize = Mathf.MoveTowards(thisCamera.orthographicSize, defaultOrthographicSize + Mathf.Min(Mathf.Sqrt((hSpeed*hSpeed) + (vSpeed*vSpeed)), maxSpeed), Time.deltaTime / MoveRatio);
    }

    private void TryToCenterScreen()
    {
        playerPos = targetTransform.position;
        cameraPos = mainCamera.position;

        if (!movement.movingDown && !movement.movingUp)
        {
            if (cameraPos.y != playerPos.y)
            {
                cameraPos.y = Mathf.MoveTowards(cameraPos.y, playerPos.y, Time.fixedDeltaTime / MoveRatio);
            }
        }
        if (!movement.movingLeft && !movement.movingRight)
        {
            if (cameraPos.x != playerPos.x)
            {
                cameraPos.x = Mathf.MoveTowards(cameraPos.x, playerPos.x, Time.fixedDeltaTime / MoveRatio);
            }
        }

        mainCamera.position = cameraPos;
    }
}
