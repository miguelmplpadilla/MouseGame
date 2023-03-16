using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcionesController : MonoBehaviour
{

    public void mostrarEsconderOpciones(Animator animatorPanel)
    {
        animatorPanel.SetBool("mostrado", !animatorPanel.GetBool("mostrado"));
    }
}
