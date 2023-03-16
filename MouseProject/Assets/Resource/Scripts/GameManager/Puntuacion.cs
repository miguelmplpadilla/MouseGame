using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puntuacion : MonoBehaviour
{

    public GameObject puntoInicio;

    public GameObject player;

    public GameObject puntuacionTxtGO;
    public TextMeshProUGUI puntuacionTxt;

    public GameObject recordTxtGO;
    public TextMeshProUGUI recordTxt;

    public float puntuacion;

    public float record;

    
    void Awake()
    {
        player = GameObject.Find("Player");

        //puntuacionTxtGO = GameObject.Find("PuntuacionTXT");
        //puntuacionTxt = puntuacionTxtGO.GetComponent<TextMeshProUGUI>();

        recordTxtGO = GameObject.Find("RecordTXT");
        recordTxt = recordTxtGO.GetComponent<TextMeshProUGUI>();

        record = PlayerPrefs.GetFloat("RECORD", 0);

    }

    private void Start()
    {
        recordTxt.text = ((int)record).ToString();
    }

    void FixedUpdate()
    {
        puntuacion = Vector3.Distance(player.transform.position, puntoInicio.transform.position) * 10;

        if (puntuacion > record)
        {
            PlayerPrefs.SetFloat("RECORD", puntuacion);
            recordTxt.text = ((int)puntuacion).ToString();
        }

        puntuacionTxt.text = ((int)puntuacion).ToString();
    }
}
