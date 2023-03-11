using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaImputs : MonoBehaviour
{
    // Start is called before the first frame update
    public PruebaMovimiento scriptPlayer;
    public PruebaMovimiento scriptEnemigo;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            scriptPlayer.Saltar();
        }

    }
}
