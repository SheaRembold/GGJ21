using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quickbar : MonoBehaviour
{
    [SerializeField]
    QuickItem[] quickItems;

    List<CreatureController> creatures = new List<CreatureController>();

    private void OnEnable()
    {
        creatures.Clear();
        for (int i = 0; i < PlayerController.Instance.Creatures.Count; i++)
            AddActions(PlayerController.Instance.Creatures[i]);
        for (int i = creatures.Count * 2; i < quickItems.Length; i++)
            quickItems[i].Init(UnityEngine.InputSystem.Key.Digit1 + i, null);
        PlayerController.OnCreatureAdd += AddActions;
    }

    private void OnDisable()
    {
        PlayerController.OnCreatureAdd -= AddActions;
    }

    void AddActions(CreatureController creature)
    {
        quickItems[creatures.Count * 2].Init(UnityEngine.InputSystem.Key.Digit1 + creatures.Count * 2, new QuickAction() { creature = creature, attack = creature.Data.BasicAttack, lastTime = Time.time - creature.Data.BasicAttack.cooldown });
        quickItems[creatures.Count * 2 + 1].Init(UnityEngine.InputSystem.Key.Digit1 + creatures.Count * 2 + 1, new QuickAction() { creature = creature, attack = creature.Data.SpecialAttack, lastTime = Time.time - creature.Data.BasicAttack.cooldown });
        creatures.Add(creature);
    }
}
