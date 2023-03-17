using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcionesController : MonoBehaviour
{
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
}
