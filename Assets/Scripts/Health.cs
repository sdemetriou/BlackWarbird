using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool isDead;
    private Slider healthBar;
    public delegate void OnDeath();
    public event OnDeath onDeathCallback;
    public GameObject healthBarPrefab;

    private Vector2 entityPos;
    private RectTransform rect;
    public Vector3 yOffset;
    private GameObject healthBarObject;

    void Awake()
    {
        currentHealth = maxHealth;

        if (healthBarPrefab == null)
        {
            enabled = false;
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene. Health bars need a Canvas.");
            enabled = false;
            return;
        }

        GameObject healthBarObj = Instantiate(healthBarPrefab, canvas.transform);
        healthBarObject = healthBarObj;
        rect = healthBarObject.GetComponent<RectTransform>();
        healthBar = healthBarObject.GetComponentInChildren<Slider>();

        if (healthBar == null)
        {
            Debug.LogError("Prefab does not contain a Slider component in children.");
            enabled = false;
            return;
        }

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        currentHealth = (int)Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
        healthBar.value = currentHealth;
        if (healthBar != null)
            healthBar.value = currentHealth;
        if (currentHealth <= 0)
            Die();

    }

    public void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " has died");

        // Only reset score if the PLAYER dies
        if (CompareTag("Player"))
        {
            GameStats.score = 0;
            GameStats.enemiesKilled = 0;
        }

        Destroy(healthBarObject);
        onDeathCallback?.Invoke();
    }


    public void Heal(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;
        currentHealth = (int)Mathf.Clamp(currentHealth + amount, 0f, maxHealth);

        healthBar.value = currentHealth;
        Debug.Log("healed. Current Health: " + currentHealth);
    }

    void Start()
    {

    }

    void Update()
    {
        if (rect == null || healthBarObject == null) return;

        entityPos = transform.position + yOffset;
        var entityScreenPos = Camera.main.WorldToScreenPoint(entityPos);
        rect.position = entityScreenPos;
    }
}
