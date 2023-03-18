using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcionesController : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
    }

    public void mostrarEsconderOpciones(Animator animatorPanel)
    {
        animatorPanel.SetBool("mostrado", !animatorPanel.GetBool("mostrado"));
    }

    public void botonPlay()
    {
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
}
