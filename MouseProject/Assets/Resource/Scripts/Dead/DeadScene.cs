using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SaltarCinematica();
        }
    }

    public void SaltarCinematica()
    {
        LoadSceneController.cargarEscena("Creditos");
    }
}
