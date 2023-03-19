using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGanchoController : MonoBehaviour
{
    public bool ganchoLanzado = false;
    public bool enganchado = false;
    public bool puedeDispararGancho = false;

    public GameObject puntoLanzamientoGancho;
    public GameObject gancho;
    public GameObject puntoEngancheVirtual;
    private LineRenderer ganchoLineRenderer;

    private PlayerGroundController groundController;
    private EnemyDeslizarController enemyDeslizar;
    private EnemyMovement enemyMovement;
    private DistanceJoint2D distanceJoint;

    public Animator animator;
    private Rigidbody2D rigidbody;

    public float speedGancho = 2;

    private RaycastHit2D hitInfo;

    private GameObject[] ganchosEscena;
    private GameObject ganchoCercano;

    public GameObject player;
    public PlayerPoints playerPoints;

    public int IGanchoPoint;
    public int IBreackGanchoPoint;

    public float ultimaVelocidad;

    public float distanciaSaltarGancho;

    private void Awake()
    {

        player = GameObject.Find("Player");
        playerPoints = player.GetComponent<PlayerPoints>();
        groundController = GetComponentInChildren<PlayerGroundController>();
        enemyMovement = GetComponent<EnemyMovement>();
        distanceJoint = GetComponent<DistanceJoint2D>();

        enemyDeslizar = GetComponentInChildren<EnemyDeslizarController>();

        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {

        ganchoLineRenderer = gancho.GetComponent<LineRenderer>();
        puntoEngancheVirtual = GameObject.Find("PuntoEngancheVirtualEnemy");
        distanceJoint.connectedBody = puntoEngancheVirtual.GetComponent<Rigidbody2D>();

        gancho.SetActive(false);

    }

    void Update()
    {

        Vector2 direccionRay = new Vector2(1, 0.6f);
        animator.SetFloat("velocidadHorizontal", rigidbody.velocity.x);

        distanciaSaltarGancho = Vector3.Distance(playerPoints.ganchoPoint[IGanchoPoint], transform.position);

        if (distanciaSaltarGancho < 0.15f)
        {
            gancho.SetActive(true);
            playerPoints.ganchoPoint[IGanchoPoint] = Vector3.zero;
            animator.SetTrigger("lanzarGancho");
            StartCoroutine("tiempoGanchoLanzado");
            hitInfo = Physics2D.Raycast(transform.position, direccionRay, 10000, 1 << 6);
            ultimaVelocidad = enemyMovement.speed;
            enemyMovement.speed = 1;
            puntoEngancheVirtual.transform.position = hitInfo.point;
            ganchoLanzado = true;

        }



        Debug.DrawRay(transform.position, direccionRay, Color.red);

        if (enganchado)
        {

            Vector3 dir = puntoEngancheVirtual.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            if (Vector3.Distance(playerPoints.breackGanchoPoint[IBreackGanchoPoint], transform.position) < 0.05f)
            {
                gancho.SetActive(false);
                playerPoints.breackGanchoPoint[IBreackGanchoPoint] = Vector3.zero;
                gancho.transform.parent = transform;
                gancho.transform.position = puntoLanzamientoGancho.transform.position;

                enemyMovement.speed = 3.1f;


                distanceJoint.autoConfigureDistance = true;
                distanceJoint.enabled = false;
                enemyMovement.saltar(0.6f);
                //enemyMovement.saltar(2);

                enganchado = false;
            }
            
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        float distanciaMinimaGancho = 99999;

        int IGanchoPointMasCercano = 0;

        for (int i = 0; i < 10; i++)
        {

            float distanciaFor = Vector3.Distance(playerPoints.ganchoPoint[i], transform.position);
            if (distanciaFor < distanciaMinimaGancho)
            {
                IGanchoPointMasCercano = i;
                distanciaMinimaGancho = distanciaFor;
            }


        }

        IGanchoPoint = IGanchoPointMasCercano;

        ///////////////////////

        float distanciaMinimaBreakGancho = 99999;

        int IBreakGanchoPointMasCercano = 0;

        for (int i = 0; i < 10; i++)
        {

            float distanciaFor = Vector3.Distance(playerPoints.breackGanchoPoint[i], transform.position);
            if (distanciaFor < distanciaMinimaBreakGancho)
            {
                IBreakGanchoPointMasCercano = i;
                distanciaMinimaBreakGancho = distanciaFor;
            }

        }

        IBreackGanchoPoint = IBreakGanchoPointMasCercano;

        animator.SetBool("enganchado", enganchado);

        ganchoLineRenderer.SetPosition(0, puntoLanzamientoGancho.transform.position);
        ganchoLineRenderer.SetPosition(1, gancho.transform.position);
    }

    private void FixedUpdate()
    {

        Vector2 direccionRay2 = new Vector2(1, 0.6f);
        animator.SetFloat("velocidadHorizontal", rigidbody.velocity.x);

        if (!ganchoLanzado && !enganchado)
        {
            if (Vector3.Distance(playerPoints.ganchoPoint[IGanchoPoint], transform.position) < 0.2f)
            {
                gancho.SetActive(true);
                playerPoints.ganchoPoint[IGanchoPoint] = Vector3.zero;
                animator.SetTrigger("lanzarGancho");
                StartCoroutine("tiempoGanchoLanzado");
                hitInfo = Physics2D.Raycast(transform.position, direccionRay2, 10000, 1 << 6);
                ultimaVelocidad = enemyMovement.speed;
                enemyMovement.speed = 1;
                puntoEngancheVirtual.transform.position = hitInfo.point;


                ganchoLanzado = true;
            }
        }

        if (ganchoLanzado)
        {
            gancho.transform.parent = null;
            gancho.transform.position = Vector3.MoveTowards(gancho.transform.position, hitInfo.point, speedGancho * Time.deltaTime);

            float distanciaGanchoEnganche = Vector2.Distance(gancho.transform.position, hitInfo.point);

            if (distanciaGanchoEnganche < 0.2f)
            {
                if (hitInfo.collider.tag.Equals("Enganche"))
                {
                    distanceJoint.enabled = true;

                    enganchado = true;
                }
                else
                {
                    gancho.transform.parent = transform;
                    gancho.transform.position = puntoLanzamientoGancho.transform.position;
                }

                ganchoLanzado = false;
            }
        }

        Vector2 direccionRay = new Vector2(1, 0.6f);
        RaycastHit2D hitInfoComprobar = Physics2D.Raycast(transform.position, direccionRay, 10000, 1 << 6);

        if (hitInfoComprobar.collider != null && hitInfoComprobar.collider.tag.Equals("Enganche"))
        {
            puedeDispararGancho = true;
        }
        else
        {
            puedeDispararGancho = false;
        }

        
    }

    public void SoltarseGanchoTeleport()
    {
        //playerPoints.breackGanchoPoint[IBreackGanchoPoint] = Vector3.zero;
        gancho.transform.parent = transform;
        gancho.transform.position = puntoLanzamientoGancho.transform.position;

        distanceJoint.enabled = false;

        //enemyMovement.saltar(enemyMovement.jumpForce);

        enganchado = false;

        comprobarLanzarGancho();

    }


    private void comprobarLanzarGancho()
    {
        Vector2 direccionRay = new Vector2(1, 0.6f);
        RaycastHit2D hitInfoComprobar = Physics2D.Raycast(transform.position, direccionRay, 10000, 1 << 6);

        float distanciaGancho = Vector2.Distance(hitInfo.point, transform.position);

        RaycastHit2D hitInfoComprobarPared = Physics2D.Raycast(enemyDeslizar.puntoRayCast.transform.position, Vector2.left, 0.01f, 1 << 6);

        Debug.DrawRay(transform.position, Vector2.left, Color.red);

        if (!enganchado)
        {
            if (hitInfoComprobar.collider != null && hitInfoComprobar.collider.tag.Equals("Enganche"))
            {

                //puedeDispararGancho = true;
            }
            else
            {

                if (!ganchoLanzado)
                {
                    //puedeDispararGancho = false;
                }
            }
        }

        if (groundController.isGrounded ||
            (hitInfoComprobarPared.collider != null && hitInfoComprobarPared.collider.tag.Equals("Ground")))
        {
            Debug.Log("Puede disparar false 2");
            //puedeDispararGancho = false;
        }

        if (ganchoLanzado)
        {
            Debug.Log(distanciaGancho);

            if (!groundController.isGrounded && distanciaGancho >= 2f && !enganchado)
            {
                Debug.Log("Puede disparar false 3");
                //puedeDispararGancho = false;
            }
        }
    }

    private IEnumerator tiempoGanchoLanzado()
    {
        yield return new WaitForSeconds(0.8f);

        if (!enganchado)
        {
            Debug.Log("He entrado tiempoGanchoLanzado()");
            distanceJoint.autoConfigureDistance = true;
            gancho.transform.parent = transform;
            gancho.transform.position = puntoLanzamientoGancho.transform.position;

            enganchado = false;
            ganchoLanzado = false;
        }
        else
        {
            gancho.SetActive(false);
            gancho.transform.parent = transform;
            gancho.transform.position = puntoLanzamientoGancho.transform.position;

            distanceJoint.enabled = false;

            enemyMovement.saltar(0.6f);
            enemyMovement.speed = 3.1f;

            enganchado = false;
        }
    }
}
