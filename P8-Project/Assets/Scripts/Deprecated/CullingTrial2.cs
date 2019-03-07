using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingTrial2 : MonoBehaviour {

    Camera cam;
    LayerMask oldMask;
    LayerMask newMask;
    public GameObject[] ZTopMidRooms;
    public GameObject startRoom;

    int currentRoom = 0;
    int previousRoom;
    public int currentLayer = 8;
    private int newLayer;


    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        oldMask = cam.cullingMask; // Original culling mask, but with invisible layers.
    }

    private void OnTriggerEnter(Collider entry)
    {
        if (entry.tag == "PortalToNext") // Portal that activates the next room (and sets it layer to visible)
        {
            // SetActiveness
            ZTopMidRooms[currentRoom + 1].SetActive(true);
            // Set layer visibiltiy
            ShowLayer(ZTopMidRooms[currentRoom + 1].layer, entry);
            // Increment
            previousRoom = currentRoom;
            entry.tag = "PortalToNextPassed";
        }
        else if (entry.tag == "PortalToNextPassed") // Portal that disables last created room
        {
            // SetActiveness
            ZTopMidRooms[currentRoom + 1].SetActive(false);
            // Set layer visibiltiy
            HideLayer(ZTopMidRooms[currentRoom + 1].layer, entry);
            // Increment
            entry.tag = "PortalToNext";
        }
        else if (entry.tag == "PortalToPrevious") // Portal that disables previous room
        {
            ZTopMidRooms[currentRoom].SetActive(false);
            HideLayer(ZTopMidRooms[currentRoom].layer, entry);

            currentRoom++;
        }
        else if (entry.tag == "PortalToPreviousPassed") // Portal that activates the previous room
        {
            currentRoom--;
            ZTopMidRooms[currentRoom].SetActive(true);
        }
    }
    

    void GoThroughPortal(Collider portal)
    {
        string layer = portal.gameObject.layer.ToString();
    }

    private void HideLayer(int layerToHide, Collider portal)
    {
        currentLayer = newLayer;
        cam.cullingMask = cam.cullingMask & (~1 << layerToHide);
    }
    private void ShowLayer(int layerToShow, Collider portal)
    {
        newLayer = layerToShow;  
        cam.cullingMask =  cam.cullingMask | 1 << layerToShow;
    }
    private void ReplaceLayer(int layerToShow, int layerToHide, Collider portal)
    {
        currentLayer = layerToShow;
        cam.cullingMask = cam.cullingMask & (~(1 << layerToHide | 1 << layerToShow));
    }
}
