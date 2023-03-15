using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeslizarController : MonoBehaviour
{

    private PlayerMovement playerMovement;
    private Animator animator;

    public bool deslizandoSuelo = false;
    public bool paredSuperior = false;

    /// <summary>

    public PlayerPoints playerPoints;

    /// </summary>

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        animator = GetComponentInParent<Animator>();

        //

        playerPoints = GetComponentInParent<PlayerPoints>();
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            if (!deslizandoSuelo)
            {
                StartCoroutine("resetearDeslizar");
                animator.SetBool("deslizandoSuelo", true);
                deslizandoSuelo = true;

                playerPoints.MakeDeslizarPoint();
            }
        }

    }

    IEnumerator resetearDeslizar()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);

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
