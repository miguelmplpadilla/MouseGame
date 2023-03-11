using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float altura;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + altura, transform.position.z);

        transform.position = Vector3.MoveTowards(target, transform.position, 0.1f);   

    }
}
