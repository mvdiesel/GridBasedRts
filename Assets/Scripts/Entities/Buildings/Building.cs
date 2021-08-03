using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string name;
    public int width, height;
    public List<Vector2Int> currentSpace = new List<Vector2Int>();
    [SerializeField] public List<GameObject> unitList = new List<GameObject>();
    [SerializeField] public Sprite buildingSprite;

    public Building(){
        this.name = "Building";
        this.width = 1;
        this.height = 1;
    }

    public string getName(){
        return name;
    }

    public int getWidth(){
        return width;
    }

    public int getHeight(){
        return height;
    }

    public List<GameObject> getUnitList(){
        return unitList;
    }

    public Sprite getSprite(){
        return buildingSprite;
    }

    public void setCurrentSpace(List<Vector2Int> vectorList){
        foreach(Vector2Int vector in vectorList){
            currentSpace.Add(vector);
        }
    }

    public List<Vector2Int> getOccupiedSpace(Vector2Int offset){
        List<Vector2Int> occupiedTiles = new List<Vector2Int>();

        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                occupiedTiles.Add(offset + new Vector2Int(i, j));
            }
        }

        return occupiedTiles;
    }

    public void getUnit(string name){ 
        GameObject gameManager = GameObject.FindWithTag("GameManager");

        for(int i = 0; i < unitList.Count; i++){
            if(unitList[i].name == name){
                gameManager.GetComponent<GridBuilder>().spawnUnit(unitList[i], currentSpace[0]);
            }
        }
    }
}
