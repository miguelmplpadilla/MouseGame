using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeslizarController : MonoBehaviour
{
    private EnemyDeslizarController playerMovement;
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

        player = GameObject.Find("Player");
        playerPoints = player.GetComponent<PlayerPoints>();
    }

    private void FixedUpdate()
    {
        float distanciaMinima = 99999;

        int IJumpPointMasCercano = 0;

        for (int i = 0; i < 10; i++)
        {
            if (playerPoints.deslizarPoint[i].x > transform.position.x)
            {
                float distanciaFor = Vector3.Distance(playerPoints.deslizarPoint[i], transform.position);
                if (distanciaFor < distanciaMinima)
                {
                    IJumpPointMasCercano = i;
                    distanciaMinima = distanciaFor;
                }
            }

        }

        IDeslizarPoint = IJumpPointMasCercano;

    }

    void Update()
    {
        if (Vector3.Distance(playerPoints.deslizarPoint[IDeslizarPoint], transform.position) < 0.02)
        {
            if (!deslizandoSuelo)
            {
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
            yield return new WaitForSeconds(1);

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
