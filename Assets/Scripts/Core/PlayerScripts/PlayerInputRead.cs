using UnityEngine;

public class PlayerInputRead : MonoBehaviour
{
    public Vector3 ReadInput()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if (input.magnitude > 1)
            input.Normalize();
        return input;
    }
}
