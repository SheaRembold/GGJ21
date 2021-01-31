using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : AiController
{
    [SerializeField]
    float minSpeed = 10f;
    [SerializeField]
    float maxSpeed = 10f;
    [SerializeField]
    float maxDistance = 1f;

    public CreatureData Data { get; set; }

    float lastAttack = float.MinValue;
    AiController target;

    QuickAction queuedAction;

    void Update()
    {
        if (queuedAction != null)
        {
            queuedAction.lastTime = Time.time;
        }
        if ((target != null && target.IsAlive || detector.detected.Count > 0) && Time.time - lastAttack >= attackDelay)
        {
            if (target == null || !target.IsAlive)
            {
                target = detector.detected[0];
                float closestDist = Vector2.Distance(transform.position, detector.detected[0].transform.position);
                for (int i = 0; i < detector.detected.Count; i++)
                {
                    float dist = Vector2.Distance(transform.position, detector.detected[i].transform.position);
                    if (dist < closestDist)
                    {
                        target = detector.detected[i];
                        closestDist = dist;
                    }
                }
            }
            if (queuedAction != null)
            {
                Attack(target.transform, queuedAction.attack);
                queuedAction = null;
            }
            else
            {
                Attack(target.transform, Data.BasicAttack);
            }
            lastAttack = Time.time;
        }
    }

    public void ManualAttack(EnemyController target, QuickAction action)
    {
        if (target != null)
        {
            this.target = target;
        }
        if (action.attack != Data.BasicAttack)
        {
            queuedAction = action;
            queuedAction.lastTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        Vector2 target = PlayerController.Instance.GetCreaturePosition(this);
        Vector2 dir = target - (Vector2)transform.position;
        float speed = Mathf.Lerp(minSpeed, maxSpeed, dir.magnitude / maxDistance);
        if (dir.magnitude < speed * Time.deltaTime)
            transform.position = target;
        else
            transform.position += (Vector3)(dir.normalized * speed * Time.deltaTime);
    }
}
