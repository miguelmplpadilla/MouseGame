using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetAxisRaw("Fire1") > 1)
        {
            SaltarCinematica();
        }
    }

    public void SaltarCinematica()
    {
        LoadSceneController.cargarEscena("Creditos");
    }
}
