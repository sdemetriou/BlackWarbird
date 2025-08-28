using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Battery : MonoBehaviour
{
   
    void Start()
    {
      
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().Heal(50);
            Destroy(gameObject);
        }
    }
}
