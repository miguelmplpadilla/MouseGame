using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerarTerreno : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    public GameObject segundoSuelo;

    public GameObject sueloPrefab;

    public float distance;

    void Start()
    {
        player = GameObject.Find("Player");

        segundoSuelo = GameObject.Find("SueloReal");
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(player.transform.position, segundoSuelo.transform.position);

        if (distance < 4)
        {

            GameObject instancia = Instantiate(sueloPrefab, segundoSuelo.transform.position, Quaternion.identity);
            Suelo instanciaScript = instancia.GetComponent<Suelo>();

            ActualizarTarget(instanciaScript.hijo);

        }



    }

    public void ActualizarTarget(GameObject newTarget)
    {
        segundoSuelo = newTarget;
    }
}
