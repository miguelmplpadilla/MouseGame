using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movement;

    public float speed = 1;
    public float minSpeed = 1;
    public float maxSpeed = 2;
    
    public float jumpForce = 1;
    public float jumpForcePared = 1;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private PlayerGroundController groundController;
    private PlayerGanchoController playerGanchoController;

    private bool saltandoParedes = false;
    public bool aireSaltandoPared = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundController = GetComponentInChildren<PlayerGroundController>();
        playerGanchoController = GetComponent<PlayerGanchoController>();
    }

    void Update()
    {
        movimiento();
        
        saltarPared();

        if (Input.GetButtonDown("Jump"))
        {
            if (groundController.isGrounded)
            {
                saltar(jumpForce);
            }
        }
        
        animator.SetFloat("verticalVelocity", rigidbody.velocity.y);
        
        animator.SetBool("deslizando", saltandoParedes);
        
        animator.SetFloat("speed", speed);
    }

    private void FixedUpdate()
    {
        if (groundController.isGrounded)
        {
            aireSaltandoPared = false;
            saltandoParedes = false;
            transform.localScale = new Vector3(1, 1, 1);
            movement = new Vector2(1, 0f);
        }

        comprobarPared();
    }

    public void movimiento()
    {
        if (!aireSaltandoPared)
        {
            if (groundController.isGrounded)
            {
                if (Input.GetButton("Fire1"))
                {
                    speed = maxSpeed;
                }
                else
                {
                    speed = minSpeed;
                }
            }
        }
        else
        {
            speed = maxSpeed;
        }

        float horizontalvelocity = movement.normalized.x * speed;

        if (!playerGanchoController.enganchado)
        {
            rigidbody.velocity =
                transform.TransformDirection(new Vector3(horizontalvelocity, rigidbody.velocity.y, 0));
        }
        else
        {
            transform.Translate(movement * Time.deltaTime * speed);
        }

        animator.SetBool("run", true);
    }

    public void saltar(float fuerzaSalto)
    {
        rigidbody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        animator.SetTrigger("jump");
    }

    public void saltarPared()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (saltandoParedes)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                saltar(jumpForcePared);
                movement = new Vector2(-movement.x, movement.y);
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,
                    transform.localScale.z);
            }
        }
    }

    public void comprobarPared()
    {
        Vector2 direccionRay = new Vector2(transform.localScale.x, 0);
        
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direccionRay,0.1f, 1 << 6);

        Debug.DrawRay(transform.position, direccionRay, Color.red);

        if (hitInfo.collider != null && hitInfo.collider.tag.Equals("Ground"))
        {
            if (!groundController.isGrounded)
            {
                aireSaltandoPared = true;
                saltandoParedes = true;
            }
        }
        else
        {
            saltandoParedes = false;
        }
    }
}
