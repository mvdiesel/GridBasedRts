using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------

//This script is responsible from the placement of objects onto the grid

//---------------------------------------------------

public class GridBuilder : MonoBehaviour
{
    ObjectPooler objectPooler;
    public GameObject grid;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;   //Getting ObjectPooler singleton
    }

    public void spawnBuilding(string buildingName){                                                                                          
        Vector3 mousePos = new Vector3();
        Vector3 screenPos = new Vector3();
        Vector2Int hitObjectPos = new Vector2Int();
        Vector3 dockPos = new Vector3();
        bool canBuild = true;

        if(Input.GetMouseButtonDown(0)){    //Raycasting on left click onto the grid
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
            if (hit)
            {
                if(hit.collider.gameObject.tag == "Tile" && hit.collider.gameObject.GetComponent<Tile>().isEmpty){
                    hitObjectPos = new Vector2Int(hit.collider.gameObject.GetComponent<Tile>().getX(), hit.collider.gameObject.GetComponent<Tile>().getY());
                    //Hit tile position
                }
                else{
                    return;
                }
            }

            dockPos = hit.collider.gameObject.transform.GetChild(1).gameObject.transform.position;  //Dock position to attach building
            GameObject spawnedObj = objectPooler.getObjectFromPool(buildingName);   //Object to be spawned

            List<Vector2Int> occupiedSpace = spawnedObj.GetComponent<Building>().getOccupiedSpace(hitObjectPos);  //Function returns occupiedSpace

            if((hitObjectPos.x + spawnedObj.GetComponent<Building>().getWidth()) > grid.GetComponent<GridController>().width || 
            (hitObjectPos.y + spawnedObj.GetComponent<Building>().getHeight()) > grid.GetComponent<GridController>().height){   //Checking if object is placed on edges
                Debug.Log("WON'T FIT!");
                canBuild = false;
                return;
            }
            foreach(Vector2Int position in occupiedSpace){      //Checking if occupiedSpace of object is empty
                if(!(grid.GetComponent<GridController>().getGridTile(position.x, position.y).isEmpty)){
                    Debug.Log("Will not build!!!!");
                    canBuild = false;
                    return;
                }
            }
            if(canBuild){   //If building placement is valid
                if(spawnedObj.GetComponent<Building>().currentSpace.Count == 0){    //If pooled building object is never used before, set occupied space
                    spawnedObj.GetComponent<Building>().setCurrentSpace(occupiedSpace);
                    foreach(Vector2Int position in occupiedSpace){
                        Debug.Log("Position " + position.x + " - " + position.y + " is occupied!");
                        grid.GetComponent<GridController>().getGridTile(position.x, position.y).setIsEmpty(false); //Disabling colliders of occupied tiles
                        grid.GetComponent<GridController>().getGridTile(position.x, position.y).GetComponent<BoxCollider2D>().enabled = 
                        !grid.GetComponent<GridController>().getGridTile(position.x, position.y).GetComponent<BoxCollider2D>().enabled;                 
                    }
                }
                else{       //If pooled building object is being moved from one location to another, clear previous occupied space and occupy new space
                    foreach(Vector2Int position in spawnedObj.GetComponent<Building>().currentSpace){
                        Debug.Log("Position " + position.x + " - " + position.y + " is cleared!");
                        grid.GetComponent<GridController>().getGridTile(position.x, position.y).setIsEmpty(true);  //Re-enabling colliders of cleared tiles
                        grid.GetComponent<GridController>().getGridTile(position.x, position.y).GetComponent<BoxCollider2D>().enabled = 
                        !grid.GetComponent<GridController>().getGridTile(position.x, position.y).GetComponent<BoxCollider2D>().enabled;            
                    }
                    spawnedObj.GetComponent<Building>().currentSpace.Clear();
                    spawnedObj.GetComponent<Building>().setCurrentSpace(occupiedSpace);
                    foreach(Vector2Int position in occupiedSpace){
                        Debug.Log("Position " + position.x + " - " + position.y + " is occupied!");
                        grid.GetComponent<GridController>().getGridTile(position.x, position.y).setIsEmpty(false);    //Disabling colliders of occupied tiles           
                        grid.GetComponent<GridController>().getGridTile(position.x, position.y).GetComponent<BoxCollider2D>().enabled = 
                        !grid.GetComponent<GridController>().getGridTile(position.x, position.y).GetComponent<BoxCollider2D>().enabled;

                        //Tile colliders are disabled when a building is placed on them to get rid of janky 2D collider behavior
                    }
                }
                
                spawnedObj = objectPooler.SpawnFromPool(buildingName, dockPos, Quaternion.identity);    //Activate object from pool
            }
            else{
                Debug.Log("CAN'T BUILD HERE!!!");
            }                       
        }
    }

    public void spawnUnit(GameObject unitToSpawn, Vector2Int buildingReference){
        int x = buildingReference.x;
        int y = buildingReference.y;
        Vector3 dockPos = new Vector3();

        if(buildingReference.x == 0 && buildingReference.y == 0){   //Check if Building spawn point is available
            Debug.Log("Can't spawn, no space!");
        }
        else{       //If the spawn point is available, spawn unit on grid
            Tile t = grid.GetComponent<GridController>().getGridTile(x, y - 1);
            dockPos = t.transform.GetChild(1).gameObject.transform.position;

            if(grid.GetComponent<GridController>().getGridTile(x, y - 1).isEmpty){
                unitToSpawn.GetComponent<Unit>().setOccupiedTile(t);
                Instantiate(unitToSpawn, dockPos, Quaternion.identity);
            }
            else{
                Debug.Log("No empty space!");
            }
        }
    }
}
