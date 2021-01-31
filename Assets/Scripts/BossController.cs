using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : EnemyController
{
    [SerializeField]
    AttackData[] specialAttacks;
    [SerializeField]
    ConversationData finishedConversation;

    List<int> available = new List<int>();
    float[] lastSpecial;

    protected override void Awake()
    {
        base.Awake();
        lastSpecial = new float[specialAttacks.Length];
        for (int i = 0; i < lastSpecial.Length; i++)
            lastSpecial[i] = float.MinValue;
    }

    protected override void Update()
    {
        FindTargets();
        if (targets.Count > 1 && Time.time - lastAttack >= attackDelay)
        {
            available.Clear();
            for (int i = 0; i < specialAttacks.Length; i++)
            {
                if (Time.time - lastSpecial[i] >= specialAttacks[i].cooldown)
                    available.Add(i);
            }
            if (available.Count > 0)
            {
                int attack = available[Random.Range(0, available.Count)];
                Attack(targets[Random.Range(0, targets.Count)], specialAttacks[attack]);
                lastSpecial[attack] = Time.time;
            }
            else
            {
                Attack(targets[Random.Range(0, targets.Count)], mainAttack);
                lastAttack = Time.time;
            }
            PlayerController.Instance.AddEnemy(this);
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        ConversationManager.Instance.StartConversation(finishedConversation, () => { PlayerPrefs.SetInt("LevelsComplete", 0); PlayerPrefs.SetInt("ConversComplete", -1); SceneManager.LoadScene("MainMenu"); });
    }
}
