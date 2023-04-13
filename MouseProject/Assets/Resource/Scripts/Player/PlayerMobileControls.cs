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
        //controlerV1();
        controlerV2();
    }

    private void controlerV1()
    {
        if (Input.touchCount > 0 && Input.touchCount < 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;

                pulsado = true;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouchPosition = Input.GetTouch(0).position;

                if (tiempoPulsado < 0.2f)
                {
                    if ((startTouchPosition.y - endTouchPosition.y) > 50)
                    {
                        player.BroadcastMessage("deslizarMovil");
                    }
                    else if ((startTouchPosition.y - endTouchPosition.y) < -50)
                    {
                        player.BroadcastMessage("saltarMovil");
                    }
                }

                startTouchPosition = new Vector2(0, 0);
                endTouchPosition = new Vector2(0, 0);

                player.BroadcastMessage("dejarMoverCamaraMovil");
                player.BroadcastMessage("dejarCorrerMovil");

                tiempoPulsado = 0;

                pulsado = false;
            }
        } else if (Input.touchCount > 1)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(1).position;
            }

            if (Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                endTouchPosition = Input.GetTouch(1).position;
                
                if ((startTouchPosition.y - endTouchPosition.y) > 50)
                {
                    player.BroadcastMessage("deslizarMovil");
                }
                else if ((startTouchPosition.y - endTouchPosition.y) < -50)
                {
                    player.BroadcastMessage("saltarMovil");
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

    private void controlerV2()
    {
        if (Input.touchCount > 0)
        {
            controlV2DetectorTouch(0);
        }
        else
        {
            player.BroadcastMessage("dejarMoverCamaraMovil");
            player.BroadcastMessage("dejarCorrerMovil");
        }
        
        if (Input.touchCount > 1)
        {
            controlV2DetectorTouch(1);
        }
    }
    
    private void controlV2DetectorTouch(int numTouch)
    {
        if (Input.GetTouch(numTouch).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(numTouch).position;
            
            Debug.Log("StartTouchPosition Touch "+numTouch+": "+startTouchPosition);

            if (Input.GetTouch(numTouch).position.x < 1003)
            {
                player.BroadcastMessage("moverCamaraMovil");
                player.BroadcastMessage("correrMovil");
            }
        }
        
        if (Input.GetTouch(numTouch).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(numTouch).position;
            
            Debug.Log("EndTouchPosition Touch "+numTouch+": "+endTouchPosition);

            if (Input.GetTouch(numTouch).position.x < 1003)
            {
                player.BroadcastMessage("dejarMoverCamaraMovil");
                player.BroadcastMessage("dejarCorrerMovil");
            } else if (Input.GetTouch(numTouch).position.x > 1003)
            {
                if ((startTouchPosition.y - endTouchPosition.y) > 50)
                {
                    player.BroadcastMessage("deslizarMovil");
                }
                else
                {
                    player.BroadcastMessage("saltarMovil");
                }
            }
        }
    }
}