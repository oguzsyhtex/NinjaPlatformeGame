using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public LayerMask obstacleLayer;
    public Transform groundCheck;
    public float checkDistance = 0.1f;

    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar;


    private int moveDirection = -1;

    private Animator anim;

    public int damage = 20;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;



   
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        anim = GetComponent<Animator>();    
    }

    
    void Update()
    {
        transform.Translate(Vector2.right * moveDirection * moveSpeed * Time.deltaTime);

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.right * moveDirection, checkDistance, obstacleLayer);

        if (hit.collider !=null)
        {
            moveDirection *= -1;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        
    }

    private void OnDrawGizmos()
    {
        if (groundCheck!=null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.right * moveDirection * checkDistance);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            anim.SetBool("EnemyisAttack",true);
            Invoke("ResetAttack", 0.5f);
         }


    }

    void ResetAttack()
    {
        anim.SetBool("EnemyisAttack", false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth<=0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void DealDamage()
    {
        Collider2D hitplayer = Physics2D.OverlapCircle(attackPoint.position, attackRange,playerLayer);

        if (hitplayer!=null)
        {
            PlayerController playerHealth = hitplayer.GetComponent<PlayerController>();
            if (playerHealth!=null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }


}
