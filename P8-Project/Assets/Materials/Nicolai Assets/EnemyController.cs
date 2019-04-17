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

	private bool countingRespawn, currCounting, returnSpawn;

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

		if(playerDistance <= 2f  && !returnSpawn) {
			runAway();
			
		}


		else if(transform.position != spawnPoint && Vector3.Distance(transform.position, spawnPoint) <= 2 ) {
			StopCoroutine(returnSpawnCounter());
		}		
		
		else {
			StartCoroutine(returnSpawnCounter());
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
		if(!countingRespawn && !currCounting) {
			
		//	Debug.Log("Started coroutine");
			if(returnSpawn) {
			agent.isStopped = false;
			agent.destination = spawnPoint;
			
		/* else if(!currCounting) {
			Debug.Log("Entered return to spawn");
		//	Debug.Log("Stopped coroutine");	
			agent.isStopped = true;
			StopCoroutine("returnSpawnCounter");			
			countingRespawn = false; */

			if(Vector3.Distance(transform.position, spawnPoint) < 2f) {
				Debug.Log("Entered Lerp");
				agent.isStopped = true;
				transform.position = Vector3.Lerp(transform.position, spawnPoint, 0f) * Time.deltaTime;
				returnSpawn = false;
				StopCoroutine(returnSpawnCounter());
			//transform.LookAt(spawnPoint);
			//transform.position = Vector3.forward*Time.deltaTime*speed;

				}
			}

		}


	}

	IEnumerator returnSpawnCounter() {
		
		if(!countingRespawn) {
			currCounting = true;
			yield return new WaitForSeconds(returnSpawnTime);
			Debug.Log("Started counting");
			returnSpawn = true;
			currCounting = false;
			returnToSpawn();
			//if(!currCounting)
			countingRespawn = true;
		}
		

		
	}
}
