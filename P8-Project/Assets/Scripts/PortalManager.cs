using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    /// Inspector variables
    public GameObject[] startRooms, endRooms, rooms;
    public string portalTag = "Portal";

    /// Public, non-inspector variables
    [HideInInspector]
    public GameObject[] layout;
    public int maxRooms;

    /// Referencial variables to components of current gameobject
    CameraEnabler cameras;

    /// Comparison variables
    private bool singlePortalCollision = false, playerReturned = false;
    private int portalExitScenario = 0; // Default is 0: do nothing
    private int currentRoom = 0;
    private Vector3 backwardPortalPos, lastPortalPos;


    void Start()
    {
        cameras = GetComponent<CameraEnabler>(); // Script that handles which layer is rendered by which camera
        maxRooms = (rooms.Length - 1); // Should probably be a public variable set by player
        layout = new GameObject[maxRooms + 1];
        GenerateLevel(); // Method to generate level from the prefab rooms
    }

    private void GenerateLevel()
    {
        layout[0] = Instantiate(startRooms[Random.Range(0, startRooms.Length - 1)], new Vector3(0f, 0f, 0f), Quaternion.identity); // Set a start room
        for (int i = 1; i < maxRooms + 1; i++) // Iterate over layout
        {
            List<Vector3> portalPositionsInLastRoomList = GetPortalPositionInRoom(layout[i]);
            List<Vector3> debugPortalPositionsInNewRoomList = new List<Vector3>();
            Vector3[] portalPosition = new Vector3[portalPositionsInLastRoomList.Count];
            int increment = 0;
            foreach (Vector3 portalPos in portalPositionsInLastRoomList)
            {
                portalPosition[increment] = portalPos;
                increment++;
            }
            for (int j = 0; j < rooms.Length; j++) // Iterate over rooms
            {
                List<Vector3> portalPositionsInNewRoomList = GetPortalPositionInRoom(rooms[j]);
                int containedPortals = 0;
                /// Checks whether exactly 1 of the portals in the room has the same position as exactly 1 portal in the previous room in the layout.
                for (int k = 0; k < portalPositionsInLastRoomList.Count; k++)
                {
                    if (portalPositionsInNewRoomList.Contains(portalPosition[k])) // Might need an additional checker for more than 1 equal portal pos
                    {
                        containedPortals++;
                    }
                }
                if (containedPortals == 1)
                {
                    layout[i] = Instantiate(rooms[j], new Vector3(0f, 0f, 0f), Quaternion.identity);
                    SetActiveChild(layout[i].transform, "Portal", false);
                    RemoveIndices(rooms,j);
                    break;
                }
                //debugPortalPositionsInNewRoomList.Add
            }

            /// DEBUGGING
            Debug.Log("Portal 1 position in last layout room: " + portalPositionsInLastRoomList[0]);
            Debug.Log("Portal 2 position in last layout room: " + portalPositionsInLastRoomList[1]);

            //for (int j = 0; j < )
        }
        layout[layout.Length+1] = Instantiate(endRooms[Random.Range(0, endRooms.Length - 1)], new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    private void OnTriggerEnter(Collider portal) // Portal collision
    {
        if (!singlePortalCollision) // Avoid multiple OnTriggerEnter calls from same collider
        {
            if (portal.transform.localPosition != backwardPortalPos && currentRoom < maxRooms)
            {
                currentRoom++;
                portalExitScenario = 1;

            }
            else if (portal.transform.localPosition == backwardPortalPos && currentRoom != 0)
            {
                currentRoom--;
                portalExitScenario = 2;
            }
            cameras.SetNewCullingMasks(currentRoom);
            lastPortalPos = portal.transform.localPosition; // Store position of last portal
            singlePortalCollision = true;
        }
    }

    private void OnTriggerExit(Collider portal) // Out of portal
    {
        PortalScenario(portalExitScenario, portal);
        singlePortalCollision = false;
    }

    private void PortalScenario(int scenario, Collider portal)
    {
        if (!playerReturned)
        {
            SetActiveChild(layout[currentRoom].transform, "Portal", true); // Enable portals in new room, in case they are disabled.

            if (portalExitScenario == 1) // Scenario 1: Enter "next-room" portal
            {
                SetActiveChild(layout[currentRoom - 1].transform, "Portal", false); // Since we enabled new portals, we should disable the existing ones.
            }
            else if (portalExitScenario == 2) // Scenario 2: Enter "previous-room" portal
            {
                SetActiveChild(layout[currentRoom + 1].transform, "Portal", false); // Since we enabled new portals, we should disable the existing ones.
            }
        }
        else // Should occur if the player enters and exits collider on the same side
        {
            if (portalExitScenario == 1)
            {
                SetActiveChild(layout[currentRoom + 1].transform, "Portal", false);
            }
            else if (portalExitScenario == 2)
            {
                SetActiveChild(layout[currentRoom - 1].transform, "Portal", false);
            }
            SetActiveChild(layout[currentRoom].transform, "Portal", true);
            playerReturned = false;
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

            i++;
        }

        return newIndicesArray;
    }
}

