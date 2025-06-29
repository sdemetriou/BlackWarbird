using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbilities : MonoBehaviour
{
  // Start is called before the first frame update
  public GameObject mainWeapon;
  public Transform muzzle;
  public GameObject plasmaProjectile;

  private List<Dictionary<string, string>> abilityPipeline;

  public void createPipeline(List<Dictionary<string, string>> pl)
  {
    abilityPipeline = pl;
  }
  void Awake()
  {
    abilityPipeline = new List<Dictionary<string, string>>();
  }
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
    {
      // Check the function pipeline given by the menu, and execute it.
      // Traverses the function pipeline data structure and does the right executions, 
      // Before passing the final output to the executor function. 
      List<string> actions = new List<string>() {"SHOOT_PROJ_PLASMA"};
      Dictionary<string, List<string>> actionDict = new Dictionary<string, List<string>>();
      actionDict.Add("GunTransAction", actions);
      executor(actionDict);
      Debug.Log("F pressed");
    }
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

  // Executor function takes in a list of instructions, 
  // and figures out what to do with them.
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
