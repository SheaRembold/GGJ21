using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Data/CreatureData")]
public class CreatureData : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Color Color = Color.white;
    public CreatureController Prefab;
    public AttackData BasicAttack;
    public AttackData SpecialAttack;
}
