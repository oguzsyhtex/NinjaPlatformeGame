using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public Collider2D attackCollider;
    // Start is called before the first frame update
    void Start()
    {
        attackCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("attack");
        }
    }

    public void EnableAttack()
    {
        attackCollider.enabled = true;
    }


    public void DisableAttack()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyHealth = other.GetComponent<Enemy>();
            if (enemyHealth!=null)
            {
                enemyHealth.TakeDamage(25);
            }
        }
    }
}
