using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    private GameObject _Player;
    private bool returningToSpawn, currCounting, canReturn, fleeing;

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
        if(distancePlayer < 2.0f && !returningToSpawn) {
            
            flee();
        }
        if(distancePlayer > 2.0f && distanceSpawn > 1.0f /* && !fleeing */) {
            returnToSpawn();
        }
    }

    void flee() {
        fleeing = true;
        Vector3 escape_Direction = transform.position - _Player.transform.position;
        agent.destination = escape_Direction;
        agent.isStopped = false;
    }

    void returnToSpawn() {
        returningToSpawn = true;
        if(!currCounting && !canReturn) {
        StartCoroutine(returnWaitTime());            
        }
        
        else if(canReturn) {
            Debug.Log("Returning to spawn");
            agent.destination = spawnPoint;
            agent.isStopped = false;
            Debug.Log("Distance is: " + distanceSpawn);

            if(distanceSpawn < 1.02f ) {
                Debug.Log("Agent stopped at spawnpoint");
                agent.isStopped = true;

                canReturn = false;
                returningToSpawn = false;
            }
//            agent.isStopped = true;
//            transform.position = Vector3.Lerp(transform.position, spawnPoint, 1f) * Time.deltaTime;
        StopCoroutine(returnWaitTime());
        }
        
        



    }

    IEnumerator returnWaitTime() {
        currCounting = true;
        yield return new WaitForSecondsRealtime(5f);
        canReturn = true;
        currCounting = false;
    }
}
