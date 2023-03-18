using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeslizarController : MonoBehaviour
{
    private EnemyDeslizarController playerMovement;
    private EnemyMovement movementScript;
    private GameObject player;
    private PlayerPoints playerPoints;
    private Animator animator;

    public GameObject puntoRayCast;

    public bool deslizandoSuelo = false;
    public bool paredSuperior = false;

    public int IDeslizarPoint;

    public float distancia;


    private void Awake()
    {
        playerMovement = GetComponentInParent<EnemyDeslizarController>();
        animator = GetComponentInParent<Animator>();

        movementScript = GetComponentInParent<EnemyMovement>();
        player = GameObject.Find("Player");
        playerPoints = player.GetComponent<PlayerPoints>();
    }

    private void FixedUpdate()
    {
        float distanciaMinima = 99999;

        int IJumpPointMasCercano = 0;

        for (int i = 0; i < 10; i++)
        {

            float distanciaFor = Vector3.Distance(playerPoints.deslizarPoint[i], transform.position);

            if (distanciaFor < distanciaMinima)
            {
                IJumpPointMasCercano = i;
                distanciaMinima = distanciaFor;
            }
        }

        IDeslizarPoint = IJumpPointMasCercano;

        ///////////////

        Vector2 direccionRay = new Vector2(transform.localScale.x, 0);

        RaycastHit2D hitInfo = Physics2D.Raycast(puntoRayCast.transform.position, direccionRay, 0.01f, 1 << 6);

        Debug.DrawRay(puntoRayCast.transform.position, direccionRay, Color.red);

        if (hitInfo.collider != null && hitInfo.collider.tag.Equals("Ground"))
        {
            if (!paredSuperior)
            {
                deslizandoSuelo = false;
                animator.SetBool("deslizandoSuelo", false);
            }
        }
    }

    void Update()
    {

        distancia = (Vector3.Distance(playerPoints.deslizarPoint[IDeslizarPoint], transform.position));

        if ((distancia < 0.2) || (distancia < 0.7 && playerPoints.deslizarPoint[IDeslizarPoint].x < transform.position.x))
        {
                print("Punto deslizar");
                playerPoints.deslizarPoint[IDeslizarPoint] = Vector3.zero;
                StartCoroutine("resetearDeslizar");
                animator.SetBool("deslizandoSuelo", true);
                deslizandoSuelo = true;

        }
    }

    IEnumerator resetearDeslizar()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);

            if (!paredSuperior)
            {
                print("Break corrutina");
                deslizandoSuelo = false;
                animator.SetBool("deslizandoSuelo", false);
                break;
            }

            yield return null;
        }

        yield return null;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            paredSuperior = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            paredSuperior = false;
        }
    }
}