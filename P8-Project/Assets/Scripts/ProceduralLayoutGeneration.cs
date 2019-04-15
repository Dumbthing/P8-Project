using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLayoutGeneration : MonoBehaviour
{
    /// Public variables, visible in the Inspector
    public string entryPortalTag = "EntryPortal";   // Public, in case it needs to be changed later
    public string exitPortalTag = "ExitPortal";
    public int maxRooms = 99;    // CURRENTLY OBSELETE, but can be used to limit the max amount of rooms later...
    public SetNextPortalPosition NextPortalPosUpdater;
    public SetPreviousPortalPosition PreviousPortalUpdater;

    /// Public variables, hidden from the Inspector. Use keyword [HideInInspector] before every variable!
    //[HideInInspector]
    public List<GameObject> layoutList; // Public, since other scripts need access to it
    [HideInInspector] public static GameObject[] startRooms, endRooms, rooms;
    [HideInInspector] public int currentRoom = 0;

    /// Private variables
    private int roomsUsed = 0;
    private int setNextLayer = 8;
    private int uniqueIterator;
    private float zeroF = 0.0f, ninetyF = 90.0f, oneEightyF = 180.0f, twoSeventyF = 270.0f;

    /* We use awake as it is called before start, and this must always be called exactly once.
     * Note that if we want to be able to call this function multiple times, like if we want to
     * restart without closing, it should be a Start() function instead, since start will run when
     * an object is set to active, while Awake will run when the object has been initialized by Unity.
     */
    private void Awake()
    {
        layoutList = new List<GameObject>();    // We choose to use a list since we don't know the final size of the layout
        LoadPrefabsToList();
        GenerateStartRoom(); // Randomly generate a starting room, by instantiating a room from the startRooms array
        GenerateLayout();
        GenerateEndRoom();
        NextPortalPosUpdater.UpdateActiveNextPortalPos();
    }

    private void LoadPrefabsToList()
    {
        startRooms = Resources.LoadAll<GameObject>("Start-Rooms");
        endRooms = Resources.LoadAll<GameObject>("End-Rooms");
        rooms = Resources.LoadAll<GameObject>("Rooms");
    }

    private void GenerateStartRoom() // No need to rotate start room
    {
        layoutList.Add(Instantiate(startRooms[Random.Range(0, startRooms.Length - 1)], new Vector3(0f, 0f, 0f), Quaternion.identity)); // Set a start room
        layoutList[0].layer = setNextLayer;
        Utils.ChangeLayersRecursively(layoutList[0].transform, setNextLayer);
    }

    private void GenerateLayout()
    {
        Utils.RandomizeArray(rooms);
        for (int i = 1; i < maxRooms; i++) // Iterate over layout
        {
            List<Transform> portalsInLastRoomList = Utils.GetPortalTransformsInRoom(layoutList[i - 1], exitPortalTag);
            for (int j = 0; j < rooms.Length; j++) // Iterate over rooms
            {
                /// This code supports dynamic rotation of rooms at a 90 degree angle, such that the portals line up with portals in the last room.
                /// If this is true, that room can be instantiated at the given rotation, and thereby connected with the previous room.
                List<Transform> portalsInNewRoomList = Utils.GetPortalTransformsInRoom(rooms[j], entryPortalTag, exitPortalTag);
                List<Vector3> ninetyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(rooms[j], entryPortalTag, exitPortalTag, ninetyF);
                List<Vector3> oneEightyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(rooms[j], entryPortalTag, exitPortalTag, oneEightyF);
                List<Vector3> twoSeventyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(rooms[j], entryPortalTag, exitPortalTag, twoSeventyF);
                int containedPortals = 0;
                float rotationParameter = zeroF;
                for (int k = 0; k < portalsInLastRoomList.Count; k++)
                {
                    for (int l = 0; l < portalsInNewRoomList.Count; l++)
                    {
                        if (portalsInLastRoomList[k].tag != portalsInNewRoomList[l].tag)
                        {
                            float newPortalRot = portalsInNewRoomList[l].eulerAngles.y;
                            float unrotatedRot = zeroF;
                            if (portalsInNewRoomList[l].eulerAngles.y == ninetyF)
                                unrotatedRot = ninetyF;
                            else if (portalsInNewRoomList[l].eulerAngles.y == oneEightyF)
                                unrotatedRot = oneEightyF;
                            else if (portalsInNewRoomList[l].eulerAngles.y == twoSeventyF)
                                unrotatedRot = twoSeventyF;

                            if (portalsInLastRoomList[k].position == portalsInNewRoomList[l].position &&
                                portalsInLastRoomList[k].eulerAngles.y != newPortalRot)          // Check for world position and y world rotation
                            {
                                containedPortals++;
                            }
                            else if (portalsInLastRoomList[k].position == ninetyDegPortalsInNewRoomList[l]) // Check for world position when rotated by 90 degrees
                            {
                                if (unrotatedRot != twoSeventyF) // unrotated rotation is 90 and rotation should therefore not be incremented by 90 when at 270
                                {
                                    if (portalsInLastRoomList[k].eulerAngles.y != newPortalRot + ninetyF )
                                    {
                                        containedPortals++;
                                        rotationParameter = ninetyF;
                                    }
                                }
                                else                                    // 90 + 270 = 360 -> 0
                                {
                                    if (portalsInLastRoomList[k].eulerAngles.y != zeroF) 
                                    {
                                        containedPortals++;
                                        rotationParameter = ninetyF;
                                    }
                                }
                            }
                            else if (portalsInLastRoomList[k].position == oneEightyDegPortalsInNewRoomList[l])  // Check for world position when rotated by 180 degrees
                            {
                                if (unrotatedRot != oneEightyF && unrotatedRot != twoSeventyF)
                                {
                                    if (portalsInLastRoomList[k].eulerAngles.y != newPortalRot + oneEightyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = oneEightyF;
                                    }
                                }
                                else if (unrotatedRot == oneEightyF)        // 180 + 180 = 360 -> 0
                                {
                                    if (portalsInLastRoomList[k].eulerAngles.y != zeroF)
                                    {
                                        containedPortals++;
                                        rotationParameter = oneEightyF;
                                    }
                                }
                                else if (unrotatedRot == twoSeventyF)       // 180 + 270 = 450 -> 90
                                {
                                    if (portalsInLastRoomList[k].eulerAngles.y != ninetyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = oneEightyF;
                                    }
                                }
                            }
                            else if (portalsInLastRoomList[k].position == twoSeventyDegPortalsInNewRoomList[l])  // Check for world position when rotated by 270 degrees
                            {
                                if (unrotatedRot == zeroF)
                                {
                                    if (portalsInLastRoomList[k].eulerAngles.y != newPortalRot + twoSeventyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                                else if (unrotatedRot == ninetyF)           // 270 + 90 = 360 -> 0
                                {
                                    if (portalsInLastRoomList[k].eulerAngles.y != zeroF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                                else if (unrotatedRot == oneEightyF)        // 270 + 180 = 450 -> 90
                                {
                                    if (portalsInLastRoomList[k].eulerAngles.y != ninetyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                                else if (unrotatedRot == twoSeventyF)       // 270 + 270 = 540 -> 180
                                {
                                    if (portalsInLastRoomList[k].eulerAngles.y != oneEightyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                            }
                        }
                    }
                }
                if (containedPortals == 1)
                {
                    setNextLayer++;
                    if (setNextLayer > 31)
                        setNextLayer = 8;
                    layoutList.Add(Instantiate(rooms[j], Utils.worldSpacePoint, Quaternion.Euler(0.0f, rotationParameter, 0.0f)));
                    layoutList[roomsUsed + 1].layer = setNextLayer;
                    Utils.ChangeLayersRecursively(layoutList[roomsUsed + 1].transform, setNextLayer);
                    Utils.SetActiveChild(layoutList[roomsUsed + 1].transform, false, entryPortalTag, exitPortalTag);
                    rooms = Utils.RemoveIndices(rooms, j);
                    roomsUsed++;
                    if (layoutList.Count > 2) // Only the first two rooms should be active on start
                    {
                        layoutList[i].SetActive(false);
                    }
                    break; // Breaks from the current for loop
                }
                if (j == rooms.Length - 1) // Last iteration
                {
                    return; // Breaks from both for loops since they are inside a method
                }
            }
        }
    }

    private void GenerateEndRoom()
    {
        setNextLayer++;
        List<Transform> portalsInLastRoomList = Utils.GetPortalTransformsInRoom(layoutList[roomsUsed], exitPortalTag); // Stores portal from previous room in a list
        Utils.RandomizeArray(endRooms);
        /// End room
        for (int i = 0; i < endRooms.Length; i++) // Iterate over rooms
        {
            List<Transform> portalsInEndRoomList = Utils.GetPortalTransformsInRoom(endRooms[i], entryPortalTag);
            List<Vector3> ninetyDegPortalsInEndRoomList = Utils.GetPortalPositionsInRoom(endRooms[i], entryPortalTag, 90.0f);
            List<Vector3> oneEightDegPortalsInEndRoomList = Utils.GetPortalPositionsInRoom(endRooms[i], entryPortalTag, 180.0f);
            List<Vector3> twoSeventyDegPortalsInEndRoomList = Utils.GetPortalPositionsInRoom(endRooms[i], entryPortalTag, 270.0f);
            float rotationParameter = 0;
            bool connectedPortal = false;
            
            /// Checks whether exactly 1 of the portals in the room has the same position as exactly 1 portal in the previous room in the layout.
            for (int j = 0; j < portalsInLastRoomList.Count; j++)
            {
                for (int k = 0; k < portalsInEndRoomList.Count; k++)
                {
                    if (portalsInLastRoomList[j].eulerAngles != portalsInEndRoomList[k].eulerAngles &&  // Check for rotation
                    portalsInLastRoomList[j].tag != portalsInEndRoomList[k].tag)                        // Check for tag
                    {
                        if (portalsInLastRoomList[j].position == portalsInEndRoomList[k].position)      // Check for world position
                        {
                            connectedPortal = true;
                        }
                        else if (portalsInLastRoomList[j].position == ninetyDegPortalsInEndRoomList[k])
                        {
                            rotationParameter = 90.0f;
                            connectedPortal = true;
                        }
                        else if (portalsInLastRoomList[j].position == oneEightDegPortalsInEndRoomList[k])
                        {
                            rotationParameter = 180.0f;
                            connectedPortal = true;
                        }
                        else if (portalsInLastRoomList[j].position == twoSeventyDegPortalsInEndRoomList[k])
                        {
                            rotationParameter = 270.0f;
                            connectedPortal = true;
                        }
                    }
                    if (connectedPortal)
                    {
                        layoutList.Add(Instantiate(endRooms[i], Utils.worldSpacePoint, Quaternion.Euler(0.0f, rotationParameter, 0.0f)));
                        layoutList[roomsUsed + 1].layer = setNextLayer;
                        Utils.SetActiveChild(layoutList[roomsUsed + 1].transform, false, entryPortalTag, exitPortalTag);
                        Utils.ChangeLayersRecursively(layoutList[roomsUsed + 1].transform, setNextLayer);
                        layoutList[roomsUsed + 1].SetActive(false);
                        return; // Breaks from the function
                    }
                }
            }
        }
    }
}
