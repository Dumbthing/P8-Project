using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{

    /// Inspector variables
    Shader ClearP;
    Shader ClearN;
    StencilController stencil;
    public ProceduralLayoutGeneration layout;
    public Material skyboxFantasy;
    public Material skyboxScifi;

    //Clear Depth Buffer Materials - Can be Shrunk 
    public Material NextFan;
    public Material NextSci;
    public Material PrevFan;
    public Material PrevSci;


    //public Cubemap skyboxFantasy;
    //public Cubemap skyboxScifi;
    public GameObject SkyBox;
    public GameObject NextDB;
    public GameObject PrevDB;

    /// Public, non-inspector variables

    /// Private variables
    private bool singlePortalCollision = false, playerReturned = false, fantasy = true, scifi = false, themeChange = false;
    private int portalExitScenario = 0; // Default is 0: do nothing
    private Vector3 backwardPortalPos, lastPortalPos;
    private Vector3 playerExitPosition;

    private float zeroF = 0.0f, ninetyF = 90.0f, oneEightyF = 180.0f, twoSeventyF = 270.0f;

    void Start()
    {

        stencil = GetComponent<StencilController>();// Script that handles which layer is rendered by which camera
        RenderSettings.skybox = skyboxFantasy;
    }

    private void OnTriggerExit(Collider portal) // Out of portal
    {
        playerExitPosition = transform.position;

        // Checks for portal's rotation, and the player's exit position to see if they exited on the same side as they entered from. 
        if ((Mathf.Round(portal.transform.eulerAngles.y) == zeroF && playerExitPosition.z >= portal.transform.position.z) ||
            (Mathf.Round(portal.transform.eulerAngles.y) == oneEightyF && playerExitPosition.z <= portal.transform.position.z) ||
            (Mathf.Round(portal.transform.eulerAngles.y) == ninetyF && playerExitPosition.x >= portal.transform.position.x) ||
            (Mathf.Round(portal.transform.eulerAngles.y) == twoSeventyF && playerExitPosition.x <= portal.transform.position.x))
        {
            Transition(portal);
            Debug.Log("Correctly passed through portal");
        }
        else
        {
            Debug.Log("Could not pass through portal, as it detected you went into and out of collider at the same place!" +
                "\nPortals rotation: " + Mathf.Round(portal.transform.eulerAngles.y) + ", PlayerExitPosition: " + playerExitPosition + " <? PortalTransform.pos.z: " + portal.transform.position.z + " || PortalTransform.pos.x: " + portal.transform.position.x);
        }

        //Transition(portal);
    }

    private void ThemeChangeScifi()
    {
        if (fantasy)
        {
            SkyBox.GetComponent<Renderer>().material = skyboxScifi;
            NextDB.GetComponent<Renderer>().material = NextSci;
            PrevDB.GetComponent<Renderer>().material = PrevSci;
            //RenderSettings.skybox = skyboxScifi;
            fantasy = false;
            scifi = true;
        }
    }

    private void ThemeChangeFantasy()
    {
        if (scifi)
        {
            SkyBox.GetComponent<Renderer>().material = skyboxFantasy;
            NextDB.GetComponent<Renderer>().material = NextFan;
            PrevDB.GetComponent<Renderer>().material = PrevFan;
            //RenderSettings.skybox = skyboxFantasy;
            fantasy = true;
            scifi = false;
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

        if (layout.currentRoom >= (layout.layoutList.Count / 2 - 2))
        {
            ThemeChangeScifi();
        }

        if (layout.currentRoom < (layout.layoutList.Count / 2 - 2))
        {
            ThemeChangeFantasy();
        }

    }
}

