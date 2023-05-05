using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public float rango;
    public LayerMask capaDelJugador;
    public bool volando;

    public GameObject parent;
    public bool jugadorCerca;



    void Start()
    {
        rango = Random.Range(0.6f, 1.4f);

        if (Random.Range(1,3) == 2)
        {
            Invoke(nameof(EmpezarComer), Random.Range(0.1f, 0.5f));
        }
    }

    void Update()
    {
        jugadorCerca = Physics2D.OverlapCircle(transform.position, rango, capaDelJugador);

        if (jugadorCerca)
        {
            anim.SetTrigger("Volar");
            volando = true;
            Invoke(nameof(Destruir), 4);
        }

        if (volando)
        {
            transform.Translate(-1 * Time.deltaTime, 1 * Time.deltaTime, 0);
        }

    }

    public void EmpezarComer()
    {
        anim.SetBool("Comiendo", true);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, rango);
    }

    public void Destruir()
    {
        Destroy(parent);
    }
}
