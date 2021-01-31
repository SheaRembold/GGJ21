using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Data/AttackData")]
public class AttackData : ScriptableObject
{
    public string title = "Basic";
    public Color color = Color.white;
    public bool auto;
    public float cooldown = 1f;
    public Projectile projectile;
}
