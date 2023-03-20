using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// Autor: Miguel Padilla Lillo
public class PlayerDeslizarController : MonoBehaviour
{

    private PlayerMovement playerMovement;
    private Animator animator;

    public bool deslizandoSuelo = false;
    public bool paredSuperior = false;

    public GameObject puntoRayCast;

    public bool deslizarBloqueado = true;

    public PlayerPoints playerPoints;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        animator = GetComponentInParent<Animator>();

        playerPoints = GetComponentInParent<PlayerPoints>();
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            deslizar();
        }
    }

    private void FixedUpdate()
    {
        Vector2 direccionRay = new Vector2(transform.localScale.x, 0);

        RaycastHit2D hitInfo = Physics2D.Raycast(puntoRayCast.transform.position, direccionRay,0.01f, 1 << 6);

        Debug.DrawRay(puntoRayCast.transform.position, direccionRay, Color.red);
        
        if (hitInfo.collider != null && hitInfo.collider.tag.Equals("Ground"))
        {
            if (!paredSuperior)
            {
                deslizandoSuelo = false;
                animator.SetBool("deslizandoSuelo", false);
                playerPoints.MakeBreakDeslizarPoint();
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
                if (deslizandoSuelo) playerPoints.MakeBreakDeslizarPoint();
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

    public void desbloquearDeslizar()
    {
        deslizarBloqueado = false;
    }
    
    public void desbloquearVariables()
    {
        deslizarBloqueado = false;
    }

    public void deslizar()
    {
        if (!deslizarBloqueado)
        {
            if (!deslizandoSuelo)
            {
                playerPoints.MakeDeslizarPoint();
                StartCoroutine("resetearDeslizar");
                animator.SetBool("deslizandoSuelo", true);
                deslizandoSuelo = true;
            }
        }
    }

    public void deslizarMovil()
    {
        deslizar();
    }
}
