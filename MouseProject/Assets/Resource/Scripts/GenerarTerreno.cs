using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerarTerreno : MonoBehaviour
{
    private GameObject player;
    private GameObject segundoSuelo;
    public GameObject sueloPrefab;

    public bool tutorial;

    public float distance;

    void Start()
    {
        player = GameObject.Find("Player");
        segundoSuelo = GameObject.Find("SueloReal");
    }

    void Update()
    {

        distance = Vector3.Distance(player.transform.position, segundoSuelo.transform.position);

        if (distance < 4)
        {

            if (!tutorial)
            {
                GenerarSuelo(2);
            }
            else
            {
                GenerarSuelo(0);
            }
           
        }
    }

    public void GenerarSuelo(float altura)
    {
        GameObject instancia = Instantiate(sueloPrefab, segundoSuelo.transform.position + new Vector3(-0.1f, altura, 0), Quaternion.identity);
        Suelo instanciaScript = instancia.GetComponent<Suelo>();
        ActualizarTarget(instanciaScript.hijo);

        if (tutorial)
        {
            instanciaScript.postTutorial = true;
            tutorial = false;
        }

    }

    public void ActualizarTarget(GameObject newTarget)
    {
        segundoSuelo = newTarget;
    }
}
