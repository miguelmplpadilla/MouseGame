using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autor: Miguel Padilla Lillo
public class TutorialController : MonoBehaviour
{

    private bool empezarTutorial = false;
    private bool bloquearTutorial = false;

    public string teclaTutorial;

    private GameObject player;
    private GameObject gameObjectTeclaTutorial;
    private GameObject canvas;

    public float velocidadRelentizarTiempo = 2;

    private bool saltar = false;
    
    [SerializeField] private Vector2 startTouchPosition;
    [SerializeField] private Vector2 endTouchPosition;

    private void Awake()
    {
        gameObjectTeclaTutorial = transform.GetChild(0).gameObject;
        canvas = transform.GetChild(1).gameObject;
        gameObjectTeclaTutorial.GetComponent<SpriteRenderer>().enabled = false;
        canvas.GetComponent<Canvas>().enabled = false;
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            tutorialPC();
        }
        else
        {
            tutorialMovil();
        }
    }

    public void tutorialPC()
    {
        if (!bloquearTutorial)
        {
            if (empezarTutorial)
            {
                Time.timeScale -= velocidadRelentizarTiempo * Time.deltaTime;

                if (teclaTutorial.Equals("Deslizar"))
                {
                    if (Input.mouseScrollDelta.y < 0)
                    {
                        bloquearTutorial = true;
                        Time.timeScale = 1;
                    }
                }
                else
                {
                    if (Input.GetButtonDown(teclaTutorial))
                    {
                        bloquearTutorial = true;
                        Time.timeScale = 1;
                    }
                }
            }
        }
    }

    public void tutorialMovil()
    {
        if (!bloquearTutorial)
        {
            if (empezarTutorial)
            {
                Time.timeScale -= velocidadRelentizarTiempo * Time.deltaTime;
                
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    startTouchPosition = Input.GetTouch(0).position;
                    Debug.Log("Posicion inicial: " + startTouchPosition);

                    if (!teclaTutorial.Equals("Deslizar"))
                    {
                        bloquearTutorial = true;
                        Time.timeScale = 1;
                    }
                }

                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if ((startTouchPosition.y - endTouchPosition.y) > 50)
                    {
                        bloquearTutorial = true;
                        Time.timeScale = 1;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.BroadcastMessage("desbloquear"+teclaTutorial);
            
            gameObjectTeclaTutorial.GetComponent<SpriteRenderer>().enabled = true;
            canvas.GetComponent<Canvas>().enabled = true;
            empezarTutorial = true;
        }
    }
}