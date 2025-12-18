using System;
using UnityEngine;

public enum CharacterTag
{
    CharacterOne,
    CharacterTwo
}

public class PlayerCharacter : MonoBehaviour
{
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Die = Animator.StringToHash("Die");

    public Signal<ExpData> OnExpCollected = new Signal<ExpData>();
    public Signal OnLevelUp = new Signal();
    
    [SerializeField] private InputGroup inputGroup;
    [SerializeField] private PlayerCharacter otherCharacter;
    [field:SerializeField] public CharacterTag CharacterTag { get; private set; }
    [Space]
    [SerializeField] private float connectionRadius;
    
    private MovementModule movementModule;
    public HealthModule healthModule;
    private ArsenalController arsenalController;
    private Animator animator;

    private int currentExperience;
    private int maxExperience = 10;

    public bool Empowered {get; private set;}
    public int Level { get; private set; } = 1;

    
    private void Start()
    {
        movementModule = GetComponent<MovementModule>();
        healthModule = GetComponent<HealthModule>();
        arsenalController = GetComponent<ArsenalController>();
        animator = GetComponentInChildren<Animator>();

        var otherHealth = otherCharacter.GetComponent<HealthModule>();
        otherHealth.OnDamageTaken += BroadcastDamage;
        
        healthModule.OnDeath += PlayDeathAnimation;
    }

    private void BroadcastDamage(HealthData healthData)
    {
        healthModule.TakeDamageWithoutNotify(healthData.damageTaken, healthData.attacker);
    }

    private void Update()
    {
        CheckEmpoweredState();

        if(!inputGroup || !movementModule) return;

        Move();
    }

    private void Move()
    {
        var direction = inputGroup.MovementDirection.normalized;
        if(Mathf.Abs(direction.x) > 0f)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);

        var walking = direction.magnitude > 0;
        animator.SetBool(Walking, walking);
        
        movementModule.Move(direction);
    }

    private void CheckEmpoweredState()
    {
        var distance = Vector2.Distance(transform.position, otherCharacter.transform.position);

        Empowered = distance <= connectionRadius * 2;
        arsenalController.SetEmpowered(Empowered);
    }

    public void ReceiveExp(int expAmount)
    {
        currentExperience += expAmount;

        if (currentExperience >= maxExperience)
        {
            currentExperience -= maxExperience;
            Level++;
            maxExperience = Mathf.RoundToInt(maxExperience * (1 + Level / 10f));
            
            OnLevelUp.Fire();
        }
        
        OnExpCollected.Fire(new ExpData(currentExperience, maxExperience, Level));       
    }
    
    private void PlayDeathAnimation(HealthModule _)
    {
        animator.SetTrigger(Die);
        // TODO Game Over
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, connectionRadius);
    }
}


public struct ExpData
{
    public float experience;
    public int maxExperience;
    public int level;


    public ExpData(int experience, int maxExperience, int level)
    {
        this.experience = experience;
        this.maxExperience = maxExperience;
        this.level = level;
    }
}