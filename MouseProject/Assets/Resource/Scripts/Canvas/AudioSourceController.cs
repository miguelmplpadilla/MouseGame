using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("VolumenMusica"))
        {
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("VolumenMusica");
        }
    }
}
