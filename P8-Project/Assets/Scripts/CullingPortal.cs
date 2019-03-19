using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingPortal : MonoBehaviour {

    // Inspector variables
    public GameObject[] rooms;

    [HideInInspector]
    public GameObject[] newRooms;
    public int maxRooms;

    // Referencial variables to components of current gameobject
    Camera playerCam;
    LayerMask oldMask;
    CameraEnabler cameras;

    // Comparison variables
    private bool singlePortalCollision = false, endRoomReached = false, playerReturned = false;
    private int portalExitScenario = 0, portalStep = 0; // Default is 0: do nothing
    private int currentRoom = 0;
    
    private Vector3 backwardPortalPos, lastPortalPos;

    void Start ()
    {
        cameras = GetComponent<CameraEnabler>();
        playerCam = GetComponentInChildren<Camera>();
        //oldMask = playerCam.cullingMask; // Original culling mask
        maxRooms = rooms.Length-1;
        newRooms = new GameObject[rooms.Length];
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            newRooms[i] = Instantiate(rooms[i], new Vector3(0f, 0f, 0f) ,Quaternion.identity);
            if (i != 0)
            {
                SetActiveChild(newRooms[i].transform, "Portal", false);
            }
            else
            {
                SetActiveChild(newRooms[i].transform, "Portal", true);
            }

        }
    }

    private void OnTriggerEnter(Collider portal) // Portal collision
    {
        if (!singlePortalCollision)
        {
            /// Check whether current room is the last room, whether a player has just enetered a portal, or whether the portal is leading backward
            //if (currentRoom < maxRooms || portalStep == 1 || portal.transform.localPosition == backwardPortalPos)
            if (portalStep == 0) // First portal
            {
                if (portal.transform.localPosition != backwardPortalPos && currentRoom < maxRooms)
                {
                    currentRoom++;
                    portalExitScenario = 1;
                    cameras.SetNewCullingMasks(currentRoom);

                }
                else if (portal.transform.localPosition == backwardPortalPos && currentRoom != 0)
                {
                    currentRoom--;
                    portalExitScenario = 2;
                    cameras.SetNewCullingMasks(currentRoom);
                }
                else if (currentRoom == maxRooms || currentRoom == 0)
                {
                    // Do nothing, and make sure next step also does nothing
                    endRoomReached = true;
                }
                lastPortalPos = portal.transform.localPosition;
                portalStep++;
            }
            else if (portalStep == 1)
            {
                if (!endRoomReached)
                {
                    if (portal.transform.localPosition == lastPortalPos) // Player went back while between portals
                    {
                        if (portalExitScenario == 1)
                        {
                            currentRoom--;
                            cameras.SetNewCullingMasks(currentRoom);
                        }
                        if (portalExitScenario == 2)
                        {
                            currentRoom++;
                            cameras.SetNewCullingMasks(currentRoom);
                        }
                        playerReturned = true;
                    }
                    else
                    {
                        if (portalExitScenario == 1)
                        {
                            /// Enters new room
                            //HideLayer(newRooms[currentRoom - 1].layer, portal);
                        }
                        else if (portalExitScenario == 2)
                        {
                            /// Enters previous room
                            //HideLayer(newRooms[currentRoom + 1].layer, portal);
                        }

                        if (backwardPortalPos == new Vector3(0,0,0))
                        {
                            backwardPortalPos = portal.transform.localPosition;
                        }
                    }
                }
                portalStep--;
                endRoomReached = false;
            }
            /// Scenario 4 [INCOMPLETE] -
            singlePortalCollision = true;
        }
    }

    private void OnTriggerExit(Collider portal) // Out of portal
    {
        if (!playerReturned)
            PortalScenarioNext(portalExitScenario, portal);
        else
            PortalScenarioReturn(portalExitScenario, portal);
        singlePortalCollision = false; // Bool variable to stop multiple OnTriggerEnter calls
    }

    private void PortalScenarioNext(int scenario, Collider portal)
    {
        if (portalExitScenario != 0) // True for all except 0
        {
            SetActiveChild(newRooms[currentRoom].transform, "Portal", true); // Enable portals in new room, in case they are disabled.
        }
        if (portalExitScenario == 1) // Scenario 1: Enter "next-room" portal, exit other portal
        {
            SetActiveChild(newRooms[currentRoom - 1].transform, "Portal", false); // Since we enabled new portals, we should disable the existing ones.
        }
        else if (portalExitScenario == 2) // Scenario 2: Enter "previous-room" portal, exit other portal
        {
            SetActiveChild(newRooms[currentRoom + 1].transform, "Portal", false); // Since we enabled new portals, we should disable the existing ones.
        }
    }

    private void PortalScenarioReturn(int scenario, Collider portal)
    {
        if (portalExitScenario == 1)
        {
            SetActiveChild(newRooms[currentRoom + 1].transform, "Portal", false);
        }
        else if (portalExitScenario == 2)
        {
            SetActiveChild(newRooms[currentRoom - 1].transform, "Portal", false);
        }
        SetActiveChild(newRooms[currentRoom].transform, "Portal", true);
        playerReturned = false;
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

    // Handle player camera culling mask
    private void HideLayer(int layerToHide, Collider portal)
    {
        playerCam.cullingMask |= 0 << layerToHide;
    }
    private void ShowLayer(int layerToShow, Collider portal)
    {
        playerCam.cullingMask |= 1 << layerToShow;
    }
    private void ReplaceLayer(int layerToShow, int layerToHide, Collider portal) // UNTESTED
    {
        playerCam.cullingMask &= (~(1 << layerToHide | 1 << layerToShow));
    }
}
