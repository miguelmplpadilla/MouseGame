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
    private EnemyMovement enemyMovement;
    private DistanceJoint2D distanceJoint;

    public float speedGancho = 2;

    private RaycastHit2D hitInfo;

    private GameObject[] ganchosEscena;
    private GameObject ganchoCercano;

    public GameObject player;
    public PlayerPoints playerPoints;

    public int IGanchoPoint;
    public int IBreackGanchoPoint;

    private void Awake()
    {

        player = GameObject.Find("Player");
        playerPoints = player.GetComponent<PlayerPoints>();
        groundController = GetComponentInChildren<PlayerGroundController>();
        enemyMovement = GetComponent<EnemyMovement>();
        distanceJoint = GetComponent<DistanceJoint2D>();

    }

    private void Start()
    {

        ganchoLineRenderer = gancho.GetComponent<LineRenderer>();
        puntoEngancheVirtual = GameObject.Find("PuntoEngancheVirtualEnemy");
        distanceJoint.connectedBody = puntoEngancheVirtual.GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        Vector2 direccionRay = new Vector2(1, 0.6f);

        if (!groundController.isGrounded && !enemyMovement.aireSaltandoPared && puedeDispararGancho)
        {
            if (!ganchoLanzado && !enganchado)
            {
                if (Vector3.Distance(playerPoints.ganchoPoint[IGanchoPoint], transform.position) < 0.06f)
                {
                    playerPoints.ganchoPoint[IGanchoPoint] = Vector3.zero;
                    StartCoroutine("tiempoGanchoLanzado");
                    hitInfo = Physics2D.Raycast(transform.position, direccionRay, 10000, 1 << 6);
                    enemyMovement.speed = 2;
                    puntoEngancheVirtual.transform.position = hitInfo.point;

                    ganchoLanzado = true;
                }
            }
        }


        Debug.DrawRay(transform.position, direccionRay, Color.red);

        if (enganchado)
        {
            if (Vector3.Distance(playerPoints.breackGanchoPoint[IBreackGanchoPoint], transform.position) < 0.07f)
            {

                playerPoints.breackGanchoPoint[IBreackGanchoPoint] = Vector3.zero;
                gancho.transform.parent = transform;
                gancho.transform.position = puntoLanzamientoGancho.transform.position;

                distanceJoint.enabled = false;

                enemyMovement.saltar(enemyMovement.jumpForce);

                enganchado = false;
            }
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
    }

    private void FixedUpdate()
    {

        if (ganchoLanzado)
        {
            gancho.transform.parent = null;
            gancho.transform.position = Vector3.MoveTowards(gancho.transform.position, hitInfo.point, speedGancho * Time.deltaTime);

            float distanciaGanchoEnganche = Vector2.Distance(gancho.transform.position, hitInfo.point);

            if (distanciaGanchoEnganche < 0.05f)
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

        ganchoLineRenderer.SetPosition(0, puntoLanzamientoGancho.transform.position);
        ganchoLineRenderer.SetPosition(1, gancho.transform.position);
    }

    public void SoltarseGanchoTeleport()
    {
        //playerPoints.breackGanchoPoint[IBreackGanchoPoint] = Vector3.zero;
        gancho.transform.parent = transform;
        gancho.transform.position = puntoLanzamientoGancho.transform.position;

        distanceJoint.enabled = false;

        //enemyMovement.saltar(enemyMovement.jumpForce);

        enganchado = false;
    }

    private IEnumerator tiempoGanchoLanzado()
    {
        yield return new WaitForSeconds(2f);

        if (!enganchado)
        {
            gancho.transform.parent = transform;
            gancho.transform.position = puntoLanzamientoGancho.transform.position;

            enganchado = false;
            ganchoLanzado = false;
        }
    }
}
