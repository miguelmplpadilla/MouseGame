using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumenController : MonoBehaviour
{

    private Slider sliderVolumen;
    private AudioSource audioSource;
    
    void Start()
    {
        sliderVolumen = GameObject.Find("SliderVolumen").GetComponent<Slider>();
        audioSource = GameObject.Find("AudioSourceMusica").GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("VolumenMusica"))
        {
            sliderVolumen.value = PlayerPrefs.GetFloat("VolumenMusica");
        }
    }

    private void LateUpdate()
    {
        audioSource.volume = sliderVolumen.value * sliderVolumen.value;
    }

    public void guardarVolumen()
    {
        PlayerPrefs.SetFloat("VolumenMusica", audioSource.volume);
    }
}
