using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCollect : MonoBehaviour, IInteractable
{
    [SerializeField]
    Portal toOpen;
    [SerializeField]
    int levelToComplete;

    CreatureController controller;

    void Awake()
    {
        controller = GetComponentInParent<CreatureController>();
    }

    public void Interact()
    {
        if (!PlayerController.Instance.Creatures.Contains(controller))
            PlayerController.Instance.AddCreature(controller.Data, controller);
        toOpen.SetOpen(true);
        int prevLevels = PlayerPrefs.GetInt("LevelsComplete", 0);
        PlayerPrefs.SetInt("LevelsComplete", Mathf.Max(prevLevels, levelToComplete));
    }
}
