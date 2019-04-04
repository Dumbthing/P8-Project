using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    /// Inspector variables
    public GameObject[] startRooms, endRooms, rooms;
    public string portalTag = "Portal";
    public int maxRooms;

    /// Public, non-inspector variables
    [HideInInspector]
    public GameObject[] layout;

    /// Referencial variables to components of current gameobject
    CameraEnabler cameras;
    GameObject[] roomsSaved;

    /// Comparison variables
    private bool singlePortalCollision = false, playerReturned = false;
    private int portalExitScenario = 0; // Default is 0: do nothing
    private int currentRoom = 0;
    private Vector3 backwardPortalPos, lastPortalPos, lastExitPortal;
    private int roomsUsed = 0;
    private int nextLayer = 8;

    void Start()
    {
        roomsSaved = rooms;
        cameras = GetComponent<CameraEnabler>(); // Script that handles which layer is rendered by which camera
        layout = new GameObject[maxRooms + 1]; // Start- and End-rooms are always included
        GenerateLevel(); // Method to generate level from the prefab rooms
    }

    private void GenerateLevel()
    {
        layout[0] = Instantiate(startRooms[Random.Range(0, startRooms.Length - 1)], new Vector3(0f, 0f, 0f), Quaternion.identity); // Set a start room
        layout[0].layer = nextLayer;
        ChangeLayersRecursively(layout[0].transform, nextLayer);
        lastExitPortal = GetARandomPortalPositionInRoom(layout[0]);

        GenerateRooms();
        SelectEndRoom();
    }

    private void GenerateRooms()
    {
        RandomizeArray(rooms);
        for (int i = 1; i < maxRooms; i++) // Iterate over layout
        {
            nextLayer++;
            //List<Vector3> portalPositionsInLastRoomList = GetPortalPositionInRoom(layout[i - 1]);
            List<Transform> portalTransformsInLastRoomList = GetPortalTransformsInRoom(layout[i - 1]);
            for (int j = 0; j < rooms.Length - roomsUsed; j++) // Iterate over rooms
            {
                List<Vector3> portalPositionsInNewRoomList = GetPortalPositionInRoom(rooms[j]);
                List<Quaternion> portalRotationsInNewRoomList = GetPortalRotationsInRoom(rooms[j]);
                int containedPortals = 0;
                /// Checks whether exactly 1 of the portals in the room has the same position as exactly 1 portal in the previous room in the layout.
                //for (int k = 0; k < portalPositionsInLastRoomList.Count; k++)
                //{
                //    if (portalPositionsInNewRoomList.Contains(portalPositionsInLastRoomList[k]))
                //    {
                //        containedPortals++;
                //    }
                //}
                for (int k = 0; k < portalTransformsInLastRoomList.Count; k++)
                {
                    if (portalPositionsInNewRoomList.Contains(portalTransformsInLastRoomList[k].localPosition) &&
                        portalRotationsInNewRoomList.Contains(portalTransformsInLastRoomList[k].localRotation))
                    {
                        containedPortals++;
                    }
                }
                if (containedPortals == 1)
                {
                    layout[i] = Instantiate(rooms[j], new Vector3(0f, 0f, 0f), Quaternion.identity);
                    layout[i].layer = nextLayer;
                    ChangeLayersRecursively(layout[i].transform, nextLayer);
                    SetActiveChild(layout[i].transform, "Portal", false);
                    rooms = RemoveIndices(rooms, j);
                    roomsUsed++;
                    break; // Breaks from the current for loop
                }
                if (j == rooms.Length - roomsUsed - 1) // Last iteration
                {
                    return; // Breaks from both for loops since they are inside a method
                }
            }
        }
    }

    private void SelectEndRoom()
    {
        nextLayer++;
        List<Transform> portalTransformsRoomBeforeEnd = GetPortalTransformsInRoom(layout[roomsUsed]); // Stores portal from previous room in a list
        RandomizeArray(endRooms);
        /// End room
        for (int i = 0; i < endRooms.Length; i++) // Iterate over rooms
        {
            List<Vector3> portalPositionsInNewRoomList = GetPortalPositionInRoom(endRooms[i]);
            List<Quaternion> portalRotationsInNewRoomList = GetPortalRotationsInRoom(endRooms[i]);
            /// Checks whether exactly 1 of the portals in the room has the same position as exactly 1 portal in the previous room in the layout.
            for (int j = 0; j < portalTransformsRoomBeforeEnd.Count; j++)
            {
                if (portalPositionsInNewRoomList.Contains(portalTransformsRoomBeforeEnd[j].localPosition) &&
                        portalRotationsInNewRoomList.Contains(portalTransformsRoomBeforeEnd[j].localRotation))
                {
                    layout[roomsUsed + 1] = Instantiate(endRooms[i], new Vector3(0f, 0f, 0f), Quaternion.identity);
                    layout[roomsUsed + 1].layer = nextLayer;
                    SetActiveChild(layout[roomsUsed + 1].transform, "Portal", false);
                    ChangeLayersRecursively(layout[roomsUsed + 1].transform, nextLayer);
                    return; // Breaks from the nested for loop
                }
            }
        }
    }

    // Access other portals
    public void SetActiveChild(Transform parent, string _tag, bool enabled)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
                child.gameObject.SetActive(enabled);
        }
    }

    public void SetSiblingPortalActive(Transform parent, string _tag, bool enabled)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag && child.transform.localPosition != lastPortalPos) // Not sure if this should be lastPortalPos
            {
                child.gameObject.SetActive(enabled);
            }
        }
    }

    private List<Transform> GetPortalTransformsInRoom(GameObject room)
    {
        List<Transform> portalPositions = new List<Transform>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == portalTag)
            {
                portalPositions.Add(child);
            }
        }
        return portalPositions;
    }

    private List<Vector3> GetPortalPositionInRoom(GameObject room)
    {
        List<Vector3> portalPositions = new List<Vector3>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == portalTag)
            {
                portalPositions.Add(child.localPosition);
            }
        }
        return portalPositions;
    }

    private List<Quaternion> GetPortalRotationsInRoom(GameObject room)
    {
        List<Quaternion> portalRotations = new List<Quaternion>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == portalTag)
            {
                portalRotations.Add(child.localRotation);
            }
        }
        return portalRotations;
    }

    private Vector3 GetARandomPortalPositionInRoom(GameObject room)
    {
        List<Vector3> newExitPortals = new List<Vector3>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == portalTag && child.localPosition != lastExitPortal)
            {
                newExitPortals.Add(child.localPosition);
            }
        }
        int r = Random.Range(0, newExitPortals.Count - 1);
        if (r < 0)
            Debug.Log("No portals found in request room");
        return newExitPortals[r];
    }

    private GameObject[] RemoveIndices(GameObject[] IndicesArray, int RemoveAt)
    {
        GameObject[] newIndicesArray = new GameObject[IndicesArray.Length - 1];

        int i = 0;
        int j = 0;
        while (i < IndicesArray.Length)
        {
            if (i != RemoveAt)
            {
                newIndicesArray[j] = IndicesArray[i];
                j++;
            }
            else
            {
                Debug.Log("Removed element " + i + " from array");
            }
            i++;
        }
        return newIndicesArray;
    }

    private void ChangeLayersRecursively(Transform trans, int newLayer)
    {
        foreach (Transform child in trans)
        {
            child.gameObject.layer = newLayer;
            ChangeLayersRecursively(child, newLayer);
        }
    }

    void RandomizeArray(GameObject[] arr) // Fischer-Yates shuffle
    {
        for (int i = 0; i < arr.Length; i++)
        {
            int r = Random.Range(0, arr.Length - 1);
            GameObject temp = arr[i];
            arr[i] = arr[r];
            arr[r] = temp;
        }
    }
}