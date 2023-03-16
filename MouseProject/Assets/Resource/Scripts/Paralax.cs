using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento;

    private Vector2 offset;

    private Material material;

    public GameObject player;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        player = GameObject.Find("Player");
        rb2D = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        offset = (rb2D.velocity.x * 0.1f) * velocidadMovimiento * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}