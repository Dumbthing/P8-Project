﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLayoutGeneration : MonoBehaviour
{
    /// Public variables, visible in the Inspector
    public string entryPortalTag = "EntryPortal";   // Public, in case it needs to be changed later
    public string exitPortalTag = "ExitPortal";
    public int maxRooms = 99;    // CURRENTLY OBSELETE, but can be used to limit the max amount of fantasyRooms later...
    public SetNextPortalPosition NextPortalPosUpdater;
    public SetPreviousPortalPosition PreviousPortalUpdater;

    /// Public variables, hidden from the Inspector. Use keyword [HideInInspector] before every variable!
    //[HideInInspector]
    public List<GameObject> layoutList; // Public, since other scripts need access to it
    [HideInInspector] public static GameObject[] startRooms, endRooms, fantasyRooms, transitionRooms, scifiRooms;
    [HideInInspector] public int currentRoom = 0;

    /// Private variables
    private int roomsUsed = 0;
    private int setNextLayer = 8;
    private int uniqueIterator;
    private float zeroF = 0.0f, ninetyF = 90.0f, oneEightyF = 180.0f, twoSeventyF = 270.0f;
    private float tolerance = 0.001f;

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
        GenerateFantasyRooms();
        GenerateTransitionRoom();
        GenerateSciFiRooms();
        GenerateEndRoom();
    }

    private void LoadPrefabsToList()
    {
        startRooms = Resources.LoadAll<GameObject>("Start-Rooms");
        endRooms = Resources.LoadAll<GameObject>("End-Rooms");
        fantasyRooms = Resources.LoadAll<GameObject>("Fantasy-Rooms");
        transitionRooms = Resources.LoadAll<GameObject>("Theme-Transition-Rooms");
        scifiRooms = Resources.LoadAll<GameObject>("Sci-Fi-Rooms");
    }

    private void GenerateStartRoom() // No need to rotate start room
    {
        layoutList.Add(Instantiate(startRooms[Random.Range(0, startRooms.Length - 1)], Utils.worldSpacePoint, Quaternion.identity)); // Set a start room
        //layoutList[0].layer = setNextLayer;
        //Utils.ChangeLayersRecursively(layoutList[0].transform, setNextLayer);
    }

    private void GenerateFantasyRooms()
    {
        if (maxRooms < 3) // In case inspector value is too low to include static rooms
            maxRooms = 3;
        Utils.RandomizeArray(fantasyRooms);
        for (int i = 1; i <= (maxRooms - 3) / 2; i++) // Iterate over layout, at max rooms minus static rooms (start, end, transition) divided by 2 themes.
        {
            List<Transform> portalsInLastRoomList = Utils.GetPortalTransformsInRoom(layoutList[i - 1], exitPortalTag);
            for (int j = 0; j < portalsInLastRoomList.Count; j++)
            {
                portalsInLastRoomList[j].position = new Vector3(Mathf.Round(portalsInLastRoomList[j].position.x * 100.0f) / 100.0f, portalsInLastRoomList[j].position.y,
                    Mathf.Round(portalsInLastRoomList[j].position.z * 100.0f) / 100.0f); // Avoid floating-point comparison errors when rotating parent
            }
            for (int j = 0; j < fantasyRooms.Length; j++) // Iterate over fantasyRooms
            {
                if (fantasyRooms[j].name.Contains("View") && i > maxRooms / 2 - 4)
                {
                    continue;   // Go to next step in for loop to avoid placing a view room when we are about to switch to a new skybox
                }
                /// This code supports dynamic rotation of fantasyRooms at a 90 degree angle, such that the portals line up with portals in the last room.
                /// If this is true, that room can be instantiated at the given rotation, and thereby connected with the previous room.
                List<Transform> portalsInNewRoomList = Utils.GetPortalTransformsInRoom(fantasyRooms[j], entryPortalTag);
                List<Vector3> ninetyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(fantasyRooms[j], entryPortalTag, ninetyF);
                List<Vector3> oneEightyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(fantasyRooms[j], entryPortalTag, oneEightyF);
                List<Vector3> twoSeventyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(fantasyRooms[j], entryPortalTag, twoSeventyF);
                int containedPortals = 0;
                float rotationParameter = zeroF;
                for (int k = 0; k < portalsInLastRoomList.Count; k++)
                {
                    for (int l = 0; l < portalsInNewRoomList.Count; l++)
                    {
                        if (portalsInLastRoomList[k].tag != portalsInNewRoomList[l].tag)
                        {
                            float newPortalRot = Mathf.Round(portalsInNewRoomList[l].eulerAngles.y * 100) / 100;
                            float unrotatedRot = zeroF;
                            if (Mathf.Round(portalsInNewRoomList[l].eulerAngles.y * 100) / 100 == ninetyF)
                                unrotatedRot = ninetyF;
                            else if (Mathf.Round(portalsInNewRoomList[l].eulerAngles.y * 100) / 100 == oneEightyF)
                                unrotatedRot = oneEightyF;
                            else if (Mathf.Round(portalsInNewRoomList[l].eulerAngles.y * 100) / 100 == twoSeventyF)
                                unrotatedRot = twoSeventyF;

                            if (Utils.VectorApproxComparison(portalsInLastRoomList[k].position, portalsInNewRoomList[l].position) &&
                                Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != newPortalRot)          // Check for world position and y world rotation
                            {
                                containedPortals++;
                            }
                            else if (Utils.VectorApproxComparison(portalsInLastRoomList[k].position, ninetyDegPortalsInNewRoomList[l])) // Check for world position when rotated by 90 degrees
                            {
                                if (unrotatedRot != twoSeventyF) // unrotated rotation is 90 and rotation should therefore not be incremented by 90 when at 270
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != newPortalRot + ninetyF )
                                    {
                                        containedPortals++;
                                        rotationParameter = ninetyF;
                                    }
                                }
                                else                                    // 90 + 270 = 360 -> 0
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != zeroF) 
                                    {
                                        containedPortals++;
                                        rotationParameter = ninetyF;
                                    }
                                }
                            }
                            else if (Utils.VectorApproxComparison(portalsInLastRoomList[k].position, oneEightyDegPortalsInNewRoomList[l]))  // Check for world position when rotated by 180 degrees
                            {
                                if (unrotatedRot != oneEightyF && unrotatedRot != twoSeventyF)
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != newPortalRot + oneEightyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = oneEightyF;
                                    }
                                }
                                else if (unrotatedRot == oneEightyF)        // 180 + 180 = 360 -> 0
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != zeroF)
                                    {
                                        containedPortals++;
                                        rotationParameter = oneEightyF;
                                    }
                                }
                                else if (unrotatedRot == twoSeventyF)       // 180 + 270 = 450 -> 90
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != ninetyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = oneEightyF;
                                    }
                                }
                            }
                            else if (Utils.VectorApproxComparison(portalsInLastRoomList[k].position, twoSeventyDegPortalsInNewRoomList[l]))  // Check for world position when rotated by 270 degrees
                            {
                                if (unrotatedRot == zeroF)
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != newPortalRot + twoSeventyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                                else if (unrotatedRot == ninetyF)           // 270 + 90 = 360 -> 0
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != zeroF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                                else if (unrotatedRot == oneEightyF)        // 270 + 180 = 450 -> 90
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != ninetyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                                else if (unrotatedRot == twoSeventyF)       // 270 + 270 = 540 -> 180
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != oneEightyF)
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
                    //setNextLayer++;
                    //if (setNextLayer > 31)
                    //    setNextLayer = 8;
                    layoutList.Add(Instantiate(fantasyRooms[j], Utils.worldSpacePoint, Quaternion.Euler(0.0f, rotationParameter, 0.0f)));
                    roomsUsed++;
                    //layoutList[roomsUsed].layer = setNextLayer;
                    //Utils.ChangeLayersRecursively(layoutList[roomsUsed].transform, setNextLayer);
                    Utils.SetActiveChild(layoutList[roomsUsed].transform, false, entryPortalTag, exitPortalTag);
                    fantasyRooms = Utils.RemoveIndices(fantasyRooms, j);
                    if (layoutList.Count > 2) // Only the first two rooms should be active on start
                    {
                        layoutList[i].SetActive(false);
                    }
                    break; // Breaks from the current for loop
                }
                if (j == fantasyRooms.Length - 1) // Last iteration
                {
                    Debug.Log("Ran out of Fantasy rooms");
                    return; // Breaks from both for loops since they are inside a method
                }
            }
        }
    }

    private void GenerateTransitionRoom()
    {
        List<Transform> portalsInLastRoomList = Utils.GetPortalTransformsInRoom(layoutList[roomsUsed], exitPortalTag); // Stores portal from previous room in a list
        for (int j = 0; j < portalsInLastRoomList.Count; j++)
        {
            portalsInLastRoomList[j].position = new Vector3(Mathf.Round(portalsInLastRoomList[j].position.x * 100.0f) / 100.0f, portalsInLastRoomList[j].position.y,
                Mathf.Round(portalsInLastRoomList[j].position.z * 100.0f) / 100.0f); // Avoid floating-point comparison errors when rotating parent
        }
        for (int i = 0; i < transitionRooms.Length; i++) // Iterate over Theme-transition rooms
        {
            List<Transform> portalsInNewRoomList = Utils.GetPortalTransformsInRoom(transitionRooms[i], entryPortalTag);
            List<Vector3> ninetyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(transitionRooms[i], entryPortalTag, ninetyF);
            List<Vector3> oneEightyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(transitionRooms[i], entryPortalTag, oneEightyF);
            List<Vector3> twoSeventyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(transitionRooms[i], entryPortalTag, twoSeventyF);
            float rotationParameter = 0;
            bool connectedPortal = false;

            /// Checks whether exactly 1 of the portals in the room has the same position as exactly 1 portal in the previous room in the layout.
            for (int j = 0; j < portalsInLastRoomList.Count; j++)
            {
                for (int k = 0; k < portalsInNewRoomList.Count; k++)
                {
                    if (portalsInLastRoomList[j].tag != portalsInNewRoomList[k].tag)                        // Check for tag
                    {
                        float newPortalRot = Mathf.Round(portalsInNewRoomList[k].eulerAngles.y * 100) / 100;
                        float unrotatedRot = zeroF;
                        if (Mathf.Round(portalsInNewRoomList[k].eulerAngles.y * 100) / 100 == ninetyF)
                            unrotatedRot = ninetyF;
                        else if (Mathf.Round(portalsInNewRoomList[k].eulerAngles.y * 100) / 100 == oneEightyF)
                            unrotatedRot = oneEightyF;
                        else if (Mathf.Round(portalsInNewRoomList[k].eulerAngles.y * 100) / 100 == twoSeventyF)
                            unrotatedRot = twoSeventyF;

                        if (Utils.VectorApproxComparison(portalsInLastRoomList[j].position, portalsInNewRoomList[k].position) &&
                            Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != newPortalRot)          // Check for world position and y world rotation
                        {
                            connectedPortal = true;
                        }
                        else if (Utils.VectorApproxComparison(portalsInLastRoomList[j].position, ninetyDegPortalsInNewRoomList[k])) // Check for world position when rotated by 90 degrees
                        {
                            if (unrotatedRot != twoSeventyF) // unrotated rotation is 90 and rotation should therefore not be incremented by 90 when at 270
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != newPortalRot + ninetyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = ninetyF;
                                }
                            }
                            else                                    // 90 + 270 = 360 -> 0
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != zeroF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = ninetyF;
                                }
                            }
                        }
                        else if (Utils.VectorApproxComparison(portalsInLastRoomList[j].position, oneEightyDegPortalsInNewRoomList[k]))  // Check for world position when rotated by 180 degrees
                        {
                            if (unrotatedRot != oneEightyF && unrotatedRot != twoSeventyF)
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != newPortalRot + oneEightyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = oneEightyF;
                                }
                            }
                            else if (unrotatedRot == oneEightyF)        // 180 + 180 = 360 -> 0
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != zeroF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = oneEightyF;
                                }
                            }
                            else if (unrotatedRot == twoSeventyF)       // 180 + 270 = 450 -> 90
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != ninetyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = oneEightyF;
                                }
                            }
                        }
                        else if (Utils.VectorApproxComparison(portalsInLastRoomList[j].position, twoSeventyDegPortalsInNewRoomList[k]))  // Check for world position when rotated by 270 degrees
                        {
                            if (unrotatedRot == zeroF)
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != newPortalRot + twoSeventyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = twoSeventyF;
                                }
                            }
                            else if (unrotatedRot == ninetyF)           // 270 + 90 = 360 -> 0
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != zeroF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = twoSeventyF;
                                }
                            }
                            else if (unrotatedRot == oneEightyF)        // 270 + 180 = 450 -> 90
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != ninetyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = twoSeventyF;
                                }
                            }
                            else if (unrotatedRot == twoSeventyF)       // 270 + 270 = 540 -> 180
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != oneEightyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = twoSeventyF;
                                }
                            }
                        }
                        if (connectedPortal)
                        {
                            layoutList.Add(Instantiate(transitionRooms[i], Utils.worldSpacePoint, Quaternion.Euler(0.0f, rotationParameter, 0.0f)));
                            roomsUsed++;
                            Utils.SetActiveChild(layoutList[roomsUsed].transform, false, entryPortalTag, exitPortalTag);
                            layoutList[roomsUsed].SetActive(false);
                            return; // Breaks from the function
                        }
                    }
                }
            }
        }
    }

    private void GenerateSciFiRooms()
    {
        Utils.RandomizeArray(scifiRooms);
        for (int i = layoutList.Count; i < maxRooms-1; i++) // Iterate over layout, going from current count to maxRooms - 1 end room
        {
            List<Transform> portalsInLastRoomList = Utils.GetPortalTransformsInRoom(layoutList[i - 1], exitPortalTag);
            for (int j = 0; j < portalsInLastRoomList.Count; j++)
            {
                portalsInLastRoomList[j].position = new Vector3(Mathf.Round(portalsInLastRoomList[j].position.x * 100.0f) / 100.0f, portalsInLastRoomList[j].position.y,
                    Mathf.Round(portalsInLastRoomList[j].position.z * 100.0f) / 100.0f); // Avoid floating-point comparison errors when rotating parent
                Debug.Log("Portal in last room rotation: " + portalsInLastRoomList[j].eulerAngles.y);
            }
            for (int j = 0; j < scifiRooms.Length; j++) // Iterate over scifiRooms
            {
                /// This code supports dynamic rotation of scifiRooms at a 90 degree angle, such that the portals line up with portals in the last room.
                /// If this is true, that room can be instantiated at the given rotation, and thereby connected with the previous room.
                List<Transform> portalsInNewRoomList = Utils.GetPortalTransformsInRoom(scifiRooms[j], entryPortalTag, exitPortalTag);
                List<Vector3> ninetyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(scifiRooms[j], entryPortalTag, exitPortalTag, ninetyF);
                List<Vector3> oneEightyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(scifiRooms[j], entryPortalTag, exitPortalTag, oneEightyF);
                List<Vector3> twoSeventyDegPortalsInNewRoomList = Utils.GetPortalPositionsInRoom(scifiRooms[j], entryPortalTag, exitPortalTag, twoSeventyF);
                int containedPortals = 0;
                float rotationParameter = zeroF;
                for (int k = 0; k < portalsInLastRoomList.Count; k++)
                {
                    for (int l = 0; l < portalsInNewRoomList.Count; l++)
                    {
                        if (portalsInLastRoomList[k].tag != portalsInNewRoomList[l].tag)
                        {
                            float newPortalRot = Mathf.Round(portalsInNewRoomList[l].eulerAngles.y * 100) / 100;
                            float unrotatedRot = zeroF;
                            if (Mathf.Round(portalsInNewRoomList[l].eulerAngles.y * 100) / 100 == ninetyF)
                                unrotatedRot = ninetyF;
                            else if (Mathf.Round(portalsInNewRoomList[l].eulerAngles.y * 100) / 100 == oneEightyF)
                                unrotatedRot = oneEightyF;
                            else if (Mathf.Round(portalsInNewRoomList[l].eulerAngles.y * 100) / 100 == twoSeventyF)
                                unrotatedRot = twoSeventyF;

                            if (Utils.VectorApproxComparison(portalsInLastRoomList[k].position, portalsInNewRoomList[l].position) &&
                              Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != newPortalRot)          // Check for world position and y world rotation
                            {
                                containedPortals++;
                            }
                            else if (Utils.VectorApproxComparison(portalsInLastRoomList[k].position, ninetyDegPortalsInNewRoomList[l])) // Check for world position when rotated by 90 degrees
                            {
                                if (unrotatedRot != twoSeventyF) // unrotated rotation is 90 and rotation should therefore not be incremented by 90 when at 270
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != newPortalRot + ninetyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = ninetyF;
                                    }
                                }
                                else                                    // 90 + 270 = 360 -> 0
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != zeroF)
                                    {
                                        containedPortals++;
                                        rotationParameter = ninetyF;
                                    }
                                }
                            }
                            else if (Utils.VectorApproxComparison(portalsInLastRoomList[k].position, oneEightyDegPortalsInNewRoomList[l]))  // Check for world position when rotated by 180 degrees
                            {
                                if (unrotatedRot != oneEightyF && unrotatedRot != twoSeventyF)
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != newPortalRot + oneEightyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = oneEightyF;
                                    }
                                }
                                else if (unrotatedRot == oneEightyF)        // 180 + 180 = 360 -> 0
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != zeroF)
                                    {
                                        containedPortals++;
                                        rotationParameter = oneEightyF;
                                    }
                                }
                                else if (unrotatedRot == twoSeventyF)       // 180 + 270 = 450 -> 90
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != ninetyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = oneEightyF;
                                    }
                                }
                            }
                            else if (Utils.VectorApproxComparison(portalsInLastRoomList[k].position, twoSeventyDegPortalsInNewRoomList[l]))  // Check for world position when rotated by 270 degrees
                            {
                                if (unrotatedRot == zeroF)
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != newPortalRot + twoSeventyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                                else if (unrotatedRot == ninetyF)           // 270 + 90 = 360 -> 0
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != zeroF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                                else if (unrotatedRot == oneEightyF)        // 270 + 180 = 450 -> 90
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != ninetyF)
                                    {
                                        containedPortals++;
                                        rotationParameter = twoSeventyF;
                                    }
                                }
                                else if (unrotatedRot == twoSeventyF)       // 270 + 270 = 540 -> 180
                                {
                                    if (Mathf.Round(portalsInLastRoomList[k].eulerAngles.y * 100) / 100 != oneEightyF)
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
                    layoutList.Add(Instantiate(scifiRooms[j], Utils.worldSpacePoint, Quaternion.Euler(0.0f, rotationParameter, 0.0f)));
                    roomsUsed++;
                    Utils.SetActiveChild(layoutList[roomsUsed].transform, false, entryPortalTag, exitPortalTag);
                    scifiRooms = Utils.RemoveIndices(scifiRooms, j);
                    layoutList[i].SetActive(false);
                    break; // Breaks from the current for loop
                }
                if (j == scifiRooms.Length - 1) // Last iteration
                {
                    Debug.Log("Ran out of Sci-Fi rooms");
                    return; // Breaks from both for loops since they are inside a method
                }
            }
        }
    }

    private void GenerateEndRoom()
    {
        List<Transform> portalsInLastRoomList = Utils.GetPortalTransformsInRoom(layoutList[roomsUsed], exitPortalTag); // Stores portal from previous room in a list
        for (int i = 0; i < portalsInLastRoomList.Count; i++)
        {
            portalsInLastRoomList[i].position = new Vector3(Mathf.Round(portalsInLastRoomList[i].position.x * 100.0f) / 100.0f, portalsInLastRoomList[i].position.y,
                Mathf.Round(portalsInLastRoomList[i].position.z * 100.0f) / 100.0f); // Avoid floating-point comparison errors when rotating parent
            if (portalsInLastRoomList[i].eulerAngles.y >= 360.0f)
            {
                Debug.Log("Before set: " + portalsInLastRoomList[i].eulerAngles.y);
                //portalsInLastRoomList[i].eulerAngles.Set(portalsInLastRoomList[i].eulerAngles.x, 0.0f, portalsInLastRoomList[i].eulerAngles.z);
                Debug.Log("After set" + portalsInLastRoomList[i].eulerAngles.y);
            }
        }
        Utils.RandomizeArray(endRooms);
        for (int i = 0; i < endRooms.Length; i++) // Iterate over End rooms
        {
            List<Transform> portalsInEndRoomList = Utils.GetPortalTransformsInRoom(endRooms[i], entryPortalTag);
            List<Vector3> ninetyDegPortalsInEndRoomList = Utils.GetPortalPositionsInRoom(endRooms[i], entryPortalTag, ninetyF);
            List<Vector3> oneEightyDegPortalsInEndRoomList = Utils.GetPortalPositionsInRoom(endRooms[i], entryPortalTag, oneEightyF);
            List<Vector3> twoSeventyDegPortalsInEndRoomList = Utils.GetPortalPositionsInRoom(endRooms[i], entryPortalTag, twoSeventyF);
            float rotationParameter = 0;
            bool connectedPortal = false;

            /// Checks whether exactly 1 of the portals in the room has the same position as exactly 1 portal in the previous room in the layout.
            for (int j = 0; j < portalsInLastRoomList.Count; j++)
            {
                for (int k = 0; k < portalsInEndRoomList.Count; k++)
                {
                    if (portalsInLastRoomList[j].tag != portalsInEndRoomList[k].tag)                        // Check for tag
                    {
                        float newPortalRot = Mathf.Round(portalsInEndRoomList[k].eulerAngles.y * 100) / 100;
                        float unrotatedRot = zeroF;
                        if (Mathf.Round(portalsInEndRoomList[k].eulerAngles.y * 100) / 100 == ninetyF)
                            unrotatedRot = ninetyF;
                        else if (Mathf.Round(portalsInEndRoomList[k].eulerAngles.y * 100) / 100 == oneEightyF)
                            unrotatedRot = oneEightyF;
                        else if (Mathf.Round(portalsInEndRoomList[k].eulerAngles.y * 100) / 100 == twoSeventyF)
                            unrotatedRot = twoSeventyF;

                        if (Utils.VectorApproxComparison(portalsInLastRoomList[j].position, portalsInEndRoomList[k].position) &&
                            Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != newPortalRot)          // Check for world position and y world rotation
                        {
                            connectedPortal = true;
                        }
                        else if (Utils.VectorApproxComparison(portalsInLastRoomList[j].position, ninetyDegPortalsInEndRoomList[k])) // Check for world position when rotated by 90 degrees
                        {
                            if (unrotatedRot != twoSeventyF) // unrotated rotation is 90 and rotation should therefore not be incremented by 90 when at 270
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != newPortalRot + ninetyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = ninetyF;
                                }
                            }
                            else                                    // 90 + 270 = 360 -> 0
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != zeroF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = ninetyF;
                                }
                            }
                        }
                        else if (Utils.VectorApproxComparison(portalsInLastRoomList[j].position, oneEightyDegPortalsInEndRoomList[k]))  // Check for world position when rotated by 180 degrees
                        {
                            if (unrotatedRot != oneEightyF && unrotatedRot != twoSeventyF)
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != newPortalRot + oneEightyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = oneEightyF;
                                }
                            }
                            else if (unrotatedRot == oneEightyF)        // 180 + 180 = 360 -> 0
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != zeroF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = oneEightyF;
                                }
                            }
                            else if (unrotatedRot == twoSeventyF)       // 180 + 270 = 450 -> 90
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != ninetyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = oneEightyF;
                                }
                            }
                        }
                        else if (Utils.VectorApproxComparison(portalsInLastRoomList[j].position, twoSeventyDegPortalsInEndRoomList[k]))  // Check for world position when rotated by 270 degrees
                        {
                            if (unrotatedRot == zeroF)
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != newPortalRot + twoSeventyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = twoSeventyF;
                                }
                            }
                            else if (unrotatedRot == ninetyF)           // 270 + 90 = 360 -> 0
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != zeroF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = twoSeventyF;
                                }
                            }
                            else if (unrotatedRot == oneEightyF)        // 270 + 180 = 450 -> 90
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != ninetyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = twoSeventyF;
                                }
                            }
                            else if (unrotatedRot == twoSeventyF)       // 270 + 270 = 540 -> 180
                            {
                                if (Mathf.Round(portalsInLastRoomList[j].eulerAngles.y * 100) / 100 != oneEightyF)
                                {
                                    connectedPortal = true;
                                    rotationParameter = twoSeventyF;
                                }
                            }
                        }
                        if (connectedPortal)
                        {
                            layoutList.Add(Instantiate(endRooms[i], Utils.worldSpacePoint, Quaternion.Euler(0.0f, rotationParameter, 0.0f)));
                            roomsUsed++;
                            Utils.SetActiveChild(layoutList[roomsUsed].transform, false, entryPortalTag, exitPortalTag);
                            layoutList[roomsUsed].SetActive(false);
                            return; // Breaks from the function
                        }
                    }
                }
            }
        }
    }
}
