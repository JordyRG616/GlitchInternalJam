using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ArsenalController : SerializedMonoBehaviour
{
    [SerializeField] private ArsenalDatabase arsenalDatabase;
    [ShowInInspector] private List<Weapon> weapons = new List<Weapon>();
    
    public bool Empowered { get; private set; }
    
    
    private void Start()
    {
        var initialWeapon = arsenalDatabase.GetRandomWeapon();
        initialWeapon.Initialize(this);
        weapons.Add(initialWeapon);
    }

    private void Update()
    {
        foreach (var weapon in weapons)
        {
            var target = GetTarget(weapon.detectionRadius);
            weapon.Aim(target);
        }
    }
    
    private Transform GetTarget(float distance)
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, distance, LayerMask.GetMask("Enemy"));

        if(enemies.Length == 0) return null;
        
        var orderedEnemies = enemies.OrderBy(x => Vector3.Distance(transform.position, x.transform.position));
        return orderedEnemies.First().transform;
    }

    public void SetEmpowered(bool empowered)
    {
        if(empowered == Empowered) return;
        Empowered = empowered;
        
        weapons.ForEach(x => x.SetEmpowered(empowered));
    }

    public void ReceiveWeapon(Weapon weapon)
    {
        weapon.Initialize(this);
        weapons.Add(weapon);
    }
}
