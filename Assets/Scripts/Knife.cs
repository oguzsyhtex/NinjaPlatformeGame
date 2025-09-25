using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyHealth = other.GetComponent<Enemy>();

            if (enemyHealth !=null)
            {
                enemyHealth.TakeDamage(15);
            }
            Destroy(gameObject);  
        }
    }
}
