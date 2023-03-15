using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuntuacionCreditosController : MonoBehaviour
{
    
    public GameObject panelPuntuacion;
    
    public List<int> puntos;
    
    private void Start()
    {
        puntos.Sort();
        puntos.Reverse();
        
        for (int i = 0; i < puntos.Count; i++)
        {
            GameObject panelPuntuacionInstanciado = Instantiate(panelPuntuacion, transform.position, Quaternion.identity);

            panelPuntuacionInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1) + " - " + puntos[i] + " m";

            panelPuntuacionInstanciado.transform.parent = transform;
            panelPuntuacionInstanciado.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
