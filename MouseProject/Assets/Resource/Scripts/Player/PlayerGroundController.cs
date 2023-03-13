using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundController : MonoBehaviour
{
    public bool isGrounded;

    public bool isEnemy;

    public GameObject parentGO;
    public EnemyMovement enemyScript;
    public PlayerMovement playerScript;


    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();

        if (isEnemy == true)
        {
            enemyScript = parentGO.GetComponent<EnemyMovement>();
        }
        else
        {
            playerScript = parentGO.GetComponent<PlayerMovement>();
        }

    }

    private void Update()
    {
        animator.SetBool("grounded", isGrounded);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (isEnemy == true)
            {
                //enemyScript.speed = 1;
            }
            else
            {
                //playerScript.speed = 1;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
