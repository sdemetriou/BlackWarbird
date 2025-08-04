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
  private List<string> abilityButtonTags;

  public void setAbilitySetting(string abilityName, abilityStatusCodes statusCode)
  {
    abilitySettings[abilityName] = statusCode;
  }


  private void storeAbilityButtonReference()
  {
    // Collecting the GameObject references for all the ability buttons. Might turn this into a function.
    foreach (string buttonTag in abilityButtonTags)
    {
      if (!abilityButtons.ContainsKey(buttonTag)) {
        GameObject[] srcAbilityButtons = GameObject.FindGameObjectsWithTag(buttonTag);
        if (srcAbilityButtons.Length != 0)
        {
          abilityButtons[buttonTag] = srcAbilityButtons[0]; // this needs to be generalized
        }
      }
    }
  }


  private void setAbilityButtonState()
  {
    foreach (string buttonTag in abilityButtonTags)
    {
      if (abilityButtons.ContainsKey(buttonTag))
      {
        abilityButtons[buttonTag].SetActive(abilitySettings[buttonTag.Replace("Button", "")] == abilityStatusCodes.collected);
      }
      else
      {

      }
    }
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

    // CONVENTION: ability button tags are to be the same as their corresponding names in abilitySettings,
    // but with "Button" added in front.
    abilityButtonTags = new List<string>(){
      "FirePlasmaSrcButton"
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

        storeAbilityButtonReference();

        // disable the buttons so that you can activate them depending on whether the player collected the ability
        foreach (string buttonTag in abilityButtonTags)
        {
          abilityButtons[buttonTag].SetActive(false);
        }

        setAbilityButtonState();

        // state of the source button is to be active when collected.
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
