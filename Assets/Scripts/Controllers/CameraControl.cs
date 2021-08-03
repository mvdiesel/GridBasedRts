using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------

//This script is responsible from the camera movement

//---------------------------------------------------

public class CameraControl : MonoBehaviour
{
    public float Speed = 1;
 
    void Update()
    {
        float xAxisValue = Input.GetAxis("Horizontal") * Speed;
        float yAxisValue = Input.GetAxis("Vertical") * Speed;
 
        transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y + yAxisValue, transform.position.z);
    }
}
