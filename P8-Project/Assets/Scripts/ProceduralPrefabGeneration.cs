using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralPrefabGeneration : MonoBehaviour {

    public GameObject startRoom;
    public GameObject[] roomArray;
    public float rooms;
    public float roomDistance;

    void Start ()
    {
        for (int i = 0; i < rooms; i++)
        {
            if (i%2 == 0)
                Instantiate(roomArray[0], new Vector3(roomDistance * i, 0, 0), Quaternion.identity);
            else
                Instantiate(roomArray[1], new Vector3(roomDistance * i, 0, 0), Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
