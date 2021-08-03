using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] public Sprite unitSprite;
    public Tile occupiedTile;
    private int currentPathIndex;
    private int speed = 1;
    private List<Tile> pathTileList;
    public bool isSelected;
    
    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelected){     //Raycasting and movement with right click
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
                if (hit)
                {
                    if(hit.collider.gameObject.tag == "Tile"){
                        var endTile = hit.collider.gameObject;

                        if(pathTileList == null){
                            setTargetPosition(endTile.GetComponent<Tile>());    //Calling pathfinding function
                        }
                    }
                }
            }
        }
        if(pathTileList != null){
            handleMovement();   //Movement is handled in every frame if there is a destination
        }
    }

    public void setOccupiedTile(Tile t){
        occupiedTile = t;
        t.GetComponent<Tile>().setIsEmpty(false);
    }

    private void handleMovement(){         //Move towards destination every frame, when too close change destination to the next "Tile"                                                                                     
        Vector3 targetPosition = pathTileList[currentPathIndex].gameObject.transform.GetChild(1).transform.position;
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        if(Vector3.Distance(transform.position, targetPosition) > .01f){
            float distanceBefore = Vector3.Distance(transform.position, targetPosition);
            transform.position = transform.position + moveDir * speed * Time.deltaTime;
        }
        else{
            currentPathIndex++;
            if(currentPathIndex >= pathTileList.Count){
                stopMoving();
            }
        }
    }

    private void stopMoving(){
        pathTileList = null;
    }

    public void setTargetPosition(Tile endTile){    //Calling pathfinding function from pathfinding singleton with start and end tiles
        occupiedTile.isEmpty = true;
        currentPathIndex = 0;
        pathTileList = Pathfinding.Instance.findPath(occupiedTile.getX(), occupiedTile.getY(), endTile.getX(), endTile.getY());
        if(pathTileList == null){
            return;
        }

        if(pathTileList != null && pathTileList.Count > 1){
            pathTileList.RemoveAt(0);
        }

        occupiedTile = endTile;
        occupiedTile.isEmpty = false;
    }

    public Sprite getSprite(){
        return unitSprite;
    }
}
