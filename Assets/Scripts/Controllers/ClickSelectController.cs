using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------

//This script is responsible from Mouse selection controls

//---------------------------------------------------

public class ClickSelectController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    public static event Action<Building, GameObject> OnSelectedBuildingChanged;
    public static Building SelectedBuilding { get; private set; }
    public static GameObject SelectedPrefab { get; private set; }
    public static GameObject selectedUnit { get; private set; }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))    //Raycasting from mouse with left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
            if (hit)
            {
                var gameObject = hit.collider.gameObject;

                if(gameObject.tag == "Unit"){   //If the object is a "Unit", handling it's "isSelected" and activating/deactivating sprites
                    if(selectedUnit == null){
                        selectedUnit = gameObject;
                        selectedUnit.GetComponent<Unit>().isSelected = true;
                        selectedUnit.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else{
                        selectedUnit.GetComponent<Unit>().isSelected = false;
                        selectedUnit.transform.GetChild(1).gameObject.SetActive(false);
                        selectedUnit = gameObject;
                        selectedUnit.GetComponent<Unit>().isSelected = true;
                        selectedUnit.transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
                else{   //If the selected object is a "Building", assigning it to the variable and handling sprite work.
                    if(SelectedBuilding == null){
                        SelectedBuilding = gameObject.GetComponent<Building>();
                        SelectedPrefab = gameObject;
                        SelectedPrefab.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else{
                        return;
                    }
                    OnSelectedBuildingChanged?.Invoke(gameObject.GetComponent<Building>(), gameObject);     //Calling the event to handle UI behaviour
                }
            }
        }
      
        if (Input.GetKeyDown(KeyCode.Escape))   //Deselecting all selected objects and necessary sprite work
        {
            if(SelectedPrefab != null){
                SelectedPrefab.transform.GetChild(0).gameObject.SetActive(false);
                SelectedPrefab = null;
            }
            SelectedBuilding = null;
            if(selectedUnit != null){
                selectedUnit.GetComponent<Unit>().isSelected = false;
                selectedUnit.transform.GetChild(1).gameObject.SetActive(false);
                selectedUnit = null;
            }
            OnSelectedBuildingChanged?.Invoke(null, null);
        }
    }
}
