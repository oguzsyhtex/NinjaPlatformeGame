using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainPlayer : MonoBehaviour
{
    protected Animator anim;

    [SerializeField]
    protected GameObject knifePrefab;

    [SerializeField]
    public float moveSpeed;


    protected bool facingRight;

    protected bool Attack;



    // Start is called before the first frame update
    public virtual void Start()
    {
        facingRight = true;
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;

        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public virtual void ThrowKnife(int value)
    {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            tmp.GetComponent<Knife>().initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            tmp.GetComponent<Knife>().initialize(Vector2.left);
        }
    }
}
