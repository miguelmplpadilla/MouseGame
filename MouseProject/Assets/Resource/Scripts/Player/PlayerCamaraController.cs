using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCamaraController : MonoBehaviour
{

    public float camSize;
    public float camSizeMin = 1.3f;
    public float camSizeMax = 2f;

    public float camOfsetX;
    public float camOfsetXMin = 0;
    public float camOfsetXMax = 1;

    private CinemachineVirtualCamera cm;

    private void Start()
    {
        camSize = camSizeMin;
        
        cm = GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (camSize < camSizeMax)
            {
                camSize += 2 * Time.deltaTime;
            }
        }
        else
        {
            if (camSize > camSizeMin)
            {
                camSize -= 2 * Time.deltaTime;
            }
        }

        cm.m_Lens.OrthographicSize = camSize;
    }
}
