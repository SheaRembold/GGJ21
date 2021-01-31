using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressController : MonoBehaviour
{
    [SerializeField]
    ConversationData[] conversations;
    [SerializeField]
    Portal[] portals;
    [SerializeField]
    CreatureData[] creatures;

    void Start()
    {
        int level = PlayerPrefs.GetInt("LevelsComplete", 0);
        int creatureCount = Mathf.Clamp(level + 1, 0, creatures.Length);
        for (int i = 0; i < creatureCount; i++)
            PlayerController.Instance.AddCreature(creatures[i], null);
        if (level < portals.Length)
            portals[level].SetOpen(true);

        int convers = PlayerPrefs.GetInt("ConversComplete", -1);
        if (convers < level)
        {
            convers++;
            if (convers < conversations.Length)
            {
                ConversationManager.Instance.StartConversation(conversations[convers], () => { PlayerPrefs.SetInt("ConversComplete", convers); });
            }
            else
            {
                PlayerPrefs.SetInt("ConversComplete", convers);
            }
        }
    }
}
