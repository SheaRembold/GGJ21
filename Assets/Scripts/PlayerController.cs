using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 10;

    public static PlayerController Instance;

    Rigidbody2D rigidbody;
    InputActionAsset actions;

    void Awake()
    {
        Instance = this;

        rigidbody = GetComponent<Rigidbody2D>();
        actions = GetComponent<PlayerInput>().actions;
    }

    void Update()
    {
        rigidbody.velocity = actions.FindAction("Move").ReadValue<Vector2>() * moveSpeed;
    }
}
