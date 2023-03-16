using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Autor: Miguel Padilla Lillo
public class LoadSceneController : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine("cargar");
    }

    public static void cargarEscena(string escena)
    {
        PlayerPrefs.SetString("NivelCargar", escena);
        SceneManager.LoadScene("PantallaCarga");
    }

    IEnumerator cargar()
    {
        yield return new WaitForSeconds(1f);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("NivelCargar"));

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
