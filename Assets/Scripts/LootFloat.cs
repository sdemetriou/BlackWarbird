using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootFloat : MonoBehaviour
{
    private Rigidbody2D rb;
    public float floatRange = 0.5f;
    private Vector2 startPos;
    private int direction = 1;
    public float moveSpeed = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = rb.position;
    }

    void Update()
    {

        float posY = startPos.y;
        if (rb.position.y < posY - floatRange)
            direction = 1;
        else if (rb.position.y > posY + floatRange)
            direction = -1;
        rb.velocity = new Vector2(rb.velocity.x, direction * moveSpeed);
    }
}
