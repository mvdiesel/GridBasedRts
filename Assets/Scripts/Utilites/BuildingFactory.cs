using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------

//This script is responsible from loading all building prefabs

//---------------------------------------------------

public class BuildingFactory : MonoBehaviour
{
    public List<GameObject> buildingTypes = new List<GameObject>();

    void Awake(){
        getAllBuildings();
    }
    // Start is called before the first frame update
    void Start()
    {
        getBuildingTypes();
    }

    public List<GameObject> getBuildingTypes(){
        return buildingTypes;
    }

    public void getAllBuildings(){      //Load every builidng prefab from path
        Object[] prefabs = Resources.LoadAll("Prefabs/Buildings", typeof(GameObject));

        foreach(Object obj in prefabs){
            GameObject temp = (GameObject) obj;
            buildingTypes.Add(temp);
        }
    }

    public GameObject getNewInstance_Building(string name){     //Instantiate new building
        for(int i = 0; i < buildingTypes.Count; i++){
            if(buildingTypes[i].name == name){
                Instantiate(buildingTypes[i]);
            }
        }

        return null;
    }
}
