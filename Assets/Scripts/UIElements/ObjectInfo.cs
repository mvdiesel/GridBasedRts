using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//---------------------------------------------------

//This script is responsible for ObjectInfo UI logic

//---------------------------------------------------

public class ObjectInfo : MonoBehaviour
{
    [SerializeField] private Image buildingSprite;
    [SerializeField] private Text buildingName;
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private GameObject productsTable;
    [SerializeField] private GameObject buttonPrefab;
    private bool isButtonCreated = false;

    private Building boundBuilding;
    private GameObject boundPrefab;

    public void Bind(Building building, GameObject prefab)  //Bind GameObject data with UI
    {
        boundBuilding = building;
        boundPrefab = prefab;
        List<GameObject> units = new List<GameObject>();
    
        if (boundBuilding != null)
        {
            panelRoot.SetActive(true);
            buildingSprite.sprite = boundBuilding.GetComponent<Building>().getSprite();
            buildingName.text = boundBuilding.GetComponent<Building>().getName();
            units = boundBuilding.GetComponent<Building>().getUnitList();
        }
        else
        {
            panelRoot.SetActive(false);
        }

        if(units.Any()){    //If building can spawn units, activate products table and add necessary buttons if not added                                                          
            productsTable.SetActive(true);
            if(!isButtonCreated){
                for(int i = 0; i < units.Count(); i++){
                    GameObject newButton = Instantiate(buttonPrefab) as GameObject;
                    newButton.GetComponentInChildren<Text>(true).text = units[i].name;
                    newButton.GetComponentInChildren<Image>(true).sprite = units[i].GetComponent<Unit>().getSprite();
                    newButton.name = units[i].name;
                    newButton.gameObject.SetActive(true);
                    newButton.transform.SetParent(productsTable.transform, false);
                }

                isButtonCreated = true;
            }
        }
        else{
            productsTable.SetActive(false);
            Debug.Log("No Units!");
        }
    }

    public void fireEvent(){        //Fire event to spawn units when buttons are pressed
        string name = EventSystem.current.currentSelectedGameObject.name;
        boundPrefab.GetComponent<Building>().getUnit(name);
    }
}
