using UnityEngine;
using Jili.StatSystem;
using Jili.StatSystem.EntityTree;

public interface IPlayerMovement
{
    void HandleDirectionChange(Vector3 input);
    void AcceleratePlayer();
    void MovePlayer();
}

[RequireComponent(typeof(PlayerInputRead)), RequireComponent(typeof(PlayerIdentity))]
public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    private readonly float        SpeedFactor = 0.6f;
    private readonly float DecelarationFactor = 0.3f;

    public float baseSpeed;
    public float maxSpeed;

    public float playerAcc;
    public float horizontalSpeed;
    public float verticalSpeed;

    private bool movingLeft = false;
    private bool movingRight = false;
    private bool movingUp = false;
    private bool movingDown = false;

    private Rigidbody2D playerRb;
    private PlayerIdentity playerStats;


    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerIdentity>();

        playerAcc = playerStats.ReadStatValueByType(StatType.Acceleration);
        maxSpeed = playerStats.ReadStatValueByType(StatType.MovementSpeed);
        baseSpeed = maxSpeed * SpeedFactor;
    }

    public void HandleDirectionChange(Vector3 input)
    {
        if (input.x < 0 && !movingLeft)
        {
            horizontalSpeed = -baseSpeed;
            movingLeft = true;
            movingRight = false;
        }
        else if (input.x > 0 && !movingRight)
        {
            horizontalSpeed = baseSpeed;
            movingRight = true;
            movingLeft = false;
        }
        else if (input.x == 0)
        {
            movingLeft = movingRight = false;
            horizontalSpeed *= DecelarationFactor;
            if (Mathf.Abs(horizontalSpeed) < 0.01f) horizontalSpeed = 0;
        }

        if (input.y < 0 && !movingDown)
        {
            verticalSpeed = -baseSpeed;
            movingDown = true;
            movingUp = false;
        }
        else if (input.y > 0 && !movingUp)
        {
            verticalSpeed = baseSpeed;
            movingUp = true;
            movingDown = false;
        }
        else if (input.y == 0)
        {
            movingUp = movingDown = false;
            verticalSpeed *= DecelarationFactor;
            if (Mathf.Abs(verticalSpeed) < 0.01f) verticalSpeed = 0;
        }
    }

    public void AcceleratePlayer()
    {
        if (movingLeft)
        {
            horizontalSpeed = Mathf.Min(horizontalSpeed - (playerAcc * Time.fixedDeltaTime), -baseSpeed + (playerAcc * Time.fixedDeltaTime));
        }
        if (movingRight)
        {
            horizontalSpeed = Mathf.Max(horizontalSpeed + (playerAcc * Time.fixedDeltaTime), baseSpeed - (playerAcc * Time.fixedDeltaTime));
        }
        if (movingUp)
        {
            verticalSpeed = Mathf.Max(verticalSpeed + (playerAcc * Time.fixedDeltaTime), baseSpeed - (playerAcc * Time.fixedDeltaTime));
        }
        if (movingDown)
        {
            verticalSpeed = Mathf.Min(verticalSpeed - (playerAcc * Time.fixedDeltaTime), -baseSpeed + (playerAcc * Time.fixedDeltaTime));
        }

        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);
        verticalSpeed = Mathf.Clamp(verticalSpeed, -maxSpeed, maxSpeed);
    }

    public void MovePlayer()
    {
        Vector2 velocity = new Vector2(horizontalSpeed, verticalSpeed);
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        playerRb.velocity = velocity;
    }
}
