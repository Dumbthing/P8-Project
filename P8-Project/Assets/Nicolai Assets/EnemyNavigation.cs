using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    private GameObject _Player;
    private bool returningToSpawn, currCounting, canReturn, fleeing;
    public float spawnProximity = 1.1f;

    public float playerProximity = 2.0f;

    private Vector3 spawnPoint;

    private NavMeshAgent agent;
    
    private float distancePlayer, distanceSpawn;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        _Player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distancePlayer = Vector3.Distance(transform.position, _Player.transform.position);
        distanceSpawn = Vector3.Distance(transform.position, spawnPoint);
        agent.isStopped = false;

        if(distancePlayer > playerProximity && distanceSpawn <= spawnProximity /* && !fleeing */ ) {
            agent.isStopped = true;
        }
        else if (distancePlayer < playerProximity) {
            Vector3 escape_Direction = transform.position - _Player.transform.position;
            agent.destination = escape_Direction;
        }
        else if (distanceSpawn >= spawnProximity) {
            agent.destination = spawnPoint;
        }
    }

    void flee() {
        Vector3 escape_Direction = transform.position - _Player.transform.position;
        agent.destination = escape_Direction;
        agent.isStopped = false;
    }

    void returnToSpawn() {



/*         if(!currCounting && !canReturn) {
            StartCoroutine(returnWaitTime());
        
        }
        else if(canReturn) {
            Debug.Log("Returning to spawn");
            agent.destination = spawnPoint;
            agent.isStopped = false;
            Debug.Log("Distance is: " + distanceSpawn);
            
            // Need a condition to enter this, even though distanceSpawn is under 1.05f, as it now just freezes in place if it's standing there.
            if(distanceSpawn < spawnProximity) {
                Debug.Log("Agent stopped at spawnpoint");
                agent.isStopped = true;

                canReturn = false;
                returningToSpawn = false;
            }

        StopCoroutine(returnWaitTime());
        } */
 


    }

 /*    IEnumerator returnWaitTime() {
        currCounting = true;
        yield return new WaitForSecondsRealtime(5f);
        canReturn = true;
        currCounting = false;
    } */
}
