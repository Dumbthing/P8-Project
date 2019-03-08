using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMaze : MonoBehaviour {

	// private Collider[] amountOfOverlaps = new Collider[10];
	private int amountOfCollisions;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collPlayer) {
		if(collPlayer.gameObject.tag == "Player") {
			// Method 1: 
			// Disable render for whole level or just the single wall, and keep level.
			// levelrenderer.enabled = false;
			// Change skybox to black
			// Enable renderer for wall they left
			// collWall.renderer.enabled = true;
			// Instantiate return point (pressure plate) to check return to area?
			// Instantiate(returnPosCheck, collWall.transform.position, collWall.transform.rotation);


			// Method 2:
			// Have a layer for walls to be removed
			// Gather information onCollision with a wall
			// Assign that wall to culling mask layer 
			// When user gets back, asign old layer.

			// Method 3:
			// This should work for being outside the Maze, not sure about inside wall.
			// On collision with the wall, get information of position.
			// Set near clip plane to be equal to the distance between player and wall.			
			// If distance < 0 set to normal near clip / break code. This should stop render of any other part of the maze. 
			// originalCamNearClip = Camera.nearClipPlane
			// newCamNearClip = distance(player.transform.position, wallColl.transform.position)
			// Camera.nearClipPlane = newCamNearClip;
			// if {playerBack} {
			// Camera.nearClipPlane = originalCamNearClip;
			//}


			// Method 4:
			// This is a bad method. As it just changes all to black
			// Use Physics.OverlapSphereNonAlloc to check for amount of collisions > 0, 
			// amountOfCollisions = Physics.OverlapSphereNonAlloc(collPlayer.transform.position, 0.1f, amountOfOverlaps);
			// if(amountOfCollisions > 0) {
			//	GUI.DrawTexture(Rect(0,0,Screen.width, Screen.height), blackTexture);
			//}


		}
	}
}
