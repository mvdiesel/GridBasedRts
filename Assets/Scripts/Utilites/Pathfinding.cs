using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------

//This script implements A* pathfinding algorithm

//---------------------------------------------------

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }
    private GridController gridController;
    private Tile[,] gridArray;
    private List<Tile> openList;
    private List<Tile> closedList;

    void Awake()
    {
        Instance = this;    //Creating a singleton
        gridController = GetComponent<GridController>();
    }

    void Start()    //Getting grid info from grid controller
    {
        int width = gridController.getWidth();
        int height = gridController.getHeight();
        gridArray = new Tile[width, height];

        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                gridArray[i , j] = gridController.getGridTile(i , j);
            }
        }
    }

    public List<Tile> findPath(int startX, int startY, int endX, int endY){    //Main pathfinding function
        Tile startTile = gridController.getGridTile(startX, startY);
        Tile endTile = gridController.getGridTile(endX, endY);

        openList = new List<Tile> { startTile };
        closedList = new List<Tile>();

        for(int i = 0; i < gridController.getWidth(); i++){     //Calculating G and F costs for all tiles
            for(int j = 0; j < gridController.getHeight(); j++){
                Tile t = gridController.getGridTile(i , j);
                t.gCost = int.MaxValue;
                t.calculateFCost();
                t.cameFromTile = null;
            }
        }

        startTile.gCost = 0;
        startTile.hCost = calculateDistanceCost(startTile, endTile);    //Calculating H cost from start to end tile
        startTile.calculateFCost();

        while(openList.Count > 0){      //Main while loop that iterates through the grid as long as there is a walkable tile
            Tile currentTile = getLowestFCostTile(openList);    //Selecting lowest cost tile

            if(currentTile == endTile){     //If reached to end, return path
                return calculatePath(endTile);
            }

            openList.Remove(currentTile);   //Remove currentTile from openList and add to closeList
            closedList.Add(currentTile);

            foreach(Tile neighborTile in getNeighborList(currentTile)){     //Checking neighbor tiles 
                if(closedList.Contains(neighborTile)) continue;
                if(!neighborTile.isEmpty){      //If neighbor tile is not walkable add to closedList
                    closedList.Add(neighborTile);
                    continue;
                }

                int tentativeGCost = currentTile.gCost + calculateDistanceCost(currentTile, neighborTile);  //Calculating tentativeGCost to find next tile
                if(tentativeGCost < neighborTile.gCost){    //When lower cost tile found, update info
                    neighborTile.cameFromTile = currentTile;
                    neighborTile.gCost = tentativeGCost;
                    neighborTile.hCost = calculateDistanceCost(neighborTile, endTile);
                    neighborTile.calculateFCost();

                    if(!openList.Contains(neighborTile)){
                        openList.Add(neighborTile);
                    }
                }
            }
        }

        return null;
    }

    private List<Tile> getNeighborList(Tile currentTile){       //Get tile neighbors
        List<Tile> neighborList = new List<Tile>();

        if(currentTile.getX() - 1 >= 0){
            // Left
            neighborList.Add(gridArray[currentTile.getX() - 1, currentTile.getY()]);
            // Left Down
            if(currentTile.getY() - 1 >= 0) neighborList.Add(gridArray[currentTile.getX() - 1, currentTile.getY() - 1]);
            // Left Up
            if(currentTile.getY() + 1 < gridController.getHeight()) neighborList.Add(gridArray[currentTile.getX() - 1, currentTile.getY() + 1]);
        }
        if(currentTile.getX() + 1 < gridController.getWidth()){
            // Right
            neighborList.Add(gridArray[currentTile.getX() + 1, currentTile.getY()]);
            // Right Down
            if(currentTile.getY() - 1 >= 0) neighborList.Add(gridArray[currentTile.getX() + 1, currentTile.getY() - 1]);
            // Right Up
            if(currentTile.getY() + 1 < gridController.getHeight()) neighborList.Add(gridArray[currentTile.getX() + 1, currentTile.getY() + 1]);
        }
        // Down
        if(currentTile.getY() - 1 >= 0) neighborList.Add(gridArray[currentTile.getX(), currentTile.getY() - 1]);
        // Up
        if(currentTile.getY() + 1 < gridController.getHeight()) neighborList.Add(gridArray[currentTile.getX(), currentTile.getY() + 1]);

        return neighborList;
    }

    private List<Tile> calculatePath(Tile endTile){     //From end to start make a list of tiles and return path
        List<Tile> path = new List<Tile>();
        path.Add(endTile);
        Tile currentTile = endTile;

        while(currentTile.cameFromTile != null){
            path.Add(currentTile.cameFromTile);
            currentTile = currentTile.cameFromTile;
        }
        path.Reverse();
        
        return path;
    }

    private int calculateDistanceCost(Tile a, Tile b){
        int xDistance = Mathf.Abs(a.getX() - b.getX());
        int yDistance = Mathf.Abs(a.getY() - b.getY());
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private Tile getLowestFCostTile(List<Tile> tileList){
        Tile lowestFCostTile = tileList[0];

        for(int i = 0; i < tileList.Count; i++){
            if(tileList[i].fCost < lowestFCostTile.fCost){
                lowestFCostTile = tileList[i];
            }
        }
        
        return lowestFCostTile;
    }
}
