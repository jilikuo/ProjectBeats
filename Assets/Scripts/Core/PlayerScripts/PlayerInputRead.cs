using UnityEngine;

public interface IPlayerInput
{
    Vector3 ReadInput();
}

public class PlayerInputRead : MonoBehaviour, IPlayerInput
{
    public Vector3 ReadInput()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if (input.magnitude > 1)
            input.Normalize();
        return input;
    }
}
