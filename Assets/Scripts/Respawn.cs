using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *Gurshane Sidhu
 *260507632
 */

//Used to respawn the ball if it roll off the stage
public class Respawn : MonoBehaviour {

    Vector3 startPos;

	// Use this for initialization
	void Start ()
    {
        //Remember where we were when we started
        startPos = transform.position;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If the R key is pressed, reset the position of the ball
	    if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startPos;
        }	
	}
}
