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
  private Dictionary<string, GameObject> abilityButtons;

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

    abilityButtons = new Dictionary<string, GameObject>()
    {
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

        // Collecting the GameObject references for all the ability buttons. Might turn this into a function.
        if (!abilityButtons.ContainsKey("FireSrcButton")) {
          GameObject[] srcAbilityButtons = GameObject.FindGameObjectsWithTag("FireSrcButton");
          if (srcAbilityButtons.Length != 0)
          {
            abilityButtons["FireSrcButton"] = srcAbilityButtons[0]; // this needs to be generalized
          }
        }

        // disable the buttons so that you can activate them depending on whether the player collected the ability
        abilityButtons["FireSrcButton"].SetActive(false);

        // different collected source abilities need to activate their respective buttons
        if (abilityButtons.ContainsKey("FireSrcButton"))
        {
          abilityButtons["FireSrcButton"].SetActive(abilitySettings["FirePlasmaSrc"] == abilityStatusCodes.collected);
        }
        else
        {

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
