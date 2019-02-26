using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCollission : MonoBehaviour {

// public GameObject PressurePlate;
public GameObject Enemy;

public GameObject Trap;
	
public bool poison;

public bool enemy;

private ParticleSystem _pSystem;

private float randTrap;

private bool trapChosen, duckChosen;

private GameObject HMDPosition;

public float midpointPerson;

public float midpointHeadMidpoint;

private float randDuck;

private GameObject projectile;

public Vector3 localLeftWallOffset, localRightWallOffset; 

	// Use this for initialization
	void Start () {
		_pSystem = Trap.GetComponent<ParticleSystem>();
		HMDPosition = GameObject.FindWithTag("HMD");
		localLeftWallOffset = Trap.transform.position + localLeftWallOffset;
		localRightWallOffset = Trap.transform.position + localRightWallOffset;
		projectile = GameObject.FindGameObjectWithTag("Projectile");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider targetObject) {

		// Set the random value for the pressure plate, then deactivate this.
		if(!trapChosen) {
			randTrap = Random.value;
			trapChosen = true;
		}
		// Using the random value to determine which trap should be activated.
		if(targetObject.gameObject.tag == "Player" && randTrap <= 0.33f) {
			ActivateEnemy();
			Debug.Log("Collision achieved" + targetObject + "");
		}

		if(targetObject.gameObject.tag == "Player" && randTrap > 0.33 && randTrap <= 0.65) {
			ActivateDuck();
		}

		if(targetObject.gameObject.tag == "Player" && randTrap > 0.66f && _pSystem != null) {
			ActivatePoison();
			Debug.Log("Collision detected" + " " + targetObject + " " + _pSystem);
		}
	}
	
	void OnTriggerExit(Collider targetObject) {
		DeactivatePoison();
	}

	void ActivateEnemy() {
		Instantiate(Enemy, Trap.transform.position, Trap.transform.rotation);
	} 

	void ActivatePoison() {
		_pSystem.Play();
	}

	void DeactivatePoison() {
		_pSystem.Stop();
	}

	void ActivateDuck() {
		if(!duckChosen) {
			randDuck = Random.value;
			duckChosen = true;
		}

		Debug.Log("Random is: " + randDuck);
		// Instantiate a projectile/spears/something at midpointHeadMidPoint, as it should give player
		// 50% of their body length, to duck under it. 
		// Then make it move in front of the trap (towards the player)
		// Randomize the position of the prefab/projectile, one for each wall, and one for ducking
		// This makes the traps less predictable, and offers more difficulty if the player has to hug either wall or duck, instead of just ducking.
		// Instantiate()

		if(randDuck <= 0.33) {
			leftFireWall();
			
		}
		if(randDuck > 0.33 && randDuck <= 0.65) {
			rightFireWall();
		}
		if(randDuck >= 0.66) {
			duckWall();
		}


	}

	void leftFireWall() {
			Debug.Log("Firewall = leftFireWall");
			Instantiate(projectile, localLeftWallOffset, projectile.transform.rotation);
		}
	void rightFireWall() {
			Debug.Log("Firewall = rightFireWall");
		}

	void duckWall() {
			midpointPerson = HMDPosition.transform.position.y /2;
			midpointHeadMidpoint = midpointPerson + midpointPerson /2; 
			Debug.Log("Values are: " + midpointPerson + " " + midpointHeadMidpoint );
		}


}
