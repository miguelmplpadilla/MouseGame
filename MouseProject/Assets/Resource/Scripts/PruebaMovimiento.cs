using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaMovimiento : MonoBehaviour
{

    public Rigidbody2D rb;

    public float velocity;

    public bool enemy;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Movimiento();

        if (Input.GetKey(KeyCode.Space))
        {
            if (enemy == true)
            {

                Invoke("Saltar", 1f);
            }
            else
            {
                Saltar();
            }
        }



    }


    public void Movimiento()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity * Time.deltaTime, rb.velocity.y);
    }

    public void Saltar()
    {

        rb.AddForce(transform.up * 20);

    }
}


