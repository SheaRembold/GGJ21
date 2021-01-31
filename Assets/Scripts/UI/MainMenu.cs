using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject startGameButton;
    [SerializeField]
    GameObject continueButton;
    [SerializeField]
    GameObject restartButton;

    void Awake()
    {
        int convers = PlayerPrefs.GetInt("ConversComplete", -1);
        if (convers > -1)
        {
            startGameButton.SetActive(false);

            continueButton.SetActive(true);
            restartButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
            restartButton.SetActive(false);

            startGameButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Continue()
    {
        SceneManager.LoadScene("Main");
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("LevelsComplete", 0);
        PlayerPrefs.SetInt("ConversComplete", -1);
        SceneManager.LoadScene("Main");
    }
}
