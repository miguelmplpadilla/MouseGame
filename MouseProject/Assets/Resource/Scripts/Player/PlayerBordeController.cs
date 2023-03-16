using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autor: Miguel Padilla Lillo
public class PlayerBordeController : MonoBehaviour
{

    public bool enganchadoBorde = false;

    private Animator animator;
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        rigidbody = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Borde"))
        {
            if (!enganchadoBorde && rigidbody.velocity.y < 0)
            {
                rigidbody.bodyType = RigidbodyType2D.Static;
                transform.parent.parent = col.gameObject.transform.parent;
                transform.parent.transform.localPosition = new Vector3(-0.612900019f, 0.628300011f, 0);
            
                animator.SetTrigger("engancharBorde");
            
                enganchadoBorde = true;
            }
        }
    }
}
