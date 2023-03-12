using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suelo : MonoBehaviour
{
    public GameObject hijo;

    void Awake()
    {
        Invoke("DestroyObj",50);
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

}
