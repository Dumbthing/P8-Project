using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    /// Inspector variables
    StencilController stencil;
    public ProceduralLayoutGeneration layout;

    /// Public, non-inspector variables

    /// Private variables
    private bool singlePortalCollision = false, playerReturned = false;
    private int portalExitScenario = 0; // Default is 0: do nothing
    private Vector3 backwardPortalPos, lastPortalPos;
    private Vector3 playerExitPosition;

    private float zeroF = 0.0f, ninetyF = 90.0f, oneEightyF = 180.0f, twoSeventyF = 270.0f;

    void Start()
    {
        stencil = GetComponent<StencilController>(); // Script that handles which layer is rendered by which camera
    }

    private void OnTriggerExit(Collider portal) // Out of portal
    {
        

        playerExitPosition = transform.localPosition;

        // Checks for portal's rotation, and the player's exit position to see if they exited on the same side as they entered from. 
        if ((portal.transform.eulerAngles.y == zeroF && playerExitPosition.z >= portal.transform.position.z) ||
            (portal.transform.eulerAngles.y == oneEightyF && playerExitPosition.z <= portal.transform.position.z) ||
            (portal.transform.eulerAngles.y == ninetyF && playerExitPosition.x >= portal.transform.position.x) ||
            (portal.transform.eulerAngles.y == twoSeventyF && playerExitPosition.x <= portal.transform.position.x)) 
        {
            Transition(portal);
        }
    }


    private void Transition(Collider portal)
    {
        if (portal.tag == layout.exitPortalTag && layout.currentRoom < layout.layoutList.Count - 1) // Exit is the exit of the room
        {
            layout.currentRoom++;
            portalExitScenario = 1;
        }
        else if (portal.tag == layout.entryPortalTag && layout.currentRoom > 0) // Entry is the entry of the room
        {
            layout.currentRoom--;
            portalExitScenario = 2;
        }
        else
            Debug.Log("Unknown portal tag encountered - No action taken.");

        stencil.SetStencilShader(layout.currentRoom);
        Utils.SetActivePortal(layout.layoutList[layout.currentRoom].transform, true, layout.entryPortalTag, layout.exitPortalTag); // Enable portals in new room, in case they are disabled.=======
      


        if (portalExitScenario == 1) // Scenario 1: Enter "next-room" portal
        {
            Utils.SetActivePortal(layout.layoutList[layout.currentRoom - 1].transform, false, layout.entryPortalTag, layout.exitPortalTag); // Since we enabled new portals, we should disable the existing ones.
            if (layout.currentRoom < layout.layoutList.Count - 1)
                layout.layoutList[layout.currentRoom + 1].SetActive(true);
            if (layout.currentRoom > 1)
                layout.layoutList[layout.currentRoom - 2].SetActive(false);
        }
        else if (portalExitScenario == 2) // Scenario 2: Enter "previous-room" portal
        {
            Utils.SetActivePortal(layout.layoutList[layout.currentRoom + 1].transform, false, layout.entryPortalTag, layout.exitPortalTag); // Since we enabled new portals, we should disable the existing ones.
            if (layout.currentRoom > 0)
                layout.layoutList[layout.currentRoom - 1].SetActive(true);
            if (layout.currentRoom < layout.layoutList.Count - 2)
                layout.layoutList[layout.currentRoom + 2].SetActive(false);

        }
        layout.NextPortalPosUpdater.UpdateActiveNextPortalPos();
        layout.PreviousPortalUpdater.UpdateActivePreviousPortalPos();
        singlePortalCollision = false;
    } 
}

