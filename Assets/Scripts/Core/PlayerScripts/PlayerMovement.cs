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

    public bool movingLeft  { get; private set; }
    public bool movingRight { get; private set; }
    public bool movingUp    { get; private set; }
    public bool movingDown  { get; private set; }

    private Rigidbody2D playerRb;
    private PlayerIdentity playerStats;


    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerIdentity>();

        playerStats.Acceleration.OnValueChanged += ReloadStats;
        playerStats.MovementSpeed.OnValueChanged += ReloadStats;

        playerAcc = playerStats.ReadStatValueByType(StatType.Acceleration);
        maxSpeed = playerStats.ReadStatValueByType(StatType.MovementSpeed);
        baseSpeed = maxSpeed * SpeedFactor;

        
    }

    void ReloadStats(Stat stat)
    {
        if (stat.Type == StatType.MovementSpeed)
        {
            maxSpeed = playerStats.ReadStatValueByType(StatType.MovementSpeed);
            baseSpeed = maxSpeed * SpeedFactor;
        }
        else if (stat.Type == StatType.Acceleration)
        {

            playerAcc = playerStats.ReadStatValueByType(StatType.Acceleration);
        }
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
