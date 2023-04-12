using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.PlayerLoop;

public class PlayerMobileControls : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private float tiempoPulsado = 0;

    private bool pulsado = false;

    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.touchCount < 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;
                Debug.Log("Posicion inicial Touch 1: " + startTouchPosition);

                pulsado = true;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouchPosition = Input.GetTouch(0).position;

                Debug.Log("Posicion final Touch 1: " + endTouchPosition);

                if (tiempoPulsado < 0.2f)
                {
                    if ((startTouchPosition.y - endTouchPosition.y) > 50)
                    {
                        player.BroadcastMessage("deslizarMovil");

                        Debug.Log("Deslizdo hacia abajo Touch 1");
                    }
                    else if ((startTouchPosition.y - endTouchPosition.y) < -50)
                    {
                        player.BroadcastMessage("saltarMovil");
                
                        Debug.Log("Deslizdo hacia arriba Touch 1");
                    }
                }

                startTouchPosition = new Vector2(0, 0);
                endTouchPosition = new Vector2(0, 0);

                player.BroadcastMessage("dejarMoverCamaraMovil");
                player.BroadcastMessage("dejarCorrerMovil");

                Debug.Log("Tiempo pulsado: "+tiempoPulsado);

                tiempoPulsado = 0;

                pulsado = false;
            }
        } else if (Input.touchCount > 1)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(1).position;
                Debug.Log("Posicion inicial Touch 2: " + startTouchPosition);
            }

            if (Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                endTouchPosition = Input.GetTouch(1).position;

                Debug.Log("Posicion final Touch 2: " + endTouchPosition);
                
                if ((startTouchPosition.y - endTouchPosition.y) > 50)
                {
                    player.BroadcastMessage("deslizarMovil");

                    Debug.Log("Deslizdo hacia abajo Touch 2");
                }
                else if ((startTouchPosition.y - endTouchPosition.y) < -50)
                {
                    player.BroadcastMessage("saltarMovil");
                    
                    Debug.Log("Deslizdo hacia arriba Touch 2");
                }

                startTouchPosition = new Vector2(0, 0);
                endTouchPosition = new Vector2(0, 0);
            }
        }

        if (pulsado)
        {
            if (tiempoPulsado > 0.1f)
            {
                player.BroadcastMessage("moverCamaraMovil");
                player.BroadcastMessage("correrMovil");
            }

            tiempoPulsado += 1 * Time.deltaTime;
        }
    }
}