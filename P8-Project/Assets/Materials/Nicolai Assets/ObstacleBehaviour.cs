using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleBehaviour : MonoBehaviour {

	public GameObject projectile, particleGas, enemy;
	public float gasAmountSeconds;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}		
	void onCollisionEnter(Collider enemyCol) {
		if(enemyCol.gameObject.tag == "Player") {
			if(gameObject.tag == "Projectile") {
				// Damage player/Other consequence
				
			}
			if(gameObject.tag == "gas") {
				while(enemyCol) {
					// Need a counter for seconds here
					gasAmountSeconds += Time.deltaTime;
					// int gasDamage = Player.health/12 * gasAmountSeconds;
					// Player.health = Player.health - gasDamage
					
					//Damage player for amount of seconds/ticks in the gas
				}
			}

		}

	}
}
