using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float speedGancho = 2;

    private RaycastHit2D hitInfo;

    private GameObject[] ganchosEscena;
    private GameObject ganchoCercano;

    private void Awake()
    {
        groundController = GetComponentInChildren<PlayerGroundController>();
        playerMovement = GetComponent<PlayerMovement>();
        distanceJoint = GetComponent<DistanceJoint2D>();
    }

    private void Start()
    {
        ganchoLineRenderer = gancho.GetComponent<LineRenderer>();
        
        puntoEngancheVirtual = GameObject.Find("PuntoEngancheVirtual");
        
        distanceJoint.connectedBody = puntoEngancheVirtual.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 direccionRay = new Vector2(1, 0.6f);
        
        if (!groundController.isGrounded && !playerMovement.aireSaltandoPared && puedeDispararGancho)
        {
            if (!ganchoLanzado && !enganchado)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    StartCoroutine("tiempoGanchoLanzado");
                    hitInfo = Physics2D.Raycast(transform.position, direccionRay,10000, 1 << 6);
                    
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
                
                distanceJoint.enabled = false;
                
                playerMovement.saltar(playerMovement.jumpForce);
                
                enganchado = false;
            }
        }
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
                    puntoEngancheVirtual.transform.position = hitInfo.point;
                    
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
        hitInfo = Physics2D.Raycast(transform.position, direccionRay,10000, 1 << 6);

        if (hitInfo.collider != null && hitInfo.collider.tag.Equals("Enganche"))
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

    private IEnumerator tiempoGanchoLanzado()
    {
        yield return new WaitForSeconds(1f);

        if (!enganchado)
        {
            gancho.transform.parent = transform;
            gancho.transform.position = puntoLanzamientoGancho.transform.position;
                
            enganchado = false;
            ganchoLanzado = false;
        }
    }
}
