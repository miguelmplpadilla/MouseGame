using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Vector2 movement;

    public float speed = 1;
    public float jumpForce = 1;

    public float delay = 0.3f;
    public float distanciaInicial;
    public float distancia;

    private Rigidbody2D rigidbody;
    private Animator animator;
    public PlayerGroundController groundScript;

    public bool canJump;

    public GameObject player;
    public PlayerMovement playerScript;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    void Start()
    {
        distanciaInicial = Vector3.Distance(player.transform.position, transform.position);
    }

    void Update()
    {

        distancia = Vector3.Distance(player.transform.position, transform.position);

        if (distancia > 5 && playerScript.speed == 1)
        {
            transform.position = player.transform.position - new Vector3(distanciaInicial,0,0);
            speed = 1;
        }

        delay = distanciaInicial + 0.1f;

        movimiento();

        if (canJump == true)
        {
            saltar();
        }
    }

    public void movimiento()
    {
        if (Input.GetButton("Fire1"))
        {
            Invoke("SetRun", delay);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Invoke("SetWalk", delay);
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

    public void SetRun()
    {
        speed = 2;
    }

    public void SetWalk()
    {
        speed = 1;
    }

    public void saltar()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Invoke("EjecutarSalto", delay);
        }

        animator.SetFloat("verticalVelocity", rigidbody.velocity.y);
    }

    public void EjecutarSalto()
    {
       
        if (groundScript.isGrounded == true)
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
        }
        
    }
}
