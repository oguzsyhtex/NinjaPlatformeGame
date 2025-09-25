using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MainPlayer
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar;


    private Rigidbody2D rb;

    private bool Slide;

    private bool jump;

    private bool Throw;


    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Transform[] groundPoints;


    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatisGround;

    private bool isGround_bool;

    private bool jumpAttack;

    private Vector3 startPos;
    private PlayerAttack playerAttack;

    
    private float idleTimer = 0f;
    public float idleThresold = 2f;
    private bool isSleeping = false;



    // Start is called before the first frame update
    public override void Start()
    {
        
        if (anim = null)
        {
            Debug.LogError("Animator yok");
        }

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        startPos = transform.position;

        base.Start();
       
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponentInChildren<PlayerAttack>();

    }

    private void Update()
    {
        if (Input.anyKey)
        {
            idleTimer = 0f;

            if (isSleeping)
            {
                anim.SetBool("sleep", false);
                isSleeping = false;
                
            }
        }
        else
        {
            idleTimer += Time.deltaTime;

            if(idleTimer>=idleThresold && !isSleeping)
            {
                anim.SetBool("sleep", true);
                isSleeping = true;
                
            }
        }

        if (transform.position.y<=-14f)
        {
            rb.velocity = Vector2.zero;
            transform.position = startPos;
        }
        HandleInput();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        isGround_bool = isGrounded();

        float horizontal = Input.GetAxis("Horizontal");

        HandleMovement(horizontal);

        HandleAttacks();

        Flip(horizontal);

        HandleLayers();

        ResetValues();
    }

    private void HandleMovement(float horizontal)
    {
        if (rb.velocity.y<0)
        {
            anim.SetBool("land",true);
        }

        if (!this.anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGround_bool || airControl)) 
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        if (isGround_bool&& jump)
        {
            isGround_bool = false;
            rb.AddForce(new Vector2(0, jumpForce));
            anim.SetTrigger("jump");
        }


        anim.SetFloat("speed", Mathf.Abs(horizontal));

        if (Slide&&!this.anim.GetCurrentAnimatorStateInfo(0).IsName("slide"))
        {
            anim.SetBool("slide", true);

        }
        else if (!Slide && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("slide"))
        {
            anim.SetBool("slide", false);
        }
    }

    private void HandleAttacks()
    {
        if (Attack && isGround_bool && !this.anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            anim.SetTrigger("attack");
            rb.velocity = Vector2.zero;
        }

        if (jumpAttack&&!this.anim.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            anim.SetTrigger("attack");
        }
        if(!jumpAttack&& !this.anim.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            anim.ResetTrigger("attack");
        }


        if (Throw && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("throw"))
        {
            anim.SetTrigger("throw");
            ThrowKnife(0);
        }

    }


    private void Flip(float horizontal)
    {
        if (horizontal>0 && !facingRight || horizontal<0 &&facingRight)
        {
            ChangeDirection();

        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack= true;
            jumpAttack = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Slide = true;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Throw= true;
            
        }
    }

    private bool isGrounded()
    {
        if (rb.velocity.y<=0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius,whatisGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        anim.ResetTrigger("jump");
                        anim.SetBool("land",false);

                        if (isSleeping)
                        {
                            anim.SetBool("sleep", false);
                            isSleeping = false;
                        }

                        
                        return true;
                    }
                }
            }
        }
        return false;
        
    }

    private void HandleLayers()
    {
        if (!isGround_bool)
        {
            anim.SetLayerWeight(1, 1);
        }
        else
        {
            anim.SetLayerWeight(1, 0);
        }
    }

    public override void ThrowKnife(int value)
    {
        base.ThrowKnife(value);
    }


    private void ResetValues()
    {
        Attack = false;
        Slide = false;
        jump = false;
        jumpAttack = false;
        Throw = false;
        
    }

    public void EnableAttack()
    {
        playerAttack.EnableAttack();
    }

    public void DisableAttack()
    {
        playerAttack.DisableAttack();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.value = currentHealth;

        if (currentHealth <= 0) 
        {
            Die();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gold"))
        {
            GoldManager.instance.AddGold(1);
            Destroy(other.gameObject);
        }
    }


    void Die()
    {
        Destroy(gameObject);
    }



}
