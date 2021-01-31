using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 20f;
    [SerializeField]
    int damage = 10;

    Vector2 dir;
    bool hit;

    public void Launch(Transform target)
    {
        dir = target.position - transform.position;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)(dir.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit)
            return;

        AiController controller = collision.GetComponentInParent<AiController>();
        if (controller != null && controller.IsAlive)
        {
            controller.TakeDamage(damage);
            hit = true;
            Destroy(gameObject);
        }
    }
}
