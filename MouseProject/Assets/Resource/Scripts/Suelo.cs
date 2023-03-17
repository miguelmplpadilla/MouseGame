using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suelo : MonoBehaviour
{
    public GameObject hijo;
    public GameObject[] escenarios;
    public GameObject escenarioTutorial;
    public GameObject escenaPostTutorial;

    public int random;
    public bool generarTerreno;
    public bool tutorial;
    public bool postTutorial;

    void Start()
    {
        if (generarTerreno == true)
        {
            if (!tutorial && !postTutorial)
            {
                Invoke("DestroyObj", 50);

                random = Random.Range(0, escenarios.Length);

                escenarios[random].SetActive(true);
            }
            else if (tutorial)
            {
                escenarioTutorial.SetActive(true);
                Invoke("DestroyObj", 200);
            }
            else if (postTutorial)
            {
                escenaPostTutorial.SetActive(true);
                Invoke("DestroyObj", 50);
            }
        }

    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

}
