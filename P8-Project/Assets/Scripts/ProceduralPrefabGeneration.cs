using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralPrefabGeneration : MonoBehaviour {
    
    public GameObject[] roomArray;
    public float rooms;
    public float roomDistance;

    void Start ()
    {
        for (int i = 0; i < rooms; i++)
        {
            int rand = Random.Range(0, 1);
            Instantiate(roomArray[rand], new Vector3(roomDistance * i, 0, 0), Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
