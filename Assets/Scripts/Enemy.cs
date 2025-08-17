using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Movement Settings")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float patrolRange = 5f;
    [SerializeField] public float chaseRange = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] public Transform wallCheck;

    [SerializeField] Vector2 wallBox = new Vector2(0.2f, 0.8f);
    [SerializeField] float wallDistance = 0.1f;
    [SerializeField] float turnCooldown = 0.2f;
    float turnLock = 0f; // timer



    private Rigidbody2D rb;
    private Animator anim;
    public Vector2 startPosition;
    private float direction = 1f;
    bool facingLeft = true;
    private GameObject player;
    private bool chase;
    private bool attacked;
    private float damageCooldown = 0.5f;
    private float damageTimer = 0f;
    SpriteRenderer sr;



    void Awake()
    {
        if (!gameObject) return;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("walk", true);
    }

    void Start()
    {
        startPosition = rb.position;
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Health>().onDeathCallback += HandleDeath;
        if (player) attacked = player.gameObject.GetComponent<PlayerMovement>().isAttacking;


    }

    void HandleDeath()
    {
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageTimer <= 0f)
        {
            var player = other.GetComponent<PlayerMovement>();
            var dist = transform.position.x - player.gameObject.transform.position.x;


            if (player && player.isAttacking && ((dist > 0 && player.facingRight) || (dist < 0 && !player.facingRight)))
            {
                Debug.Log("Player is attacking!");
                Health health = GetComponent<Health>();
                if (health != null)
                {
                    Debug.Log("ENEMY TOOK DAMAGE!");
                    health.TakeDamage(10);
                    Debug.Log($"Enemy currentHealth: {GetComponent<Health>().currentHealth}");
                    damageTimer = damageCooldown;
                }
            }
        }
    }

    void Update()
    {
        if (!gameObject) return;
        if (player && Mathf.Abs(rb.transform.position.x - player.transform.position.x) < chaseRange)
        {
            // Debug.Log($" {Mathf.Abs(transform.position.x - playerPos)}");
            chase = true;
        }
        else
        {
            chase = false;
        }

        if (chase)
            Chase();
        else
            Patrol();

        // Debug.Log($"direction: {direction}; facingLeft: {facingLeft}");
        if (direction > 0 && facingLeft || direction < 0 && !facingLeft)
        {
            // Debug.Log("FLIPPED");
            Flip();
        }


        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
        if (player) attacked = player.gameObject.GetComponent<PlayerMovement>().isAttacking;

        // if (turnLock > 0f) turnLock -= Time.deltaTime;

        // if (IsWallAhead() && turnLock <= 0f)
        // {
        //     direction *= -1f;          // flip once
        //     turnLock = turnCooldown;   // prevent ping-pong
        // }

        HandleWallCollisions();

    }

    bool IsWallAhead()
    {
        // only check the side we're moving toward
        Vector2 dir = direction >= 0f ? Vector2.right : Vector2.left;

        // cast a small box from wallCheck forward
        var hit = Physics2D.BoxCast(wallCheck.position, wallBox, 0f, dir, wallDistance, groundLayer);
        if (!hit) return false;

        // require a mostly-horizontal surface (ignore floor/ceil)
        // normal.x is opposite of our movement when it's a wall
        float nx = hit.normal.x;
        bool horizontal = Mathf.Abs(nx) > 0.5f && Mathf.Abs(nx) >= Mathf.Abs(hit.normal.y);
        bool opposing = Mathf.Sign(nx) == -Mathf.Sign(dir.x);
        return horizontal && opposing;
    }

    public void HandleWallCollisions()
    {
        var leftWall = Physics2D.Raycast(wallCheck.position, Vector2.left, 1.2f, groundLayer);
        var rightWall = Physics2D.Raycast(wallCheck.position, Vector2.right, 1.2f, groundLayer);
        if (leftWall)
        {
            Debug.Log("wall is on LEFT");
            direction = 1;
        }
        else if (rightWall)
        {
            Debug.Log("wall is on RIGHT");
            direction = -1;

        }
    }

    public void Move()
    {
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    // void OnCollisionStay2D(Collision2D other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //     {
    //         // Check if hit came from left or right
    //         foreach (var contact in other.contacts)
    //         {
    //             // ground is on my left
    //             if (contact.normal.x > 0)
    //             {
    //                 facingLeft = false;
    //                 direction = 1;
    //                 Debug.Log("Enemy Hit from LEFT");
    //                 // startPosition = transform.position;
    //             }
    //             if (contact.normal.x < 0)
    //             {

    //                 facingLeft = true;
    //                 direction = -1;
    //                 Debug.Log("Enemy Hit from RIGHT");

    //             }
    //         }
    //     }
    // }


    void Patrol()
    {
        // Debug.Log("Patrolling");
        float dist = rb.transform.position.x - startPosition.x;
        if (Mathf.Abs(dist) >= patrolRange)
        {
            direction = -Mathf.Sign(dist) * 1;
            // Debug.Log("TURNED");
        }
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    void Chase()
    {
        // Debug.Log("Chasing");
        anim.SetBool("walk", true);
        float dist = Mathf.Abs(rb.transform.position.x - player.transform.position.x);
        // Debug.Log($"dist: {dist}");
        if (rb.transform.position.x - player.transform.position.x < 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        if (sr)
            sr.flipX = direction > 0;
        // transform.Rotate(0, 180, 0);
    }
}
