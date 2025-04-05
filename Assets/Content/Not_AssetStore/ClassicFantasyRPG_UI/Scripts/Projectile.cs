using System;
using Interacting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10;
    public Rigidbody2D rb;
    public LayerMask affects;
    public int AttackDamage { private get; set; }

    private void Start()
    {
        var right = transform.right;
        var facing = right.x / Math.Abs(right.x);
        rb.velocity = new Vector2(facing * speed, 0f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var otherGameObject = other.gameObject;
        if (affects != (affects | (1 << otherGameObject.layer))) {
            return;
        }
        var durable = otherGameObject.GetComponent<Durable>();
        if (durable != null && durable.enabled)
        {
            durable.ApplyDamage(AttackDamage);
        }
        Destroy(gameObject);
    }
}