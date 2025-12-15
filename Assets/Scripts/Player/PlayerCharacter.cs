using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private InputGroup inputGroup;
    
    private MovementModule movementModule;


    private void Start()
    {
        movementModule = GetComponent<MovementModule>();
    }

    private void Update()
    {
        if(!inputGroup || !movementModule) return;
        
        movementModule.Move(inputGroup.MovementDirection);
    }
}
