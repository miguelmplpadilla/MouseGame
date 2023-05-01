using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

// Autor: Miguel Padilla Lillo
public class TutorialController : MonoBehaviour
{
    
    [Serializable]
    public struct Teclas {
        public string name;
        public Sprite image;
    }
    
    public Teclas[] teclasTeclado;
    public Teclas[] teclasMando;

    [SerializeField] private string textoTutorial;
    Dictionary<string, Sprite> spriteTeclasTeclado = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> spriteTeclasMando = new Dictionary<string, Sprite>();

    private bool empezarTutorial = false;
    private bool bloquearTutorial = false;

    public string teclaTutorial;

    private GameObject player;
    private GameObject canvas;

    private SpriteRenderer teclaTutorialSpriteRenderer;
    private TextMeshProUGUI textoCanvasTutorial;

    public float velocidadRelentizarTiempo = 2;

    private bool saltar = false;
    
    [SerializeField] private Vector2 startTouchPosition;
    [SerializeField] private Vector2 endTouchPosition;

    [MenuItem("Scripts/DeletePlayerPrefs")]
    public static void deletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Awake()
    {
        for (int i = 0; i < teclasTeclado.Length; i++)
        {
            spriteTeclasTeclado.Add(teclasTeclado[i].name, teclasTeclado[i].image);
            spriteTeclasMando.Add(teclasMando[i].name, teclasMando[i].image);
        }
        
        canvas = transform.GetChild(1).gameObject;
        teclaTutorialSpriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        textoCanvasTutorial = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        teclaTutorialSpriteRenderer.enabled = false;
        canvas.GetComponent<Canvas>().enabled = false;
    }

    private void Start()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            teclaTutorialSpriteRenderer.sprite = spriteTeclasMando[teclaTutorial];
        }
        else
        {
            teclaTutorialSpriteRenderer.sprite = spriteTeclasTeclado[teclaTutorial];
        }
        
        textoCanvasTutorial.text = textoTutorial;
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
                    if (Input.mouseScrollDelta.y < 0 || Input.GetButton("Fire2"))
                    {
                        bloquearTutorial = true;
                        Time.timeScale = 1;
                    }
                }
                else
                {
                    if (Input.GetJoystickNames().Length > 0 && teclaTutorial.Equals("Fire1"))
                    {
                        if (Input.GetAxisRaw("Fire1") > 0)
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

                    if (!teclaTutorial.Equals("Deslizar") && !teclaTutorial.Equals("Jump"))
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
                    } else if ((startTouchPosition.y - endTouchPosition.y) < -50)
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
            
            teclaTutorialSpriteRenderer.enabled = true;
            canvas.GetComponent<Canvas>().enabled = true;
            empezarTutorial = true;
        }
    }
}