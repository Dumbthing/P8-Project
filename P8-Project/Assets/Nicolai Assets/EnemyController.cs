using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	private GameObject player;
	private float distance, returnSpawnTime;
	public NavMeshAgent agent;
	private float speed = 1;

	// Used to change the radius (meters/units) of the agent
	private float agentRadius = 0.5f; 
	private float acceleration = 2;

	private bool countingRespawn;

	private Vector3 spawnPoint, newSpawnPoint;

	private Vector3 beforeRunningPosition;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		agent.radius = agentRadius;
		agent.speed = speed;
		agent.acceleration = acceleration;
		// followDistance = 5;
		player = GameObject.FindWithTag("Player");
		returnSpawnTime = 5f;
		spawnPoint = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		var playerDistance = Vector3.Distance(transform.position, player.transform.position);
		// Debug.Log("Distance is: " + playerDistance);
		//agent.destination = player.position;

/* 		if(player != null) {
			distance = Vector3.Distance(transform.position, player.transform.position);
			if(followDistance >= distance) {
				agent.isStopped = false;
				agent.destination = player.transform.position;
			}
			else {
				agent.isStopped = true;
			}
		} */

		if(playerDistance <= 2f) {
			runAway();
		}
		else {
			returnToSpawn();
		}
		// agent.Move(player.position);
		// Debug.Log("Player position is:" + " " + player.position);
		// Debug.Log("Agent Destination is:" + " " + agent.destination);


	}
    // If player is outside radius, and x amount of seconds passed (coroutine), return to instantiated spawn point. 
	void runAway() {
		Vector3 escape_Direction = transform.position - player.transform.position;
		// Vector3 wantedPosition = player.transform.position+(escape_Direction.normalized*wantedDistance);
		
		agent.destination = escape_Direction;
		//transform.LookAt(escape_Direction);
		//transform.position = wantedPosition*Time.deltaTime*speed;
	}

	void returnToSpawn() {
		if(!countingRespawn) {
			StartCoroutine("returnSpawnCounter");
		//	Debug.Log("Started coroutine");
			agent.isStopped = false;
			agent.destination = spawnPoint;
		}
		else {
			Debug.Log("Entered return to spawn");
		//	Debug.Log("Stopped coroutine");	
			agent.isStopped = true;
			StopCoroutine("returnSpawnCounter");			
			countingRespawn = false;

		if(transform.position != spawnPoint) {
			
			//transform.LookAt(spawnPoint);
			//transform.position = Vector3.forward*Time.deltaTime*speed;

		}
		else {

		}



		}


	}

	IEnumerator returnSpawnCounter() {
		
		if(!countingRespawn) {
			bool currCounting;
			yield return new WaitForSeconds(returnSpawnTime);
			currCounting = false;
			Debug.Log("Started counting");
			if(!currCounting)
			countingRespawn = true;
		}
		
		
	}
}
