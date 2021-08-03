using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileId;
    public int x, y;
    public int gCost, hCost, fCost;

    public Tile cameFromTile;

    [SerializeField] private Sprite baseSprite, offsetSprite;
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject occupied;
    [SerializeField] private SpriteRenderer sr;

    public bool isEmpty;
    // Start is called before the first frame update
    void Start()
    {
        isEmpty = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void colorPicker(bool isOffset){
        if(isOffset){
            sr.sprite = offsetSprite;
        }
        else{
            sr.sprite = baseSprite;
        }
    }

    public int getTileId(){
        return tileId;
    }

    public int getX(){
        return x;
    }

    public int getY(){
        return y;
    }

    public void setIsEmpty(bool value){
        this.isEmpty = value;
    }

    public void setOccupied(bool value){
        occupied.SetActive(value);
    }

    public void calculateFCost(){
        fCost = gCost + hCost;
    }

    void OnMouseEnter(){
        highlight.SetActive(true);
    }

    void OnMouseExit(){
        highlight.SetActive(false);
    }
}
