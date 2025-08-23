using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float patrolRange = 5f;
    public float chaseRange = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 startPosition;
    private float direction = 1f;
    bool facingLeft = true;
    private GameObject player;
    private bool chase;
    private bool attacked;
    private float damageCooldown = 0.5f;
    private float damageTimer = 0f;

    public bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        startPosition = rb.position;
        anim.SetBool("walk", true);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // GetComponent<Health>().onDeathCallback += HandleDeath;
        //  if (player) attacked = player.gameObject.GetComponent<PlayerMovement>().isAttacking;
        Health health = GetComponent<Health>();
        if (health != null)
            health.onDeathCallback += HandleDeath;
    }

    void HandleDeath()
    {
        isDead = true;
        Debug.Log("Enemy died: " + gameObject.name);
        // Notify LevelManager
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
            levelManager.EnemyKilled();

        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageTimer <= 0f)
        {
            var player = other.GetComponent<PlayerMovement>();
            var dist = transform.position.x - player.gameObject.transform.position.x;


            if (player && player.isAttacking && ((dist > 0 && player.facingRight) || (dist < 0 && !player.facingRight)))
            {
                Health health = GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(10);
                    damageTimer = damageCooldown;
                }
            }

        }
    }



    void FixedUpdate()
    {
        if (player)
        {
            var playerPos = player.transform.position;
            float distToPlayer = Vector2.Distance(transform.position, playerPos);
            chase = distToPlayer < chaseRange;
        }
        if (chase && player)
        {
            Chase();
        }
        else
            Patrol();


    }

    void Update()
    {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
        if (player) attacked = player.gameObject.GetComponent<PlayerMovement>().isAttacking;

    }


    void Patrol()
    {
        float posX = startPosition.x;
        if (rb.position.x < posX - patrolRange)
            direction = 1;
        else if (rb.position.x > posX + patrolRange)
            direction = -1;
        if (direction > 0 && facingLeft || direction < 0 && !facingLeft)
            Flip();
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    void Chase()
    {
        anim.SetBool("walk", true);
        Vector2 enemyPos = rb.position;
        Vector2 playerPos = player.transform.position;
        Vector2 dir = (playerPos - enemyPos).normalized;
        if (!facingLeft && dir.x < 0 || facingLeft && dir.x > 0)
            Flip();
        transform.position += (Vector3)dir * moveSpeed * Time.deltaTime;
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        transform.Rotate(0, 180, 0);
    }
}
