using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//---------------------------------------------------

//This script is responsible for BuildMenu UI element logic

//---------------------------------------------------

public class BuildMenu : MonoBehaviour
{
    [SerializeField] private BuildingFactory bf;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject panel;

    public static event Action<string> OnButtonPressed;

    private List<GameObject> buildingTypes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        buildingTypes = bf.getBuildingTypes();  //Get all building types at start from BuildingFactory
        createButtons(buildingTypes);           
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createButtons(List<GameObject> buildings){     //Create a button for each building type dynamically
        foreach(GameObject building in buildings){
            GameObject newButton = Instantiate(buttonPrefab) as GameObject;
            newButton.GetComponentInChildren<Text>(true).text = building.name;
            newButton.GetComponentInChildren<Image>(true).sprite = building.GetComponent<Building>().getSprite();
            newButton.name = building.name;
            newButton.SetActive(true);
            newButton.transform.SetParent(panel.transform, false);
        }
    }

    public void fireEvent(){        //Fire event in case a button is pressed
        string name = EventSystem.current.currentSelectedGameObject.name;
        OnButtonPressed?.Invoke(name);
    }
}
