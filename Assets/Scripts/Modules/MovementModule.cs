using System;
using UnityEngine;

public class MovementModule : MonoBehaviour
{
    private Action<Vector2> movementCallback;

    [SerializeField] private float movementSpeed;
    
    private Rigidbody2D body;


    private void Start()
    {
        body = GetComponent<Rigidbody2D>();

        if (body == null)
            movementCallback = MoveTransform;
        else
            movementCallback = MoveBody;
    }

    public void Move(Vector2 direction)
    {
        movementCallback.Invoke(direction);
    }

    private void MoveTransform(Vector2 direction)
    {
        transform.position += (Vector3)direction * (movementSpeed * Time.deltaTime);;
    }

    private void MoveBody(Vector2 direction)
    {
        var pos = body.position + direction * (movementSpeed * Time.fixedDeltaTime);
        body.MovePosition(pos);
    }
}
