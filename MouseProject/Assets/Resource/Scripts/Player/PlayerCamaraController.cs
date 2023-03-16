using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Autor: Miguel Padilla Lillo
public class PlayerCamaraController : MonoBehaviour
{

    public float camSize;
    public float camSizeMin = 1.3f;
    public float camSizeMax = 2f;

    public float camOfsetX;
    public float camOfsetXMin = 0;
    public float camOfsetXMax = 1;

    private CinemachineVirtualCamera cm;
    private CinemachineFramingTransposer cmft;

    private PlayerMovement playerMovement;
    private PlayerBordeController playerBordeController;
    private PlayerDeslizarController playerDeslizarController;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerBordeController = GetComponentInChildren<PlayerBordeController>();
        playerDeslizarController = GetComponentInChildren<PlayerDeslizarController>();
    }

    private void Start()
    {
        camSize = camSizeMin;
        
        cm = GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>();
        cmft = cm.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        if (!playerMovement.bloqueoSprint)
        {
            if (playerMovement.estamina > 0 && !playerBordeController.enganchadoBorde && !playerDeslizarController.deslizandoSuelo)
            {
                if (Input.GetButton("Fire1"))
                {
                    if (camSize < camSizeMax)
                    {
                        camSize += 2 * Time.deltaTime;
                    }
            
                    if (camOfsetX < camOfsetXMax)
                    {
                        camOfsetX += 2 * Time.deltaTime;
                    }
                }
                else
                {
                    if (camSize > camSizeMin)
                    {
                        camSize -= 2 * Time.deltaTime;
                    }
            
                    if (camOfsetX > camOfsetXMin)
                    {
                        camOfsetX -= 2 * Time.deltaTime;
                    }
                }
            }
            else
            {
                if (camSize > camSizeMin)
                {
                    camSize -= 2 * Time.deltaTime;
                }
            
                if (camOfsetX > camOfsetXMin)
                {
                    camOfsetX -= 2 * Time.deltaTime;
                }
            }
        }

        cm.m_Lens.OrthographicSize = camSize;
        cmft.m_TrackedObjectOffset = new Vector3(camOfsetX, cmft.m_TrackedObjectOffset.y, cmft.m_TrackedObjectOffset.z);
    }
}
