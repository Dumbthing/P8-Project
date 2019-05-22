

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProceduralPrefabGeneration : MonoBehaviour {

    //public PortalTeleporter portalTele;

    public GameObject startRoom, endRoom;
    public GameObject[] roomTypeOneArray;
    public GameObject[] roomTypeTwoArray;
    public GameObject[] roomTypeThreeArray;
    public GameObject[] roomTypeFourArray;

    GameObject portalStart;
    GameObject portalEnd;
    GameObject portal;
    GameObject portal2;
    GameObject portal3;
    GameObject portal4;

    GameObject portalTemp;
    Transform portalTemp2;

    [Tooltip("Input is multiplied by the amount of room types!")]
    public float rooms;
    public float roomDistance;

    void Start ()
    {
        /// Randomizes rooms for different layouts
        RandomizeArray(roomTypeOneArray);
        RandomizeArray(roomTypeTwoArray);
        RandomizeArray(roomTypeThreeArray);
        RandomizeArray(roomTypeFourArray);
        InstantiateRooms();
    }

    void InstantiateRooms()
    {
        for (int i = 0; i < rooms + 1; i++)
        {
            if (i != 0)
            {
                TagCreate.AddTag("Room1" + "_" + i);
                roomTypeOneArray[i - 1].tag = ("Room1"+"_"+i);
                Instantiate(roomTypeOneArray[i - 1], new Vector3(roomDistance * -1.5f, 0, roomDistance * i), Quaternion.identity); // Far-left
                portal = GameObject.FindGameObjectWithTag("Room1" + "_" + i);

                TagCreate.AddTag("Room2" + "_" + i);
                roomTypeTwoArray[i - 1].tag = ("Room2" + "_" + i);
                Instantiate(roomTypeTwoArray[i - 1], new Vector3(roomDistance / -2f, 0, roomDistance * i), Quaternion.identity);    // Left
                portal2 = GameObject.FindGameObjectWithTag("Room2" + "_" + i);

                TagCreate.AddTag("Room3" + "_" + i);
                roomTypeThreeArray[i - 1].tag = ("Room3" + "_" + i);
                Instantiate(roomTypeThreeArray[i - 1], new Vector3(roomDistance / 2f, 0, roomDistance * i), Quaternion.identity);   // Right
                portal3 = GameObject.FindGameObjectWithTag("Room3" + "_" + i);

                TagCreate.AddTag("Room4" + "_" + i);
                roomTypeFourArray[i - 1].tag = ("Room4" + "_" + i);
                Instantiate(roomTypeFourArray[i - 1], new Vector3(roomDistance * 1.5f, 0, roomDistance * i), Quaternion.identity); // Far-right
                portal4 = GameObject.FindGameObjectWithTag("Room4" + "_" + i);

                /// Room 1 ---  This for loop links the Entrance and Exit of the portals. 
                for (int j = 0; j < portal.transform.childCount; j++)
                {
                    Transform child = portal.transform.GetChild(j);
                    if (child.tag == "Portal1") //Entrance
                    {
                        portalTemp = GameObject.FindGameObjectWithTag("PortalStart");
                        child.GetComponent<PortalTeleporter>().destination = portalTemp.transform.GetChild(j).transform;
                        portalTemp2 = portal2.transform.GetChild(j).transform;
                    }
                    else if (child.tag == "Portal2") //Exit
                    {
                        child.GetComponent<PortalTeleporter>().destination = portalTemp2;
                    }
                }
                /// Room 2 ---  This for loop links the Entrance and Exit of the portals. 
                for (int j = 0; j < portal2.transform.childCount; j++)
                {
                    Transform child = portal2.transform.GetChild(j);
                    if (child.tag == "Portal1") //Entrance
                    {
                        child.GetComponent<PortalTeleporter>().destination = portal.transform.GetChild(j).transform;
                        portalTemp2 = portal3.transform.GetChild(j).transform;
                    }
                    else if (child.tag == "Portal2") //Exit
                    {
                        child.GetComponent<PortalTeleporter>().destination = portalTemp2;
                    }
                }
                /// Room 3 ---  This for loop links the Entrance and Exit of the portals. 
                for (int j = 0; j < portal3.transform.childCount; j++)
                {
                    Transform child = portal3.transform.GetChild(j);
                    if (child.tag == "Portal1") //Entrance
                    {
                        child.GetComponent<PortalTeleporter>().destination = portal2.transform.GetChild(j).transform;
                        portalTemp2 = portal4.transform.GetChild(j).transform;
                    }
                    else if (child.tag == "Portal2") //Exit
                    {
                        //portal2.transform.GetChild(j)
                        child.GetComponent<PortalTeleporter>().destination = portalTemp2;
                    }
                }
                /// Room 4 ---  This for loop links the Entrance and Exit of the portals. 
                for (int j = 0; j < portal4.transform.childCount; j++)
                {
                    Transform child = portal4.transform.GetChild(j);
                    if (child.tag == "Portal1") //Entrance
                    {
                        child.GetComponent<PortalTeleporter>().destination = portal3.transform.GetChild(j).transform;
                        portalTemp2 = portalEnd.transform.GetChild(j).transform;
                    }
                    else if (child.tag == "Portal2") //Exit
                    {
                        child.GetComponent<PortalTeleporter>().destination = portalTemp2;
                    }
                }
                /// Start Room --- 
                for (int j = 0; j < portalStart.transform.childCount; j++)
                {
                    Transform child = portalStart.transform.GetChild(j);
                    if (child.tag == "Portal1") //Entrance
                    {
                    }
                    else if (child.tag == "Portal2") //Exit
                    {
                        portalTemp2 = portal.transform.GetChild(0).transform;
                        child.GetComponent<PortalTeleporter>().destination = portalTemp2;
                    }
                }
                /// End Room  ---  
                for (int j = 0; j < portalEnd.transform.childCount; j++)
                {
                    Transform child = portalEnd.transform.GetChild(j);
                    if (child.tag == "Portal1") //Entrance
                    {
                        child.GetComponent<PortalTeleporter>().destination = portal4.transform.GetChild(j).transform;
                        portalTemp2 = portalEnd.transform.GetChild(j).transform;
                    }
                }
            }
            else
            {
                /// Create start room
                TagCreate.AddTag("PortalStart");
                startRoom.tag = ("PortalStart");
                Instantiate(startRoom, new Vector3(0, 0, 0), Quaternion.identity);
                portalStart = GameObject.FindGameObjectWithTag("PortalStart");

                /// Create end room
                Instantiate(endRoom, new Vector3(0, 0, -roomDistance), Quaternion.identity);
                TagCreate.AddTag("PortalEnd");
                endRoom.tag = ("PortalEnd");
                portalEnd = GameObject.FindGameObjectWithTag("PortalEnd");
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
