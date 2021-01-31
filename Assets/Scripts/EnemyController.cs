using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AiController
{
    [SerializeField]
    AttackData mainAttack;
    [SerializeField]
    GameObject highlight;
    [SerializeField]
    GameObject select;

    float lastAttack = float.MinValue;

    protected override void Awake()
    {
        base.Awake();
        highlight.SetActive(false);
        select.SetActive(false);
    }

    void Update()
    {
        if (mainAttack != null && detector.detected.Count > 0 && Time.time - lastAttack >= attackDelay && Time.time - lastAttack >= mainAttack.cooldown)
        {
            Attack(detector.detected[Random.Range(0, detector.detected.Count)].transform, mainAttack);
            lastAttack = Time.time;
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
}
