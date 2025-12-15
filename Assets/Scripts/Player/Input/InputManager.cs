using UnityEngine;
using System;

public class InputManager : FractaManager
{
    private InputSystem_Actions inputSystem_Actions;

    [SerializeField] private InputGroup FirstCharacterInputs;
    [SerializeField] public InputGroup SecondCharacterInputs;

    
    private void Start()
    {
        inputSystem_Actions = new InputSystem_Actions();
        
        var firstCharMove = inputSystem_Actions.Player_1.Move;
        FirstCharacterInputs.Clear();
        FirstCharacterInputs.SetMovementAction(firstCharMove);
        
        var secondCharMove = inputSystem_Actions.Player_2.Move;
        SecondCharacterInputs.Clear();
        SecondCharacterInputs.SetMovementAction(secondCharMove);
    }
}