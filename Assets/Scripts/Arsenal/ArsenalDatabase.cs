using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Arsenal/Database", fileName = "Arsenal Database")]
public class ArsenalDatabase : SerializedScriptableObject
{
    [SerializeField] private List<Weapon> weapons;


    public Weapon GetRandomWeapon()
    {
        return weapons.TakeRandom().GetInstance();
    }
}