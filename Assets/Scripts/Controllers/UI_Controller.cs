using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------

//This script is responsible from UI behaviour

//---------------------------------------------------

public class UI_Controller : MonoBehaviour
{
    [SerializeField] public ObjectInfo objectInfo;

    private void Awake()    //Subscribing to events
    {
        ClickSelectController.OnSelectedBuildingChanged += HandleSelectedBuildingChanged;
        HandleSelectedBuildingChanged(ClickSelectController.SelectedBuilding, ClickSelectController.SelectedPrefab);
    }

    private void OnDestroy()
    {
        ClickSelectController.OnSelectedBuildingChanged -= HandleSelectedBuildingChanged;
    }

    private void HandleSelectedBuildingChanged(Building building, GameObject prefab)    //Function that binds the building with ObjectInfo UI
    {
        if (objectInfo != null)
            objectInfo.Bind(building, prefab);
    }
}
