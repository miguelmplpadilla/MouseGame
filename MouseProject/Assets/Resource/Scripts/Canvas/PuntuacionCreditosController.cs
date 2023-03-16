using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Autor: Miguel Padilla Lillo
public class PuntuacionCreditosController : MonoBehaviour
{
    
    public GameObject panelPuntuacion;

    private TextMeshProUGUI textoPuntuacionActual;
    
    public List<float> puntos;
    
    private void Start()
    {

        textoPuntuacionActual = GameObject.Find("TextoPuntuacionActual").GetComponent<TextMeshProUGUI>();
        
        if (PlayerPrefs.HasKey("Record1"))
        {
            int numPartida = 1;
            while (true)
            {
                if (PlayerPrefs.HasKey("Record"+numPartida))
                {
                    puntos.Add(PlayerPrefs.GetFloat("Record"+numPartida));
                    numPartida++;
                }
                else
                {
                    break;
                }
            }

            textoPuntuacionActual.text = "Current score: "+ (int)PlayerPrefs.GetFloat("Record" + PlayerPrefs.GetInt("NumPartida")) + " m";
        }
        
        puntos.Sort();
        puntos.Reverse();
        
        for (int i = 0; i < puntos.Count; i++)
        {
            GameObject panelPuntuacionInstanciado = Instantiate(panelPuntuacion, transform.position, Quaternion.identity);

            panelPuntuacionInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1) + " - " + (int)puntos[i] + " m";

            panelPuntuacionInstanciado.transform.parent = transform;
            panelPuntuacionInstanciado.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
