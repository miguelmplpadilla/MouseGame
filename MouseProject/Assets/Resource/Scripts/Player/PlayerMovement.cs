using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autor: Miguel Padilla Lillo
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

    private Vector3 posicionAnteriro;

    private RaycastHit2D hitInfo;
    private Vector2 direccionRay;

    public bool bloqueoSprint = true;
    public bool bloqueoSaltar = true;

    private PlayerPoints playerPoints;

    public bool desbloquearVar = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundController = GetComponentInChildren<PlayerGroundController>();
        playerGanchoController = GetComponent<PlayerGanchoController>();
        playerDeslizarController = GetComponentInChildren<PlayerDeslizarController>();
        playerBordeController = GetComponentInChildren<PlayerBordeController>();

        playerPoints = GetComponent<PlayerPoints>();

        if (desbloquearVar)
        {
            gameObject.BroadcastMessage("desbloquearVariables");
        }
    }

    private void Start()
    {
        rectTransformEstamina = GameObject.Find("PanelEstamina").GetComponent<RectTransform>();

        posicionAnteriro = transform.position;
        direccionRay = new Vector2(transform.localScale.x, 0);
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
            if (!bloqueoSaltar)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    if (groundController.isGrounded && !playerBordeController.enganchadoBorde && !playerDeslizarController.deslizandoSuelo)
                    {
                        saltar(jumpForce);
                        playerPoints.MakeJumpPoint();
                    }
                }
            }

            animator.SetBool("deslizando", false);
        }

        animator.SetFloat("speed", speed);
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

        comprobarPared();
    }

    public void movimiento()
    {
        if (!aireSaltandoPared && !playerGanchoController.enganchado)
        {
            if (groundController.isGrounded && !playerDeslizarController.deslizandoSuelo)
            {
                if (estamina > 0)
                {
                    if (!bloqueoSprint)
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
                    else
                    {
                        speed = minSpeed;
                    }
                }
                else
                {
                    speed = minSpeed;
                }
            }
        }
        else
        {
            speed = maxSpeed;

            if (playerGanchoController.enganchado)
            {
                speed = minSpeed;
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
        
        direccionRay = new Vector2(transform.localScale.x, 0);

        hitInfo = Physics2D.Raycast(transform.position, direccionRay,0.01f, 1 << 6);

        if (groundController.isGrounded && hitInfo.collider != null && hitInfo.collider.tag.Equals("Ground"))
        {
            animator.SetBool("run", false);
        }
        else
        {
            animator.SetBool("run", true);
        }
    }

    public void saltar(float fuerzaSalto)
    {
        rigidbody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        animator.SetTrigger("jump");
    }

    public void saltarPared()
    {
        if (!bloqueoSaltar)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (saltandoParedes)
                {
                    playerPoints.MakeJumpWallPoint();
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                    saltar(jumpForcePared);
                    movement = new Vector2(-movement.x, movement.y);
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,
                        transform.localScale.z);
                }
            }
        }
    }

    public void comprobarPared()
    {
        direccionRay = new Vector2(transform.localScale.x, 0);
        
        hitInfo = Physics2D.Raycast(transform.position, direccionRay,0.1f, 1 << 6);

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
        if (!playerBordeController.enganchadoBorde && !playerDeslizarController.deslizandoSuelo && groundController.isGrounded)
        {
            if (!bloqueoSprint)
            {
                if (Input.GetButton("Fire1"))
                {
                    if (estamina > 0)
                    {
                        estamina -= 50f * Time.deltaTime;
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
                        estamina += 50f * Time.deltaTime;
                    }
                    else
                    {
                        estamina = 150;
                    }
                }
            }
        }
        else
        {
            if (estamina < 150)
            {
                estamina += 50f  * Time.deltaTime;
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
    
    public void desbloquearJump()
    {
        bloqueoSaltar = false;
    }
    
    public void desbloquearFire1()
    {
        bloqueoSprint = false;
    }
    
    public void desbloquearVariables()
    {
        bloqueoSaltar = false;
        bloqueoSprint = false;
    }
}
