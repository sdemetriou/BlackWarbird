using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbilities : MonoBehaviour
{
  // Start is called before the first frame update
  public GameObject mainWeapon;
  public Transform muzzle;

  public GameObject plasmaProjectile;
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {

  }

  Dictionary<string, string> GunSourceAbility(string gunSourceAction)
  {
    Dictionary<string, string> output = new Dictionary<string, string>();
    output.Add("GunSourceAction", gunSourceAction);
    return output;
  }

  Dictionary<string, List<string>> GunTransAbility(Dictionary<string, string> source1, Dictionary<string, string> source2)
  {
    Dictionary<string, List<string>> output = new Dictionary<string, List<string>>();
    List<string> combination = new List<string>() { source1["GunSourceAction"], source2["GunSourceAction"] };
    output.Add("GunTransAction", combination);
    return output;
  }

  void executor(Dictionary<string, List<string>> actionsToPerform)
  {
    List<string> actions = actionsToPerform["GunTransAction"];
    foreach (string action in actions)
    {
      switch (action)
      {
        case "SHOOT_PROJ_PLASMA":
          // spawns plasma projectile
          Instantiate(plasmaProjectile, muzzle.position, muzzle.rotation);
          break;
      }
    }
  }
}
