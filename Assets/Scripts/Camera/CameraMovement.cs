using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Transform mainCamera;

    private Vector3 playerPos;
    private Vector3 cameraPos;

    public float horizontalBoundary = 5;
    public float verticalBoundary = 3;

    private void Start()
    {
        playerPos = target.transform.position;
        cameraPos = mainCamera.transform.position;
        mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y, cameraPos.z);
    }

    void LateUpdate()
    {
        playerPos = target.transform.position;
        cameraPos = mainCamera.transform.position;

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
    }
}
