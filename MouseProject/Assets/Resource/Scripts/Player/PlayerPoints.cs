using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public bool visualMarks;

    public GameObject marca;

     public PlayerGroundController groundController;
     public PlayerMovement playerScript;

     public Vector3[] paredJumpPoint;
     public int IparedJumpPoint;

    public Vector3[] deslizarPoint;
    public int IdeslizarPoint;

    public Vector3[] jumpPoint;
    public int IJumpPoint;

    public Vector3[] runPoint;
    public int IrunPoint;

    public Vector3[] ganchoPoint;
    public int IganchoPoint;

    public Vector3[] breackGanchoPoint;
    public int IbreackGanchoPoint;

    public Vector3[] walkPoint;
    public int IwalkPoint;


    void Awake()
    {
        paredJumpPoint = new Vector3[10];
        jumpPoint = new Vector3[10];
        runPoint = new Vector3[10];
        deslizarPoint = new Vector3[10];
        walkPoint = new Vector3[10];

        ganchoPoint = new Vector3[10];
        breackGanchoPoint = new Vector3[10];

        groundController = GetComponentInChildren<PlayerGroundController>();
        playerScript = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (playerScript.estamina > 0)
            {
                MakeRunPoint();
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            MakeWalkPoint();
        }
    }

    public void MakeDeslizarPoint()
    {
        deslizarPoint[IdeslizarPoint] = transform.position;
        IdeslizarPoint++;
        if (IdeslizarPoint >= 10) { IdeslizarPoint = 0; }
        CrearMarca();
    }

    public void MakeJumpPoint()
    {
        jumpPoint[IJumpPoint] = transform.position;
        IJumpPoint++;
        if (IJumpPoint >= 10) { IJumpPoint = 0; }
        CrearMarca();
    }

    public void MakeJumpWallPoint()
    {
        paredJumpPoint[IparedJumpPoint] = transform.position;
        IparedJumpPoint++;
        if (IparedJumpPoint >= 10) { IparedJumpPoint = 0; }
        CrearMarca();
    }

    public void MakeRunPoint()
    {
        runPoint[IrunPoint] = transform.position;
        IrunPoint++;
        if (IrunPoint >= 10) { IrunPoint = 0; }
        CrearMarca();
    }

    public void MakeWalkPoint()
    {
        walkPoint[IwalkPoint] = transform.position;
        IwalkPoint++;
        if (IwalkPoint >= 10) { IwalkPoint = 0; }
        CrearMarca();
    }

    public void MakeGanchoPoint()
    {
        ganchoPoint[IganchoPoint] = transform.position;
        IganchoPoint++;
        if (IganchoPoint >= 10) { IganchoPoint = 0; }
        CrearMarca();
    }

    public void MakeBreackGanchoPoint()
    {
        breackGanchoPoint[IbreackGanchoPoint] = transform.position;
        IbreackGanchoPoint++;
        if (IbreackGanchoPoint >= 10) { IbreackGanchoPoint = 0; }
        CrearMarca();
    }

    public void CrearMarca()
    {
        if (visualMarks)
        {
            if (marca != null) Instantiate(marca, transform.position, Quaternion.identity);
        }
    }
}
