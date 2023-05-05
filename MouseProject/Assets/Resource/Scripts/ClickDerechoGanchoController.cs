using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDerechoGanchoController : MonoBehaviour
{

    [SerializeField] private Sprite spriteTeclado;
    [SerializeField] private Sprite spriteMando;
    
    void Start()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            GetComponent<SpriteRenderer>().sprite = spriteMando;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = spriteTeclado;
        }
    }
}
