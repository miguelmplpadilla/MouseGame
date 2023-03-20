using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.PlayerLoop;

public class PlayerMobileControls : MonoBehaviour
{
    [SerializeField] private Vector2 startTouchPosition;
    [SerializeField] private Vector2 endTouchPosition;

    public float tiempoPulsado = 0;

    public bool pulsado = false;

    private GameObject player;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
            Debug.Log("Posicion inicial: " + startTouchPosition);

            pulsado = true;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            Debug.Log("Posicion final: " + endTouchPosition);

            if ((startTouchPosition.y - endTouchPosition.y) > 50)
            {
                player.BroadcastMessage("deslizarMovil");
                
                Debug.Log("Deslizdo hacia abajo");
            }
            else if (tiempoPulsado > 0.005f && tiempoPulsado < 0.15f)
            {
                player.BroadcastMessage("saltarMovil");
                
                Debug.Log("Pulsado");
            }
            else if (tiempoPulsado > 0.15f)
            {
                Debug.Log("Tiempo pulsado: "+tiempoPulsado);
                Debug.Log("Mantenido");
            }
            
            player.BroadcastMessage("dejarMoverCamaraMovil");

            tiempoPulsado = 0;

            pulsado = false;
        }

        if (tiempoPulsado > 0.1f)
        {
            player.BroadcastMessage("moverCamaraMovil");
            player.BroadcastMessage("correrMovil");
            Debug.Log("Manteniendo");
        }

        if (pulsado)
        {
            tiempoPulsado += 1 * Time.deltaTime;
        }
    }
}