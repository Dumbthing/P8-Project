using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    void AssignPortals()
    {

    }


    void InstantiateRooms()
    {
        for (int i = 0; i < rooms + 1; i++)
        {
            if (i != 0)
            {
                TagCreate.AddTag("Room1" + "_" + i);
                TagCreate.AddTag("Portal" + "_" + i + "_1");
                roomTypeOneArray[i - 1].tag = ("Room1"+"_"+i);
                Instantiate(roomTypeOneArray[i - 1], new Vector3(roomDistance * -1.5f, 0, roomDistance * i), Quaternion.identity); // Far-left

                TagCreate.AddTag("Room2" + "_" + i);
                TagCreate.AddTag("Portal" + "_" + i + "_2");
                roomTypeTwoArray[i - 1].tag = ("Room2" + "_" + i);
                Instantiate(roomTypeTwoArray[i - 1], new Vector3(roomDistance / -2f, 0, roomDistance * i), Quaternion.identity);    // Left

                TagCreate.AddTag("Room3" + "_" + i);
                TagCreate.AddTag("Portal" + "_" + i + "_3");
                roomTypeThreeArray[i - 1].tag = ("Room3" + "_" + i);
                Instantiate(roomTypeThreeArray[i - 1], new Vector3(roomDistance / 2f, 0, roomDistance * i), Quaternion.identity);   // Right

                TagCreate.AddTag("Room4" + "_" + i);
                TagCreate.AddTag("Portal" + "_" + i + "_4");
                roomTypeFourArray[i - 1].tag = ("Room4" + "_" + i);
                Instantiate(roomTypeFourArray[i - 1], new Vector3(roomDistance * 1.5f, 0, roomDistance * i), Quaternion.identity); // Far-right

                
            }
            else
            {
                TagCreate.AddTag("PortalStart");
                startRoom.tag = ("PortalStart");
                Instantiate(startRoom, new Vector3(0, 0, 0), Quaternion.identity);

                
                Instantiate(endRoom, new Vector3(0, 0, -roomDistance), Quaternion.identity);
                TagCreate.AddTag("PortalEnd");
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

    public static class TagCreate
    {
        public static void AddTag(string tag)
        {
            UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty("tags");

                for (int i = 0; i < tags.arraySize; ++i)
                {
                    if (tags.GetArrayElementAtIndex(i).stringValue == tag)
                    {
                        return;     // Tag already present, nothing to do.
                    }
                }

                tags.InsertArrayElementAtIndex(tags.arraySize);
                tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
                so.ApplyModifiedProperties();
                so.Update();
            }
        }
    }

}
