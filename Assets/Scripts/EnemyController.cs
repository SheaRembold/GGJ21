using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AiController
{
    [SerializeField]
    protected AttackData mainAttack;
    [SerializeField]
    protected GameObject highlight;
    [SerializeField]
    protected GameObject select;
    [SerializeField]
    protected float speed = 10f;

    protected float lastAttack = float.MinValue;
    protected List<EnemyController> party = new List<EnemyController>();
    protected List<CreatureController> targets = new List<CreatureController>();
    protected CreatureController lastTarget;

    protected override void Awake()
    {
        base.Awake();
        highlight.SetActive(false);
        select.SetActive(false);
    }

    protected void FindTargets()
    {
        party.Clear();
        targets.Clear();
        for (int i = 0; i < detector.detected.Count; i++)
        {
            if (detector.detected[i] is EnemyController && detector.detected[i] != this)
                party.Add(detector.detected[i] as EnemyController);
            else if (detector.detected[i] is CreatureController)
                targets.Add(detector.detected[i] as CreatureController);
        }
        for (int i = 0; i < party.Count; i++)
        {
            if (party[i].lastTarget != null)
                targets.Add(party[i].lastTarget);
        }
    }

    protected virtual void Update()
    {
        FindTargets();
        if (mainAttack != null && targets.Count > 0 && Time.time - lastAttack >= attackDelay && Time.time - lastAttack >= mainAttack.cooldown)
        {
            lastTarget = targets[Random.Range(0, targets.Count)];
            Attack(lastTarget, mainAttack);
            lastAttack = Time.time;
            PlayerController.Instance.AddEnemy(this);
        }
    }

    void FixedUpdate()
    {
        if (lastTarget != null && lastTarget.IsAlive)
        {
            Vector2 targetPos = lastTarget.transform.position;
            Vector2 dir = targetPos - (Vector2)transform.position;
            if (dir.magnitude > attackDistance)
                transform.position += (Vector3)(dir.normalized * speed * Time.deltaTime);
        }
    }

    public void SetHighlighted(bool selected)
    {
        highlight.SetActive(selected);
    }

    public void SetSelected(bool selected)
    {
        select.SetActive(selected);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        PlayerController.Instance.RemoveEnemy(this);
    }
}
