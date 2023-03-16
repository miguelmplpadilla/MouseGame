using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autor: Miguel Padilla Lillo
public class PlayerPoints : MonoBehaviour
{
    // Start is called before the first frame update
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

    public Vector3 walkPoint;

    public bool visualMarks;


    void Awake()
    {
        paredJumpPoint = new Vector3[10];
        jumpPoint = new Vector3[10];
        runPoint = new Vector3[10];
        deslizarPoint = new Vector3[10];

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
        walkPoint = transform.position;
        CrearMarca();
    }

    public void CrearMarca()
    {
        if (visualMarks)
        {
            Instantiate(marca, transform.position, Quaternion.identity);
        }
    }
}
