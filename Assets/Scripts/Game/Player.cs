using UnityEngine;

public class Player : MonoBehaviour
{
    public float movePower = 5f;

    Rigidbody2D rigid;
    SpriteRenderer render;
    Animator animator;

    Vector3 movement;

    //---------------------------------------------------[Override Function]
    //Initialization
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        render = gameObject.GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    //Graphic & Input Updates	
    void Update()
    {
       
    }

    //Physics engine Updates
    void FixedUpdate()
    {
        Move();
    }

    //---------------------------------------------------[Movement Function]

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("walk", false);
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;

            render.flipX = false;

            animator.SetBool("walk", true);
        }

        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;

            render.flipX = true;

            animator.SetBool("walk", true);
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            moveVelocity = Vector3.down;

            animator.SetBool("walk", true);
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            moveVelocity = Vector3.up;

            animator.SetBool("walk", true);
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }


}




