using System;
using UnityEngine;

public enum CharacterTag
{
    CharacterOne,
    CharacterTwo
}

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private InputGroup inputGroup;
    [SerializeField] private PlayerCharacter otherCharacter;
    [field:SerializeField] public CharacterTag CharacterTag { get; private set; }
    [Space]
    [SerializeField] private float connectionRadius;
    
    private MovementModule movementModule;
    private WeaponModule weaponModule;

    private int currentExperience;
    private int maxExperience = 10;

    public bool Empowered {get; private set;}
    public int Level { get; private set; } = 1;

    
    private void Start()
    {
        movementModule = GetComponent<MovementModule>();
        weaponModule = GetComponent<WeaponModule>();
    }

    private void Update()
    {
        var distance = Vector2.Distance(transform.position, otherCharacter.transform.position);

        Empowered = distance <= connectionRadius * 2;
        weaponModule.SetEmpowered(Empowered);
        
        if(!inputGroup || !movementModule) return;
        
        movementModule.Move(inputGroup.MovementDirection);
    }

    public void ReceiveExp(int expAmount)
    {
        currentExperience += expAmount;

        if (currentExperience >= maxExperience)
        {
            currentExperience -= maxExperience;
            Level++;
            maxExperience = Mathf.RoundToInt(maxExperience * (1 + Level / 10f));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, connectionRadius);
    }
}
