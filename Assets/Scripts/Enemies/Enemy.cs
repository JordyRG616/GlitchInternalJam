using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Die = Animator.StringToHash("Die");

    public Signal<Enemy> OnDeathAnimationDone = new();
    
    [SerializeField] private float damage;
    [SerializeField] private Transform visual;
    
    private Vector3 visualPosition;
    
    private DropManager dropManager;
    
    private MovementModule movementModule;
    private HealthModule healthModule;
    private Animator animator;

    private PlayerCharacter firstCharacter;
    private PlayerCharacter secondCharacter;

    public bool Moving { get; set; } = true;
    

    private void Start()
    {
        dropManager = FractaGlobal.GetManager<DropManager>();
        
        movementModule = GetComponent<MovementModule>();
        healthModule = GetComponent<HealthModule>();
        animator = GetComponentInChildren<Animator>();
        
        healthModule.OnDamageTaken += PlayHitAnimation;
        healthModule.OnDeath += TriggerDeathAnimation;
        
        var playerCharacters = FindObjectsByType<PlayerCharacter>(FindObjectsSortMode.None);
        firstCharacter = playerCharacters[0];
        secondCharacter = playerCharacters[1];
        
        visualPosition = visual.localPosition;
    }

    private void OnEnable()
    {
        Moving = true;
    }

    private void PlayHitAnimation(HealthData healthData)
    {
        animator.SetTrigger(Hit);
        Moving = false;
    }

    private void TriggerDeathAnimation(HealthModule _)
    {
        animator.SetTrigger(Die);
        visual.SetParent(null, true);
        gameObject.SetActive(false);
    }

    public void DropExperience()
    {
        var killer = healthModule.LastAttacker.GetComponent<PlayerCharacter>();
        if (killer != null)
        {
            var tag = killer.CharacterTag;
            var drop = dropManager.RequestExperienceDrop();
            drop.Setup(1, tag, transform.position);
        }
        
        visual.SetParent(transform);
        visual.localPosition = visualPosition;
        OnDeathAnimationDone.Fire(this);
    }

    private void Update()
    {
        if(!Moving) return;
        
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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out HealthModule healthModule))
        {
            healthModule.TakeDamage(damage, gameObject);
            gameObject.SetActive(false);
        }
    }
}
