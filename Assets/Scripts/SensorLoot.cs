using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorLoot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerSensor>().sensorCount++;
            Debug.Log("Sensor collected!");
            Destroy(gameObject);
        }
    }
}

