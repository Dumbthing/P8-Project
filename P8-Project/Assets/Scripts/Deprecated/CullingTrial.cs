using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingTrial : MonoBehaviour {

    Camera cam;
    LayerMask oldMask;
    LayerMask newMask;
    public GameObject[] ZTopMidRooms;
    public GameObject startRoom;

    GameObject currentRoom;
    GameObject previousRoom;
    public int currentLayer = 8;
    private int newLayer;


    void Start () {
        cam = GetComponentInChildren<Camera>();
        oldMask = cam.cullingMask; // Original culling mask, but with invisible layers.
    }

    private void OnTriggerEnter(Collider entry)
    {
        if (entry.tag == "Portal")
            entry.tag = "PortalToNext";
        if (entry.tag == "PortalToNext")
        {
            ZTopMidRooms[0].SetActive(true);
            cam.cullingMask = ShowLayer(cam.cullingMask, 20, entry);    
        }
        else if (entry.tag == "PortalToPrevious")
        {
            startRoom.SetActive(true);
            cam.cullingMask = ShowLayer(cam.cullingMask, 8, entry);
        }
        else if (entry.tag == "Layer1Trigger")
        {
            newMask = oldMask;
            cam.cullingMask = newMask;
            Debug.Log("Collision detected with a Layer 1 Trigger!");
        }
        else if (entry.tag == "Layer2Trigger")
        {
            newMask = oldMask & (~(1 << 11 | 1 << 12));
            cam.cullingMask = newMask;
            Debug.Log("Collision detected with a Layer 2 Trigger!");
            entry.gameObject.layer = 0;
        }
        else if (entry.tag == "Layer3Trigger")
        {
            cam.cullingMask = 0 << 11;
            cam.cullingMask = 0 << 12;
            cam.cullingMask = 1 << 13;
            Debug.Log("Collision detected with a Layer 3 Trigger!");
            entry.gameObject.layer = 0;
        }

    }

    private void OnTriggerExit(Collider exit)
    {
        if (exit.tag == "PortalToNext")
        {
            //startRoom.SetActive(false);
            cam.cullingMask = HideLayer(cam.cullingMask, currentLayer, exit);
            exit.tag = "PortalToPrevious";
        }
        else if (exit.tag == "PortalToPrevious")
        {
            // Change the portal tag
            exit.tag = "PortalToNext";
            // Disable rooms that are behind walls (culling?)
            previousRoom.SetActive(false);
        }
    }

    void GoThroughPortal(Collider portal)
    {
        string layer = portal.gameObject.layer.ToString();
    }

    private LayerMask HideLayer(LayerMask mask, int layerToHide, Collider portal)
    {
        currentLayer = newLayer;
        return mask & (~1 << layerToHide);
    }
    private LayerMask ShowLayer(LayerMask mask, int layerToShow, Collider portal)
    {
        Debug.Log("Event");
        newLayer = layerToShow;
        //return mask = LayerMask.NameToLayer(portal.gameObject.layer.ToString());    
        return mask | 1 << layerToShow;
    }
    private LayerMask ReplaceLayer(LayerMask mask, int layerToShow, int layerToHide, Collider portal)
    {
        currentLayer = layerToShow;
        return mask & (~(1 << layerToHide | 1 << layerToShow));
    }
}
