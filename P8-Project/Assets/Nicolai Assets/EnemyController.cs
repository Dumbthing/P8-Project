using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	private GameObject player;
	private float distance, followDistance;
	public NavMeshAgent agent;
	private float speed = 1;

	// Used to change the radius (meters/units) of the agent
	private float agentRadius = 0.5f; 
	private float acceleration = 2;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		agent.radius = agentRadius;
		agent.speed = speed;
		agent.acceleration = acceleration;
		followDistance = 5;
		player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		//agent.destination = player.position;

		if(player != null) {
			distance = Vector3.Distance(transform.position, player.transform.position);
			if(followDistance >= distance) {
				agent.isStopped = false;
				agent.destination = player.transform.position;
			}
			else {
				agent.isStopped = true;
			}
		}
		// agent.Move(player.position);
		// Debug.Log("Player position is:" + " " + player.position);
		// Debug.Log("Agent Destination is:" + " " + agent.destination);
	}
}
