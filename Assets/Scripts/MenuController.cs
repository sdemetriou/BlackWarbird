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

  private List<Dictionary<string, string>> abilityPipeline;

  private Dictionary<string, abilityStatusCodes> abilitySettings;
  // Meant to be used to set the status of abilities in abilitySettings
  public enum abilityStatusCodes
  {
    uncollected,
    collected,
    disabled
  }

  private bool MenuState;

  public WeaponAbilities weaponScriptInstance;

  public void setAbilitySetting(string abilityName, abilityStatusCodes statusCode)
  {
    abilitySettings[abilityName] = statusCode;
  }

  void Awake()
  {
    menuPanel.SetActive(false);
    abilityPipeline = new List<Dictionary<string, string>>();
    GameObject weapon = GameObject.Find("weapon");
    weaponScriptInstance = weapon.GetComponent<WeaponAbilities>();

    abilitySettings = new Dictionary<string, abilityStatusCodes>()
    {
      {"FirePlasmaSrc", abilityStatusCodes.uncollected}
    };
  }
  void Start()
  {
    MenuState = false;
  }

  // Update is called once per frame
  void Update()
  {
    Debug.Log(abilitySettings["FirePlasmaSrc"]);
    if (Input.GetKeyDown(KeyCode.M))
    {
      MenuState = !MenuState;
      if (MenuState)
      {
        Debug.Log("Test");

        menuPanel.SetActive(true);

        Time.timeScale = 0f;
        MenuState = true;

// Needs to be able to figure out which buttons to make appear on the menu depending on what abilities the player collected.
        GameObject[] srcAbilityButtons = GameObject.FindGameObjectsWithTag("SourceAbilityButton");
        GameObject srcAbilityButton = srcAbilityButtons[0];


        switch (abilitySettings["FirePlasmaSrc"])
        {
          case abilityStatusCodes.collected:
            srcAbilityButton.SetActive(true);
            break;
          case abilityStatusCodes.uncollected:
            srcAbilityButton.SetActive(false);
            break;
        }

      }
      else
      {
        //weaponScriptInstance.createPipeline(abilityPipeline);

        menuPanel.SetActive(false);
        //firePlasmaSrcButton.SetActive(false);
        //firePlasmaSrcButtonFunction.SetActive(false);

        Time.timeScale = 1f;
        MenuState = false;
      }
    }
  }
}
