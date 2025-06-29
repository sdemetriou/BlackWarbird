using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
  // Start is called before the first frame update

  // The menu panel object
  public GameObject menuPanel;

  private bool MenuToggle;

  public WeaponAbilities weaponScriptInstance;
  private List<Dictionary<string, string>> abilityPipeline;

  void Awake()
  {
    abilityPipeline = new List<Dictionary<string, string>>();
    GameObject weapon = GameObject.Find("weapon");
    weaponScriptInstance = weapon.GetComponent<WeaponAbilities>();
  }
  void Start()
  {
    MenuToggle = true;
    menuToggle();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.M))
    {
      menuToggle();
      pauseGame();
    }
    else
    {
      weaponScriptInstance.createPipeline(abilityPipeline);
      menuToggle();
      unpauseGame();
    }
  }

  void menuToggle()
  {
    MenuToggle = !MenuToggle;
    menuPanel.SetActive(MenuToggle);
  }
  void pauseGame()
  {
    Time.timeScale = 0;
  }
  void unpauseGame()
  {
    Time.timeScale = 1;
  }
}
