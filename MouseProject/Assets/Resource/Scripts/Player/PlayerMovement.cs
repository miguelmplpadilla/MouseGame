using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movement;

    public float speed = 1;
    public float minSpeed = 1;
    public float maxSpeed = 2;
    
    public float jumpForce = 1;
    public float jumpForcePared = 1;

    public float estamina = 150;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private PlayerGroundController groundController;
    private PlayerGanchoController playerGanchoController;
    private PlayerDeslizarController playerDeslizarController;
    private PlayerBordeController playerBordeController;

    private bool saltandoParedes = false;
    public bool aireSaltandoPared = false;

    private RectTransform rectTransformEstamina;

    public GameObject marca;

    public Vector3[] paredJumpPoint;
    public int IparedJumpPoint;

    public Vector3[] jumpPoint;
    public int IJumpPoint;

    public Vector3 runPoint;
    public Vector3 walkPoint;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundController = GetComponentInChildren<PlayerGroundController>();
        playerGanchoController = GetComponent<PlayerGanchoController>();
        playerDeslizarController = GetComponentInChildren<PlayerDeslizarController>();
        playerBordeController = GetComponentInChildren<PlayerBordeController>();
    }

    private void Start()
    {
        rectTransformEstamina = GameObject.Find("PanelEstamina").GetComponent<RectTransform>();

        paredJumpPoint = new Vector3[10];
        jumpPoint = new Vector3[10];
    }

    void Update()
    {
        movimiento();

        if (!playerBordeController.enganchadoBorde && !playerDeslizarController.deslizandoSuelo)
        {
            saltarPared();
        }

        animator.SetFloat("verticalVelocity", rigidbody.velocity.y);

        if (saltandoParedes)
        {
            if (rigidbody.velocity.y < 0.2f)
            {
                animator.SetBool("deslizando", true);
            }
            else
            {
                animator.SetBool("deslizando", false);
            }
        } else
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (groundController.isGrounded && !playerBordeController.enganchadoBorde && !playerDeslizarController.deslizandoSuelo)
                {
                    saltar(jumpForce);
                    jumpPoint[IJumpPoint] = transform.position;
                    IJumpPoint++;
                    if (IJumpPoint>=10)
                    {
                        IJumpPoint = 0;
                    }
                }
            }
            
            animator.SetBool("deslizando", false);
        }

        animator.SetFloat("speed", speed);

        comprobarPared();
    }

    private void LateUpdate()
    {
        actualizarEstamina();
    }

    private void FixedUpdate()
    {
        if (groundController.isGrounded)
        {
            aireSaltandoPared = false;
            saltandoParedes = false;
            transform.localScale = new Vector3(1, 1, 1);
            movement = new Vector2(1, 0f);
        }

        
    }

    public void movimiento()
    {
        if (!aireSaltandoPared)
        {
            if (groundController.isGrounded && !playerDeslizarController.deslizandoSuelo)
            {
                if (estamina > 0)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        speed = maxSpeed;
                    }
                    else
                    {
                        speed = minSpeed;
                    }
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    speed = maxSpeed;
                    runPoint = transform.position;
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    speed = minSpeed;
                    walkPoint = transform.position;
                }
            }
        }

        float horizontalvelocity = movement.normalized.x * speed;

        if (!playerGanchoController.enganchado)
        {
            rigidbody.velocity =
                transform.TransformDirection(new Vector3(horizontalvelocity, rigidbody.velocity.y, 0));
        }
        else
        {
            transform.Translate(movement * Time.deltaTime * speed);
        }

        animator.SetBool("run", true);
    }

    public void saltar(float fuerzaSalto)
    {
        rigidbody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        GameObject instancia = Instantiate(marca, transform.position, Quaternion.identity);
        instancia.name = "MarcaPlayer";
        animator.SetTrigger("jump");
    }

    public void saltarPared()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (saltandoParedes)
            {
                paredJumpPoint[IparedJumpPoint] = transform.position;
                IparedJumpPoint++;
                if (IparedJumpPoint >= 10) { IparedJumpPoint = 0; }

                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                saltar(jumpForcePared);
                movement = new Vector2(-movement.x, movement.y);
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,
                    transform.localScale.z);
                //speed = minSpeed;
            }
        }
    }

    public void comprobarPared()
    {
        Vector2 direccionRay = new Vector2(transform.localScale.x, 0);
        
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direccionRay,0.1f, 1 << 6);

        Debug.DrawRay(transform.position, direccionRay, Color.red);

        if (hitInfo.collider != null && hitInfo.collider.tag.Equals("Ground"))
        {
            if (!groundController.isGrounded)
            {
                aireSaltandoPared = true;
                saltandoParedes = true;
            }
        }
        else
        {
            saltandoParedes = false;
        }
    }

    public void actualizarEstamina()
    {
        if (!playerBordeController.enganchadoBorde && !playerDeslizarController.deslizandoSuelo)
        {
            if (Input.GetButton("Fire1"))
            {
                if (estamina > 0)
                {
                    estamina -= 100f * Time.deltaTime;
                }
                else
                {
                    estamina = -150;
                }
            }
            else
            {
                if (estamina < 150)
                {
                    estamina += 80f  * Time.deltaTime;
                }
                else
                {
                    estamina = 150;
                }
            }
        }
        else
        {
            if (estamina < 150)
            {
                estamina += 80f  * Time.deltaTime;
            }
            else
            {
                estamina = 150;
            }
        }

        if (estamina >= -1 && estamina <= 150)
        {
            rectTransformEstamina.sizeDelta = new Vector2(estamina, rectTransformEstamina.sizeDelta.y);
        }
    }

    public void salirBorde()
    {
        transform.parent = null;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        saltar(jumpForcePared);
        playerBordeController.enganchadoBorde = false;
    }
}
