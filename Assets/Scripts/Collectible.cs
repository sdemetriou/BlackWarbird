using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update
    private string abilityType;

    void Awake()
    {
      abilityType = "plasmaSrc";
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    Debug.Log("I exist.");
    }

    // Triggered when this object's 2D collider enters another object's 2D collider.
    void OnTriggerEnter2D(Collider2D foreignCollider)
    {
      GameObject foreignObject = foreignCollider.gameObject;
      if (foreignCollider.tag != "Player")
      {
        Debug.Log("uncollision");
        return;
      } 
      Debug.Log("collision!!!!!!!!!!!!!");
      GameObject[] menuObjects = GameObject.FindGameObjectsWithTag("MenuController");
      MenuController menu = menuObjects[0].GetComponent<MenuController>();
      
      menu.setAbilitySetting(abilityType, MenuController.abilityStatusCodes.collected);
      Destroy(this.gameObject);
    }
}
