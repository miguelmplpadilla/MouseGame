using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autor: Miguel Padilla Lillo
public class PlayerGanchoController : MonoBehaviour
{

    public bool ganchoLanzado = false;
    public bool enganchado = false;
    public bool puedeDispararGancho = false;

    public GameObject puntoLanzamientoGancho;
    public GameObject gancho;
    private GameObject puntoEngancheVirtual;
    private LineRenderer ganchoLineRenderer;

    private PlayerGroundController groundController;
    private PlayerMovement playerMovement;
    private DistanceJoint2D distanceJoint;
    private PlayerDeslizarController playerDeslizarController;
    
    private Animator animator;
    private Rigidbody2D rigidbody;

    public float speedGancho = 2;

    private RaycastHit2D hitInfo;

    private GameObject[] ganchosEscena;
    private GameObject ganchoCercano;
    private GameObject clickDerechoGancho;

    private void Awake()
    {
        groundController = GetComponentInChildren<PlayerGroundController>();
        playerMovement = GetComponent<PlayerMovement>();
        distanceJoint = GetComponent<DistanceJoint2D>();
        playerDeslizarController = GetComponentInChildren<PlayerDeslizarController>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ganchoLineRenderer = gancho.GetComponent<LineRenderer>();
        puntoEngancheVirtual = GameObject.Find("PuntoEngancheVirtual");
        distanceJoint.connectedBody = puntoEngancheVirtual.GetComponent<Rigidbody2D>();
        clickDerechoGancho = GameObject.Find("ClickDerechoGancho");
    }

    void Update()
    {
        animator.SetFloat("velocidadHorizontal", rigidbody.velocity.x);
        
        if (!puedeDispararGancho)
        {
            if (enganchado)
            {
                distanceJoint.autoConfigureDistance = true;
                distanceJoint.enabled = false;
                
                gancho.transform.parent = transform;
                gancho.transform.position = puntoLanzamientoGancho.transform.position;

                enganchado = false;
                ganchoLanzado = false;
            }
        }

        Vector2 direccionRay = new Vector2(1, 0.6f);
        
        if (!groundController.isGrounded && !playerMovement.aireSaltandoPared && puedeDispararGancho)
        {
            if (!ganchoLanzado && !enganchado)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    animator.SetTrigger("lanzarGancho");
                    StartCoroutine("tiempoGanchoLanzado");
                    hitInfo = Physics2D.Raycast(transform.position, direccionRay,10000, 1 << 6);
                    
                    puntoEngancheVirtual.transform.position = hitInfo.point;
                    
                    ganchoLanzado = true;
                }
            }
        }

        
        Debug.DrawRay(transform.position, direccionRay, Color.red);
        
        if (enganchado)
        {
            if (Input.GetButtonDown("Jump"))
            {
                gancho.transform.parent = transform;
                gancho.transform.position = puntoLanzamientoGancho.transform.position;
                
                distanceJoint.autoConfigureDistance = true;
                distanceJoint.enabled = false;
                
                playerMovement.saltar(playerMovement.jumpForce);
                
                enganchado = false;
            }
        }
        
        animator.SetBool("enganchado", enganchado);
    }

    private void FixedUpdate()
    {
        if (ganchoLanzado)
        {
            gancho.transform.parent = null;
            gancho.transform.position = Vector3.MoveTowards( gancho.transform.position, hitInfo.point, speedGancho * Time.deltaTime);

            float distanciaGanchoEnganche = Vector2.Distance(gancho.transform.position, hitInfo.point);
            
            if (distanciaGanchoEnganche < 0.05f)
            {
                if (hitInfo.collider.tag.Equals("Enganche"))
                {
                    distanceJoint.enabled = true;

                    distanceJoint.autoConfigureDistance = false;

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
        
        comprobarLanzarGancho();
        
        ganchoLineRenderer.SetPosition(0, puntoLanzamientoGancho.transform.position);
        ganchoLineRenderer.SetPosition(1, gancho.transform.position);
    }

    private void comprobarLanzarGancho()
    {
        Vector2 direccionRay = new Vector2(1, 0.6f);
        RaycastHit2D hitInfoComprobar = Physics2D.Raycast(transform.position, direccionRay,10000, 1 << 6);

        float distanciaGancho = Vector2.Distance(hitInfoComprobar.point, transform.position);

        RaycastHit2D hitInfoComprobarPared = Physics2D.Raycast(playerDeslizarController.puntoRayCast.transform.position, Vector2.left,0.01f, 1 << 6);

        Debug.DrawRay(transform.position, Vector2.left, Color.red);
        
        if (hitInfoComprobar.collider != null && hitInfoComprobar.collider.tag.Equals("Enganche"))
        {
            clickDerechoGancho.transform.localScale = new Vector3(1, 1, 1);
            clickDerechoGancho.transform.position = hitInfoComprobar.point;
            
            puedeDispararGancho = true;
        }
        else
        {
            clickDerechoGancho.transform.localScale = new Vector3(0,0,0);
            puedeDispararGancho = false;
        }
        
        if (groundController.isGrounded || 
            (hitInfoComprobarPared.collider != null && hitInfoComprobarPared.collider.tag.Equals("Ground")))
        {
            puedeDispararGancho = false;
        }

        if (!groundController.isGrounded && distanciaGancho >= 2f && !enganchado)
        {
            puedeDispararGancho = false;
        }
    }

    private IEnumerator tiempoGanchoLanzado()
    {
        yield return new WaitForSeconds(2f);

        if (!enganchado)
        {
            distanceJoint.autoConfigureDistance = true;
            gancho.transform.parent = transform;
            gancho.transform.position = puntoLanzamientoGancho.transform.position;
                
            enganchado = false;
            ganchoLanzado = false;
        }
    }
}
