using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions inputActions;

    private void Start()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }
}
