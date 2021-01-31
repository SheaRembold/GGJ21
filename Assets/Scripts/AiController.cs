using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth = 100;
    [SerializeField]
    protected ProgressBar healthBar;
    [SerializeField]
    protected float attackDelay = 1f;

    public bool IsAlive { get { return health > 0; } }

    protected Detector detector;
    protected int health;

    protected virtual void Awake()
    {
        detector = GetComponentInChildren<Detector>();
        health = maxHealth;
        healthBar.SetProgress(1f);
    }

    protected void Attack(Transform target, AttackData attack)
    {
        Projectile projectile = Instantiate(attack.projectile, transform.position, Quaternion.identity);
        projectile.Launch(target);
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        healthBar.SetProgress((float)health / maxHealth);

        if (health <= 0)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        enabled = false;
    }
}
