using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralPrefabGeneration : MonoBehaviour {

    public GameObject startRoom, endRoom;
    public GameObject[] roomTypeOneArray;
    public GameObject[] roomTypeTwoArray;
    public GameObject[] roomTypeThreeArray;
    public GameObject[] roomTypeFourArray;

    [Tooltip("Input is multiplied by the amount of room types!")]
    public float rooms;
    public float roomDistance;

    void Start ()
    {
        // Randomizes rooms for different layouts - currently not working
        RandomizeArray(roomTypeOneArray);
        RandomizeArray(roomTypeTwoArray);
        RandomizeArray(roomTypeThreeArray);
        RandomizeArray(roomTypeFourArray);
        InstantiateRooms();
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    void InstantiateRooms()
    {
        for (int i = 0; i < rooms + 1; i++)
        {
            if (i != 0)
            {
                for (int j = 0; j < rooms; j++)
                {
                    Instantiate(roomTypeOneArray[i - 1], new Vector3(roomDistance * -1.5f, 0, roomDistance * i), Quaternion.identity); // Far-left
                    Instantiate(roomTypeTwoArray[i - 1], new Vector3(roomDistance / -2f, 0, roomDistance * i), Quaternion.identity);    // Left
                    Instantiate(roomTypeThreeArray[i - 1], new Vector3(roomDistance / 2f, 0, roomDistance * i), Quaternion.identity);   // Right
                    Instantiate(roomTypeFourArray[i - 1], new Vector3(roomDistance * 1.5f, 0, roomDistance * i), Quaternion.identity); // Far-right
                }
            }
            else
            {
                Instantiate(startRoom, new Vector3(0, 0, 0), Quaternion.identity);
                Instantiate(endRoom, new Vector3(0, 0, -roomDistance), Quaternion.identity);
            }
        }
    }

    void RandomizeArray(GameObject[] arr) // Fischer-Yates shuffle
    {
        for (int i = 0; i < arr.Length; i++)
        {
            int r = Random.Range(0, arr.Length-1);
            GameObject temp = arr[i];
            arr[i] = arr[r];
            arr[r] = temp;
        }
    }
}
