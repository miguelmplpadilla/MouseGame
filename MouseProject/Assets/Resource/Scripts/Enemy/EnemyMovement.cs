using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Vector2 movement;

    public float speed = 1;
    public float minSpeed = 1;
    public float maxSpeed = 2;

    public float jumpForce = 3;
    public float jumpForcePared = 4;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private PlayerGroundController groundController;

    public bool saltandoParedes = false;
    public bool aireSaltandoPared = false;

    public float distanciaInicial;
    public float distancia;

    public GameObject player;
    public PlayerMovement playerScript;

    public float delay;

    public float delayOffset;

    public LayerMask playerLayer;

    public LayerMask enemyLayer;

    public GameObject marca;

    public int IparedJumpPoint;
    public int IJumpPoint;

    public bool recuperandoPosicion;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundController = gameObject.GetComponentInChildren<PlayerGroundController>();

        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();

        distanciaInicial = Vector3.Distance(player.transform.position, transform.position);
    }

    private void Start()
    {
        
    }

    void Update()
    {
        //Physics.GetIgnoreLayerCollision(enemyLayer, playerLayer);

        distancia = Vector3.Distance(transform.position, player.transform.position);

       

        if (distancia > 5f)
        {
            /*
            playerScript.IJumpPoint = 0;
            playerScript.IparedJumpPoint = 0;
            IJumpPoint = 0;
            IparedJumpPoint = 0;
            speed = 1;
            */

            transform.position = player.transform.position - new Vector3(4.5f, 0, 0);
            recuperandoPosicion = true;

        }
        if (distancia < distanciaInicial)
        {

            maxSpeed = 1.9f;
            minSpeed = 0.9f;

            if (speed == 2)
            {
                speed = 1.9f;
            }

            if (distancia < 0.5)
            {

                speed = minSpeed;


            }

        }
        else
        {
            maxSpeed = 2;
            minSpeed = 1;
        }



        if (recuperandoPosicion == true)
        {
            maxSpeed = 2;
            minSpeed = 1;

            if (distancia > distanciaInicial)
            {
                speed = maxSpeed;
            }
            else
            {
                speed = minSpeed;
                recuperandoPosicion = false;
            }
        }



        movimiento();

        saltarPared();

        if (Input.GetButtonDown("Jump"))
        {

            //Invoke("EjecutarSalto", delay);

            
        }

        if (Vector3.Distance(playerScript.jumpPoint[IJumpPoint], transform.position)<0.04)
        {
            EjecutarSalto();
            playerScript.jumpPoint[IJumpPoint] = Vector3.zero;
            IJumpPoint++;

            if (IJumpPoint>=10)
            {
                IJumpPoint = 0;
            }
            
        }

        comprobarPared();

        animator.SetFloat("verticalVelocity", rigidbody.velocity.y);

        animator.SetBool("deslizando", saltandoParedes);

        animator.SetFloat("speed", speed);

        if (speed == maxSpeed)
        {
            animator.SetBool("isRun", true);
            animator.SetBool("isWalk", false);
        }
        else
        {
            animator.SetBool("isRun", false);
            animator.SetBool("isWalk", true);
        }
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

        float distanciaMinima = 99999;

        int IJumpPointMasCercano=0;

        for (int i = 0; i < 10; i++)
        {
            if (playerScript.jumpPoint[i].x > transform.position.x)
            {
                float distanciaFor = Vector3.Distance(playerScript.jumpPoint[i], transform.position);
                if (distanciaFor < distanciaMinima)
                {
                    IJumpPointMasCercano = i;
                    distanciaMinima = distanciaFor;
                }
            }
           
        }

        IJumpPoint = IJumpPointMasCercano;

        ////

        float distanciaMinimaPared = 99999;

        int IJumpPointMasCercanoPared = 0;

        for (int i = 0; i < 10; i++)
        {

            float distanciaFor = Vector3.Distance(playerScript.paredJumpPoint[i], transform.position);
            if (distanciaFor < distanciaMinimaPared)
            {
                IJumpPointMasCercanoPared = i;
                distanciaMinimaPared = distanciaFor;
            }


        }

        IparedJumpPoint = IJumpPointMasCercanoPared;

    }

    public void movimiento()
    {

            if (Vector3.Distance(playerScript.runPoint, transform.position) < 0.05)
            {
                SetRun();
                playerScript.runPoint = Vector3.zero;
            }
            else if (Vector3.Distance(playerScript.walkPoint, transform.position) < 0.05)
            {
                SetWalk();
                playerScript.walkPoint = Vector3.zero;
            }



        float horizontalvelocity = movement.normalized.x * speed;

        rigidbody.velocity =
            transform.TransformDirection(new Vector3(horizontalvelocity, rigidbody.velocity.y, 0));

        animator.SetBool("run", true);
    }

    public void SetRun()
    {
        speed = maxSpeed;
    }

    public void SetWalk()
    {
        speed = minSpeed;
    }


    public void saltar(float fuerzaSalto)
    {
        rigidbody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        animator.SetTrigger("jump");
    }

    public void EjecutarSalto()
    {
        if (groundController.isGrounded)
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Instantiate(marca, transform.position, Quaternion.identity);
            animator.SetTrigger("jump");
        }
        
    }

    public void EjecutarSaltoPared()
    {
        
        if (saltandoParedes)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            movement = new Vector2(-movement.x, movement.y);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,
                transform.localScale.z);

            rigidbody.AddForce(Vector2.up * jumpForcePared, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
            //speed = minSpeed;
        }

    }

    public void saltarPared()
    {
        if (Vector3.Distance(playerScript.paredJumpPoint[IparedJumpPoint], transform.position) < 0.05)
        {
            EjecutarSaltoPared();
            playerScript.paredJumpPoint[IparedJumpPoint] = Vector3.zero;
            IparedJumpPoint++;
            if (IparedJumpPoint >= 10) { IparedJumpPoint = 0; }
            
        }
    }

    public void comprobarPared()
    {
        Vector2 direccionRay = new Vector2(transform.localScale.x, 0);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direccionRay, 0.1f, 1 << 6);

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
