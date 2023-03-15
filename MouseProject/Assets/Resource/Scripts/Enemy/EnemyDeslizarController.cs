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

    public bool deslizandoSuelo = false;
    public bool paredSuperior = false;

    public int IDeslizarPoint;

    /////

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


    }

    void Update()
    {

        float distancia = (Vector3.Distance(playerPoints.deslizarPoint[IDeslizarPoint], transform.position));

        if ((distancia < 0.03) || (distancia < 0.7 && playerPoints.deslizarPoint[IDeslizarPoint].x < transform.position.x))
        {
            if (!deslizandoSuelo)
            {
                playerPoints.deslizarPoint[IDeslizarPoint] = Vector3.zero;
                //movementScript.speed = 1;
                StartCoroutine("resetearDeslizar");
                animator.SetBool("deslizandoSuelo", true);
                deslizandoSuelo = true;
            }
        }
    }

    IEnumerator resetearDeslizar()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.42f);

            if (!paredSuperior)
            {
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
