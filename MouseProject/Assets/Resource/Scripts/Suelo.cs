using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suelo : MonoBehaviour
{
    public GameObject hijo;
    public GameObject[] escenarios;

    public int random;
    public bool generarTerreno;

    void Awake()
    {
        if (generarTerreno == true)
        {
            Invoke("DestroyObj", 50);

            random = Random.Range(0, escenarios.Length);

            escenarios[random].SetActive(true);
        }

    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

}
