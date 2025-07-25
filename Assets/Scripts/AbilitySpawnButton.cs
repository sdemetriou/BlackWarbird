using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySpawnButton : MonoBehaviour
{
  [SerializeField] private GameObject abilityToSpawn;
  private GameObject spawnedAbility;
  private bool isAbilitySpawned;
  // Start is called before the first frame update
  void Start()
  {
    isAbilitySpawned = false;
  }

  // Update is called once per frame
  void Update()
  {

    if (isAbilitySpawned)
    {
      Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePosition.z = 0f;
      spawnedAbility.transform.position = mousePosition;
    }
  }

  public void buttonPress()
  {
    spawnedAbility = Instantiate(abilityToSpawn);
    isAbilitySpawned = true;
  }
}
