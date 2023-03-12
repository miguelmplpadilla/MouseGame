using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movement;

    public float speed = 1;
    public float jumpForce = 1;

    private Rigidbody2D rigidbody;
    private Animator animator;
    public PlayerGroundController groundScript;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundScript = gameObject.GetComponentInChildren<PlayerGroundController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        movimiento();
        
        saltar();
    }

    public void movimiento()
    {
        if (Input.GetButton("Fire1"))
        {
            speed = 2;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            speed = 1;
        }
            
        movement = new Vector2(1, 0f);
            
        float horizontalvelocity = movement.normalized.x * speed;

        /*
        rigidbody.velocity =
            transform.TransformDirection(new Vector3(horizontalvelocity, rigidbody.velocity.y, 0));
        */


        rigidbody.velocity = new Vector3(horizontalvelocity, rigidbody.velocity.y, 0);

        animator.SetBool("run", true);
    }

    public void saltar()
    {
        if (Input.GetButtonDown("Jump"))
        {
            
            if (groundScript.isGrounded == true)
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetTrigger("jump");
            }
        }
        
        animator.SetFloat("verticalVelocity", rigidbody.velocity.y);
    }
}
