using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingTrial2 : MonoBehaviour {

    Camera cam;
    public GameObjectSearcher objSearch;
    LayerMask oldMask;
    public GameObject[] roomArr;

    public int portalEntered = 0;
    public int lastPortalId;
    public Vector3 lastPortalPos;
    public int currentRoom = 0;
    public int previousRoom = 0;
    public int currentLayer = 8;
    private int newLayer;


    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        oldMask = cam.cullingMask; // Original culling mask, but with invisible layers.
    }

    //private void OnTriggerExit(Collider col)
    //{
    //    //// Might be better to go into variable based system, rather than tag based.

    //    if (col.tag == "PortalToNext") // Portal that activates the next room (and sets it layer to visible)
    //    {
    //        roomArr[currentRoom + 1].SetActive(true);
    //        ShowLayer(roomArr[currentRoom + 1].layer, col);
    //        previousRoom = currentRoom;
    //        col.tag = "PortalToNextPassed";
    //        ///// IF PortalToPreviousPassed 
    //        /// Set it to PortalToPrevious
            
    //    }
    //    else if (col.tag == "PortalToNextPassed") // Portal that disables last created room
    //    {
    //        roomArr[currentRoom + 1].SetActive(false);
    //        HideLayer(roomArr[currentRoom + 1].layer, col);
    //        col.tag = "PortalToNext";
    //    }
    //    else if (col.tag == "PortalToPrevious") // Portal that disables previous room
    //    {
    //        roomArr[currentRoom].SetActive(false);
    //        HideLayer(roomArr[currentRoom].layer, col);
    //        currentRoom++;
    //        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToPrevious", true);
    //        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToPreviousPassed", true);
    //        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToNext", true);
    //        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToNextPassed", true);
    //        col.tag = "PortalToPreviousPassed";
    //    }
    //    else if (col.tag == "PortalToPreviousPassed") // Portal that activates the previous room
    //    {
    //        if (currentRoom != 0)
    //        {
    //            currentRoom--;
    //            roomArr[currentRoom].SetActive(true);
    //            ShowLayer(roomArr[currentRoom].layer, col);
    //            col.tag = "PortalToPrevious";
    //        }
    //        else // Player went the wrong way :(
    //        {

    //        }
    //    }
    //}
    
    
    private void OnTriggerExit(Collider col)
    {
        if (roomArr.Length > previousRoom+1)
        {
            if (portalEntered == 0) // Portal that activates the next room (and sets it layer to visible)
            {
                if (col.transform.localPosition == lastPortalPos) // Should be same position as last portal id
                {
                    if (currentRoom == 0)
                    {

                    }
                    else
                    {
                        Debug.Log("Same portal, before pass");
                        currentRoom--;
                        roomArr[currentRoom].SetActive(true);
                        ShowLayer(roomArr[currentRoom].layer, col);
                    }
                }
                else
                {
                    Debug.Log("col pos: " + col.transform.localPosition + ", last Portal pos: " + lastPortalPos);
                    roomArr[currentRoom + 1].SetActive(true);
                    ShowLayer(roomArr[currentRoom + 1].layer, col);
                    previousRoom = currentRoom;
                    portalEntered++;
                    currentRoom++;
                }
            }
            else if (portalEntered == 1) // Portal that disables last created room
            {
                if (col.GetInstanceID() == lastPortalId)
                {
                    Debug.Log("Same portal, after pass");
                    roomArr[currentRoom].SetActive(false);
                    HideLayer(roomArr[currentRoom].layer, col);
                    portalEntered--;
                    currentRoom--;
                }
                else
                {
                    roomArr[currentRoom - 1].SetActive(false);
                    HideLayer(roomArr[currentRoom - 1].layer, col);
                    objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToPrevious", true);
                    objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToPreviousPassed", true);
                    objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToNext", true);
                    objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToNextPassed", true);
                    previousRoom = currentRoom;
                    portalEntered = 0;
                }
            }
            lastPortalId = col.GetInstanceID();
            lastPortalPos = col.transform.localPosition;
        }
    }

    void GoThroughPortal(Collider portal)
    {
        string layer = portal.gameObject.layer.ToString();
    }

    private void HideLayer(int layerToHide, Collider portal)
    {
        currentLayer = newLayer;
        Debug.Log("Hiding layer " + layerToHide);
        cam.cullingMask |=  0 << layerToHide;
    }
    private void ShowLayer(int layerToShow, Collider portal)
    {
        newLayer = layerToShow;  
        cam.cullingMask |= 1 << layerToShow;
    }
    private void ReplaceLayer(int layerToShow, int layerToHide, Collider portal)
    {
        currentLayer = layerToShow;
        cam.cullingMask &= (~(1 << layerToHide | 1 << layerToShow));
    }
}
