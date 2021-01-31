using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField]
    string toScene;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetOpen(bool open)
    {
        animator.SetBool("IsOpen", open);
    }

    public void Interact()
    {
        SceneManager.LoadScene(toScene);
    }
}
