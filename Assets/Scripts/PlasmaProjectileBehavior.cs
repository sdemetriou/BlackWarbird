using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaProjectileBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      transform.Translate(Vector2.right * 4 * Time.deltaTime);
      rb.velocity = Vector2.right * 4;
    }
}
