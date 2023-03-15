using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public PlayerPoints playerPoints;

    public float delay;

    public bool pararse;

    public float delayOffset;

    public LayerMask playerLayer;

    public LayerMask enemyLayer;

    public GameObject marca;

    private PlayerGanchoController playerGanchoController;
    private EnemyDeslizarController playerDeslizarController;
    private PlayerBordeController playerBordeController;

    public int IparedJumpPoint;
    public int IJumpPoint;
    public int IRunPoint;

    public bool recuperandoPosicion;

    public float estamina;

    public bool canJumpWall;
    public bool jugadorCerca;

    public float ultimaVelocidad;
    public Vector3 ultimaPosicion;

    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundController = gameObject.GetComponentInChildren<PlayerGroundController>();

        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        playerPoints = player.GetComponent<PlayerPoints>();

        playerGanchoController = GetComponent<PlayerGanchoController>();
        playerDeslizarController = GetComponentInChildren<EnemyDeslizarController>();
        playerBordeController = GetComponentInChildren<PlayerBordeController>();

        distanciaInicial = Vector3.Distance(player.transform.position, transform.position);


    }

    private void Start()
    {
        
    }

    void Update()
    {
        //Physics.GetIgnoreLayerCollision(enemyLayer, playerLayer);

        distancia = Vector3.Distance(transform.position, player.transform.position);

        movimiento();

        saltarPared();

        //if (!groundController.isGrounded) { estamina = 150; }


        if (distancia > 4f || player.transform.position.x + 2 < transform.position.x)
        {

            transform.position = player.transform.position - new Vector3(3.5f, 0, 0);
            recuperandoPosicion = true;

        }

        if (distancia < 0.2)
        {
            print("Te han pillado");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("Dead");

        }
        else if (distancia < 0.6 && saltandoParedes == false && aireSaltandoPared == false)
        {  

            if (!jugadorCerca)
            {
                if (groundController.isGrounded) speed = 0.9f;
                print("CercaDelJugador");
                ultimaVelocidad = speed;
                pararse = true;
                jugadorCerca = true;
            }

            jugadorCerca = true;
        }
        else if (distancia > 0.6)
        {
            jugadorCerca = false;
        }

        if (distancia < 1)
        {
            if (speed == 2) { speed = 1.9f; }
            else if (speed == 1) { speed = 0.9f; };
        }
        else
        {
            if (speed == 1.9f) { speed = 2f; }
            else if (speed == 0.9f && recuperandoPosicion == false) { speed = 1f; };
        }

        if (distancia > distanciaInicial + 2)
        {
            speed = 1.9f;
            recuperandoPosicion = true;
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
                recuperandoPosicion = false;
            }

        }

        float distanciaSalto = Vector3.Distance(playerPoints.jumpPoint[IJumpPoint], transform.position);

        if ((distanciaSalto < 0.03 && (speed == 1.9f || speed == 2)) || (distanciaSalto < 0.015 && (speed == 0.9f || speed == 1))) //SALTAR
        {
            EjecutarSalto();

        }
        else if (distanciaSalto < 0.05 && playerPoints.jumpPoint[IJumpPoint].x < transform.position.x )
        {
            EjecutarSalto();
        }

       
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

        comprobarPared();
        EncontrarPuntosCercanos();

        if (transform.position == ultimaPosicion)
        {
            EjecutarSalto();
        }
        else
        {
            ultimaPosicion = transform.position;
        }
    }

    private void LateUpdate()
    {
        actualizarEstamina();
        //estamina = 150;

    }

    public void EncontrarPuntosCercanos()
    {
        float distanciaMinima = 99999;

        int IJumpPointMasCercano = 0;

        for (int i = 0; i < 10; i++)
        {
            if (playerPoints.jumpPoint[i].x + 1 > transform.position.x)
            {
                float distanciaFor = Vector3.Distance(playerPoints.jumpPoint[i], transform.position);
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

            float distanciaFor = Vector3.Distance(playerPoints.paredJumpPoint[i], transform.position);
            if (distanciaFor < distanciaMinimaPared)
            {
                IJumpPointMasCercanoPared = i;
                distanciaMinimaPared = distanciaFor;
            }

        }

        IparedJumpPoint = IJumpPointMasCercanoPared;

        //

        float distanciaMinimaRun = 99999;

        int IRunPointMasCercano = 0;

        for (int i = 0; i < 10; i++)
        {

            float distanciaFor = Vector3.Distance(playerPoints.runPoint[i], transform.position);
            if (distanciaFor < distanciaMinimaPared)
            {
                IRunPointMasCercano = i;
                distanciaMinimaRun = distanciaFor;
            }

        }

        IRunPoint = IRunPointMasCercano;


    }

    public void movimiento()
    {

            if (Vector3.Distance(playerPoints.runPoint[IRunPoint], transform.position) < 0.03)
            {
                SetRun();
                playerPoints.runPoint[IRunPoint] = Vector3.zero;
            }
            else if (Vector3.Distance(playerPoints.walkPoint, transform.position) < 0.03)
            {
                if (groundController.isGrounded)
                {
                    SetWalk();
                    
                }
                else
                {
                    pararse = true;
                }
                 playerPoints.walkPoint = Vector3.zero;
             }

        if (pararse == true && groundController.isGrounded && saltandoParedes == false && aireSaltandoPared == false)
        {
            SetWalk();
            pararse = false;
        }

        float horizontalvelocity = movement.normalized.x * speed;

        rigidbody.velocity =
            transform.TransformDirection(new Vector3(horizontalvelocity, rigidbody.velocity.y, 0));

        animator.SetBool("run", true);
    }

    public void SetRun()
    {
        print("EnemyRun");
        speed = maxSpeed;
        speed = 2;
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
            //Instantiate(marca, transform.position, Quaternion.identity);
            animator.SetTrigger("jump");
        }

        if (speed == 1.9f) { speed = 2; }
        if (speed == 0.9f) { speed = 1; }

        if (jugadorCerca == true)
        {
            speed = ultimaVelocidad;
        }

        //Invoke("EjecutarSalto", 0.025f);
        playerPoints.jumpPoint[IJumpPoint] = Vector3.zero;

        if (IJumpPoint >= 10)
        {
            IJumpPoint = 0;
        }

    }

    public void saltarPared()
    {

        float distance = Vector3.Distance(playerPoints.paredJumpPoint[IparedJumpPoint], transform.position);
        if (distance < 0.15)
        {
            
            if (jugadorCerca)
            {
                speed = ultimaVelocidad;
            }

            if (speed == 1.9f) speed = 2;
            else if (speed == 0.9f || speed == 1) speed = 1.3f;


            //speed = 2;

            pararse = true;
            canJumpWall = true;
            saltandoParedes = true;
            EjecutarSaltoPared();
            playerPoints.paredJumpPoint[IparedJumpPoint] = Vector3.zero;
            IparedJumpPoint++;
            if (IparedJumpPoint >= 10) { IparedJumpPoint = 0; }

        }
    }

    public void EjecutarSaltoPared()
    {

        if (canJumpWall && saltandoParedes)
        {

            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);

            rigidbody.AddForce(Vector2.up * jumpForcePared, ForceMode2D.Impulse);
            animator.SetTrigger("jump");

            movement = new Vector2(-movement.x, movement.y);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,
                transform.localScale.z);
            canJumpWall = false;
        }



    }

    

    public void actualizarEstamina()
    {
        if (!playerBordeController.enganchadoBorde && !playerDeslizarController.deslizandoSuelo)
        {
            if (Input.GetButton("Fire1"))
            {
                if (estamina > 0)
                {
                    estamina -= 100f * Time.deltaTime;
                }
                else
                {
                    estamina = -150;
                }
            }
            else
            {
                if (estamina < 150)
                {
                    estamina += 80f * Time.deltaTime;
                }
                else
                {
                    estamina = 150;
                }
            }
        }
        else
        {
            if (estamina < 150)
            {
                estamina += 80f * Time.deltaTime;
            }
            else
            {
                estamina = 150;
            }
        }

    }

    public void comprobarPared()
    {
        Vector2 direccionRay = new Vector2(transform.localScale.x, 0);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direccionRay, 0.16f, 1 << 6);

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

    public void salirBorde()
    {
        transform.parent = null;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;

        if (speed == 0.9) speed = 1;
        if (speed == 1.9) speed = 2;

        if (jugadorCerca) speed = ultimaVelocidad;

        rigidbody.AddForce(Vector2.up * jumpForcePared, ForceMode2D.Impulse);
        animator.SetTrigger("jump");
        playerBordeController.enganchadoBorde = false;
    }

}
