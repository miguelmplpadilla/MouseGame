using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Autor: Miguel Padilla Lillo
public class Puntuacion : MonoBehaviour
{
    private Vector3 puntoInicio;

    private GameObject player;
    private TextMeshProUGUI puntuacionTxt;

    private TextMeshProUGUI recordTxt;

    public float puntuacion;

    public float record;

    private int numPartida;

    
    void Awake()
    {
        player = GameObject.Find("Player");

        puntuacionTxt = GameObject.Find("TextoPuntuacion").GetComponent<TextMeshProUGUI>();

        recordTxt = GameObject.Find("TextoRecord").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        puntoInicio = player.transform.position;
        
        recordTxt.text = ((int)record).ToString();

        List<float> puntos = new List<float>();

        if (PlayerPrefs.HasKey("NumPartida"))
        {
            int contNumPartida = 1;
            
            while (true)
            {
                if (PlayerPrefs.HasKey("Record"+contNumPartida))
                {
                    puntos.Add(PlayerPrefs.GetFloat("Record"+contNumPartida));
                    contNumPartida++;
                }
                else
                {
                    break;
                }
            }
            
            puntos.Sort();
            puntos.Reverse();
            
            numPartida = PlayerPrefs.GetInt("NumPartida");
            
            record = puntos[0];
        }
        else
        {
            numPartida = 0;
        }

        recordTxt.text = (int)record + "m";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            guardarPuntuacion();
            LoadSceneController.cargarEscena("Creditos");
        }
    }

    void FixedUpdate()
    {
        puntuacion = Vector3.Distance(player.transform.position, puntoInicio) * 10;

        if (puntuacion > record)
        {
            //PlayerPrefs.SetFloat("RECORD", puntuacion);
            recordTxt.text = (int)puntuacion + "m";
        }

        puntuacionTxt.text = (int)puntuacion + "m";
    }

    public void guardarPuntuacion()
    {
        numPartida++;
        
        PlayerPrefs.SetInt("NumPartida", numPartida);
        
        PlayerPrefs.SetFloat("Record"+numPartida, puntuacion);
    }
}
