// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Sensor : MonoBehaviour
// {
//     public float detectionRadius = 10f;
//     private bool wasDeployed = false;

//     public void Deploy()
//     {
//         wasDeployed = true;
//         Invoke("ScanEnemies", 0.2f);
//     }

//     void ScanEnemies()
//     {
//         if (!wasDeployed) return;

//         Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
//         int zombieCount = 0;

//         foreach (Collider2D hit in hits)
//         {
//             if (hit.CompareTag("Enemy"))
//                 zombieCount++;
//         }

//         Debug.Log($"Sensor detected {zombieCount} Zombies!");
//     }
// }


// write anything we want in this code