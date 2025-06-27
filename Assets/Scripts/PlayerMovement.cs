using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator anim;
    private bool facingRight = true;
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool jumpPressed;
    public bool isAttacking = false;
    private float attackDuration = 0.5f;
    private float attackTimer = 0f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("idle", true);
        anim.SetBool("walk", false);
    }

    void Start()
    {
        GetComponent<Health>().onDeathCallback += HandleDeath;

    }
    void HandleDeath()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health health = GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(10);
                Debug.Log("Player was hit!");
            }
        }
    }



    void Update()
    {
        SetVelocity();
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;
            anim.SetBool("attack", true);
            anim.SetBool("idle", false);
            anim.SetBool("walk", false);
            anim.SetBool("jump", false);
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
                anim.SetBool("attack", false);
            }
        }
    }


    void SetVelocity()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        var velocityX = movement.x * moveSpeed;
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
        FixAnimations(velocityX);
        if (velocityX < 0 && facingRight || velocityX > 0 && !facingRight)
            Flip();
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = true;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    void FixAnimations(float velocityX)
    {
        // airborne
        if (!IsGrounded())
        {
            anim.SetBool("jump", true);
            anim.SetBool("walk", false);
            anim.SetBool("idle", false);
        }
        else
        {
            // on ground
            anim.SetBool("jump", false);

            if (velocityX != 0)
            {
                anim.SetBool("walk", true);
                anim.SetBool("idle", false);
            }
            else
            {
                anim.SetBool("walk", false);
                anim.SetBool("idle", true);
            }
        }
    }
}
