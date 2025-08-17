using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead;
    private Slider healthBar;
    public delegate void OnDeath();
    public event OnDeath onDeathCallback;
    public GameObject healthBarPrefab;

    private Vector2 entityPos;
    private RectTransform rect;
    public Vector3 yOffset = new Vector3(0, 1.5f, 0);
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
        if (currentHealth <= 0)
            Die();

    }

    public void Die()
    {
        isDead = true;
        Debug.Log("has died");
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
        // Debug.Log($"{gameObject.tag} Health - transform.position: {transform.position}");
        Vector3 worldOffset = yOffset;
       // entityPos = transform.position + worldOffset;
        Vector3 entityScreenPos = Camera.main.WorldToScreenPoint(transform.position + worldOffset);
        rect.transform.position = entityScreenPos;
    }
}
