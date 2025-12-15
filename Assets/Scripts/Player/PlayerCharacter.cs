using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private InputGroup inputGroup;
    [SerializeField] private PlayerCharacter otherCharacter; 
    [Space]
    [SerializeField] private float connectionRadius;
    
    private MovementModule movementModule;
    private WeaponModule weaponModule;

    private void Start()
    {
        movementModule = GetComponent<MovementModule>();
        weaponModule = GetComponent<WeaponModule>();
    }

    private void Update()
    {
        var distance = Vector2.Distance(transform.position, otherCharacter.transform.position);
        
        weaponModule.SetEmpowered(distance <= connectionRadius * 2);
        
        if(!inputGroup || !movementModule) return;
        
        movementModule.Move(inputGroup.MovementDirection);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, connectionRadius);
    }
}
