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

    private bool saltandoParedes = false;
    private bool aireSaltandoPared = false;

    public GameObject marca;

    public Vector3[] paredJumpPoint;
    public int IparedJumpPoint;

    public Vector3[] jumpPoint;
    public int IJumpPoint;

    public Vector3 runPoint;
    public Vector3 walkPoint;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundController = gameObject.GetComponentInChildren<PlayerGroundController>();

        paredJumpPoint = new Vector3[10];
        jumpPoint = new Vector3[10];
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
                jumpPoint[IJumpPoint] = transform.position;
                IJumpPoint++;
                if (IJumpPoint>=10)
                {
                    IJumpPoint = 0;
                }
            }
        }
        
        animator.SetFloat("verticalVelocity", rigidbody.velocity.y);
        
        animator.SetBool("deslizando", saltandoParedes);
        
        animator.SetFloat("speed", speed);

        comprobarPared();
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

        
    }

    public void movimiento()
    {

                if (Input.GetButtonDown("Fire1"))
                {
                    speed = maxSpeed;
                    runPoint = transform.position;
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    speed = minSpeed;
                    walkPoint = transform.position;
                }


        float horizontalvelocity = movement.normalized.x * speed;

        rigidbody.velocity =
            transform.TransformDirection(new Vector3(horizontalvelocity, rigidbody.velocity.y, 0));

        animator.SetBool("run", true);
    }

    public void saltar(float fuerzaSalto)
    {
        rigidbody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        GameObject instancia = Instantiate(marca, transform.position, Quaternion.identity);
        instancia.name = "MarcaPlayer";
        animator.SetTrigger("jump");
    }

    public void saltarPared()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (saltandoParedes)
            {
                paredJumpPoint[IparedJumpPoint] = transform.position;
                IparedJumpPoint++;
                if (IparedJumpPoint >= 10) { IparedJumpPoint = 0; }

                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                saltar(jumpForcePared);
                movement = new Vector2(-movement.x, movement.y);
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,
                    transform.localScale.z);
                //speed = minSpeed;
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
