using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMenuItem : MonoBehaviour
{
  // Start is called before the first frame update
    public string type;

    [SerializeField] private RectTransform workspace;
    [SerializeField] private Canvas uiCanvas;
  

    void Awake()
    {
      
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           //RectTransform.anchoredPosition = RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, Input.mousePosition, camera);  
    }
}
