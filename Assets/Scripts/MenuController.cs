using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
  // Start is called before the first frame update

  // The menu panel object
  public GameObject menuPanel;
  [SerializeField] private GameObject firePlasmaSrcButton;
  [SerializeField] private GameObject firePlasmaSrcButtonFunction;

  private bool MenuState;

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
    MenuState = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.M))
    {
      MenuState = !MenuState;
      if (MenuState)
      {
        Debug.Log("Test");

        menuPanel.SetActive(true);
        firePlasmaSrcButton.SetActive(true);
        firePlasmaSrcButtonFunction.SetActive(true);

        Time.timeScale = 0f;
        MenuState = true;
      }
      else
      {
        weaponScriptInstance.createPipeline(abilityPipeline);

        menuPanel.SetActive(false);
        firePlasmaSrcButton.SetActive(false);
        firePlasmaSrcButtonFunction.SetActive(false);

        Time.timeScale = 1f;
        MenuState = false;
      }
    }
  }
}
