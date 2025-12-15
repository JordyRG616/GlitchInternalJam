using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "System/Input Group", fileName = "New Input Group")]
public class InputGroup : ScriptableObject
{
    private InputAction move;

    public Vector2 MovementDirection => move.ReadValue<Vector2>();
    
    
    public void SetMovementAction(InputAction movementAction)
    {
        move = movementAction;
        move.Enable();
    }

    public void Clear()
    {
        move = null;
    }
}