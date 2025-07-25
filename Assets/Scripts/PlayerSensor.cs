using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSensor : MonoBehaviour
{
    public GameObject sensorPrefab;
    public float throwDistance = 3f;
    public int sensorCount = 0;
    public float detectionRadius = 10f;
    private bool wasDeployed = false;

    public Text enemyInRangeCountTxt;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && sensorCount > 0)
        {
            // ThrowSensor();
            // var player = GameObject.FindGameObjectWithTag("Player");
            Deploy();

            // var dropPoint = player.transform.position.x + throwDistance;
            // GameObject sensor = Instantiate(sensorPrefab, new Vector2(dropPoint, player.transform.position.y), Quaternion.identity);
            // sensor.GetComponent<Sensor>().Deploy();
            sensorCount--;
            // Debug.Log("Sensor deployed!");
        }
    }

    public void Deploy()
    {
        wasDeployed = true;
        Invoke("ScanEnemies", 0.2f);
    }

    void ScanEnemies()
    {
        if (!wasDeployed) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        int zombieCount = 0;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
                zombieCount++;
        }
        enemyInRangeCountTxt.text = $"Sensor detected {zombieCount} Zombies!";

        //$"Sensor detected {zombieCount} Zombies!";
        //Debug.Log($"Sensor detected {zombieCount} Zombies!");
    }

    void ThrowSensor()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        Vector2 spawnPos = (Vector2)transform.position + direction * throwDistance;

        Debug.Log($"Sensor thrown to {spawnPos}");
    }
}
