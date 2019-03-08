using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingPortal : MonoBehaviour {

    // Inspector variables
    public GameObject[] rooms;

    // Referencial variables to components of current gameobject
    Camera playerCam;
    LayerMask oldMask;

    // Comparison variables
    private bool singlePortalCollision = false;
    private bool defineBackwardPortal = false;
    private int portalExitScenario = 0; // Default is 0: do nothing
    private int portalStep = 0;
    private int currentRoom = 0, maxRooms;
    
    private Vector3 backwardPortalPos;
    private Vector3 lastPortalPos;

	void Start ()
    {
        playerCam = GetComponentInChildren<Camera>();
        oldMask = playerCam.cullingMask; // Original culling mask
        maxRooms = rooms.Length-1;
    }

    private void OnTriggerEnter(Collider portal) // Portal collision
    {
        if (!singlePortalCollision)
        {
            // Check whether current room is the last room, or whether a player has just enetered a portal
            if (currentRoom < maxRooms || portalStep == 1 || portal.transform.localPosition == backwardPortalPos)
            {
                if (portalStep == 0) // First portal
                {
                    if (portal.transform.localPosition != backwardPortalPos)
                    {
                        currentRoom++;
                        portalExitScenario = 1;
                    }
                    else
                    {
                        currentRoom--;
                        portalExitScenario = 2;
                    }
                    lastPortalPos = portal.transform.localPosition;
                    portalStep++;
                }
                else // Second portal
                {
                    if (portal.transform.localPosition == lastPortalPos) // Player went back while between portals
                    {
                            rooms[currentRoom].SetActive(false);
                            HideLayer(rooms[currentRoom].layer, portal);
                        if (portalExitScenario == 1)
                            SetActiveChild(rooms[currentRoom].transform, "Portal", true); // Enable portals in new room, in case they are disabled.
                        if (portalExitScenario == 2)
                            SetActiveChild(rooms[currentRoom].transform, "Portal", true); // Enable portals in new room, in case they are disabled.
                    }
                    else
                    {
                        if (portalExitScenario == 1)
                        {
                            rooms[currentRoom - 1].SetActive(false);
                            HideLayer(rooms[currentRoom - 1].layer, portal);
                        }
                        else if (portalExitScenario == 2)
                        {
                            rooms[currentRoom + 1].SetActive(false);
                            HideLayer(rooms[currentRoom + 1].layer, portal);
                        }

                        if (!defineBackwardPortal)
                        {
                            backwardPortalPos = portal.transform.localPosition;
                            defineBackwardPortal = true;
                        }
                    }
                    portalStep--;
                }
            }

            /// Scenario 4 [INCOMPLETE] -
            /// Scenario 5 [INCOMPLETE] -
            singlePortalCollision = true;
        }
    }

    private void OnTriggerExit(Collider portal) // Out of portal
    {
        PortalScenario(portalExitScenario, portal);
        singlePortalCollision = false; // Bool variable to stop multiple OnTriggerEnter calls
    }

    private void PortalScenario(int scenario, Collider portal)
    {
        if (portalExitScenario != 0) // True for all except 0
        {
            rooms[currentRoom].SetActive(true); // Enabling new room, including its portals
            ShowLayer(rooms[currentRoom].layer, portal);
            SetActiveChild(rooms[currentRoom].transform, "Portal", true); // Enable portals in new room, in case they are disabled.
        }

        if (portalExitScenario == 1) // Scenario 1: Enter "next-room" portal, exit other portal
        {
            SetActiveChild(rooms[currentRoom - 1].transform, "Portal", false); // Since we enabled new portals, we should disable the existing ones.
        }
        else if (portalExitScenario == 2) // Scenario 2: Enter "previous-room" portal, exit other portal
        {
            SetActiveChild(rooms[currentRoom + 1].transform, "Portal", false); // Since we enabled new portals, we should disable the existing ones.
            
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
