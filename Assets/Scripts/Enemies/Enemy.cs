using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float damage;
    
    private MovementModule movementModule;

    private PlayerCharacter firstCharacter;
    private PlayerCharacter secondCharacter;
    

    private void Start()
    {
        movementModule = GetComponent<MovementModule>();
        var playerCharacters = FindObjectsByType<PlayerCharacter>(FindObjectsSortMode.None);
        firstCharacter = playerCharacters[0];
        secondCharacter = playerCharacters[1];
    }

    private void Update()
    {
        Move();
    }

    public Transform GetTarget()
    {
        var dist1 = Vector3.Distance(transform.position, firstCharacter.transform.position);
        var dist2 = Vector3.Distance(transform.position, secondCharacter.transform.position);
        
        var closest = dist1 < dist2 ? firstCharacter.transform : secondCharacter.transform;
        
        return closest;
    }

    public void Move()
    {
        var target = GetTarget();
        var direction = target.position - transform.position;
        direction.Normalize();
        
        movementModule.Move(direction);
        transform.up = direction;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out HealthModule healthModule))
        {
            healthModule.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
