using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] public int width, height;
    [SerializeField] private Tile referenceTile;
    [SerializeField] private Transform cameraPosition;
    protected Dictionary<Vector2Int, Tile> tileDictionary = new Dictionary<Vector2Int, Tile>();
    private bool isOccupied;
    private bool isBuilt;
    // Start is called before the first frame update

    void Awake()
    {
        generateGrid();
    }
    
    void Start()
    {
        Debug.Log(tileDictionary[new Vector2Int(10, 10)].name);
        isOccupied = false;

        BuildMenu.OnButtonPressed += eventCatcher;
    }

    // Update is called once per frame
    void Update()
    {
        OnBuildModeFinish();
    }

    void generateGrid(){    //Generating game grid and adding "Tile" objects to "TileDictionary"
        int id = 0;
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                var generatedTile = Instantiate(referenceTile, new Vector3(x, y) * .3f, Quaternion.identity);
                generatedTile.name = $"Tile {x} {y}";
                generatedTile.tileId = id;
                generatedTile.x = x;
                generatedTile.y = y;
                id++;

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);   //Changing sprites of offset tiles
                generatedTile.colorPicker(isOffset);

                tileDictionary.Add(new Vector2Int(x,y), generatedTile);
            }
        }

        cameraPosition.transform.position = new Vector3((float)width * .15f, (float)height * .15f, -10);    //Setting up initial camera position
    }

    public Tile getGridTile(int x, int y){
        return tileDictionary[new Vector2Int(x, y)];
    }

    IEnumerator OnBuildModActivated(string buildingName){   //Coroutine responsible for handling build functions
        foreach(KeyValuePair<Vector2Int, Tile> entry in tileDictionary){    //Setting "OccupiedSprite" objects during coroutine (Red squares)
            if(!entry.Value.isEmpty){
                entry.Value.setOccupied(true);
            }
        }

        while(!Input.GetMouseButtonDown(0)){    //Returning null until the player places the building with left click
            yield return null;
        }
        
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        gameManager.GetComponent<GridBuilder>().spawnBuilding(buildingName);    //Calling build function from GameManager

        yield break;

    }

    public Dictionary<Vector2Int, Tile> getGridDictionary(){
        return tileDictionary;
    }

    public int getWidth(){
        return width;
    }

    public int getHeight(){
        return height;
    }

    public void eventCatcher(string buildingName){          //Event catcher function that catches button presses from UI
        StartCoroutine(OnBuildModActivated(buildingName));
    }

    public void OnBuildModeFinish(){    //Turning off "OccupiedSprite" objects
        if(Input.GetMouseButtonDown(0)){
            foreach(KeyValuePair<Vector2Int, Tile> entry in tileDictionary){
                entry.Value.setOccupied(false);
            }
        }
    }
}
