using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcionesController : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void mostrarEsconderOpciones(Animator animatorPanel)
    {
        animatorPanel.SetBool("mostrado", !animatorPanel.GetBool("mostrado"));
    }

    public void botonPlay()
    {
        esconderCursor();
        LoadSceneController.cargarEscena("EscenaMiguel");
    }

    public void botonSalir()
    {
        Application.Quit();
    }

    public void volverMenuInicio()
    {
        LoadSceneController.cargarEscena("MenuInicio");
    }

    public void esconderCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
