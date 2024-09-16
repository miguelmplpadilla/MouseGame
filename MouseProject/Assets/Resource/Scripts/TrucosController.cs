using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrucosController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            eliminarPlayerPrefsTutorial();
        }
    }

    public void eliminarPlayerPrefsTutorial()
    {
        PlayerPrefs.DeleteKey("TutorialTerminado");
    }

    public void eliminarPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}