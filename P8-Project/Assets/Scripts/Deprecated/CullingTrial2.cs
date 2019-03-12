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
    public bool portalEnterChecker = false;
    public bool portalDir = false;


    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        oldMask = cam.cullingMask; // Original culling mask, but with invisible layers.
    }

    private void OnTriggerEnter(Collider col)
    {
        if (portalEnterChecker == false && roomArr.Length > previousRoom + 1)
        {
            if (portalEntered == 0) // Portal that activates the next room (and sets its layer to visible)
            {
                if (col.transform.localPosition == lastPortalPos) // Should be same position as last portal id
                {
                    if (currentRoom == 0)
                    {
                        Debug.Log("Room 1, entered previous portal, so nothing should happen");
                    }
                    else
                    {
                        Debug.Log("Same portal, before pass");
                        currentRoom--;
                        roomArr[currentRoom].SetActive(true);
                        ShowLayer(roomArr[currentRoom].layer, col);
                        portalEntered++;
                        portalDir = true;
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
                    if (portalDir) // Going forward
                    {
                        Debug.Log("Portaldir true");
                        roomArr[currentRoom].SetActive(false);
                        HideLayer(roomArr[currentRoom].layer, col);
                        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToPrevious", true);
                        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToPreviousPassed", true);
                        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToNext", true);
                        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToNextPassed", true);
                        portalEntered = 0;
                        portalDir = false;
                    }
                    else // Going backward 
                    {
                        Debug.Log("Portaldir false");
                        roomArr[currentRoom - 1].SetActive(false);
                        HideLayer(roomArr[currentRoom - 1].layer, col);
                        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToPrevious", true);
                        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToPreviousPassed", true);
                        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToNext", true);
                        objSearch.SetActiveChild(roomArr[currentRoom].transform, "PortalToNextPassed", true);
                        portalEntered = 0;
                    }
                }
            }
            lastPortalId = col.GetInstanceID();
            lastPortalPos = col.transform.localPosition;
            portalEnterChecker = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        portalEnterChecker = false; // Ensures that OnTriggerEnter is only called once per portal     
    }

    private void GoThroughPortal(Collider portal) // Deprecated ?
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

    public void SetActiveChild(Transform parent, string _tag, bool boolVal)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
                child.gameObject.SetActive(boolVal);
        }
    }

    public void SetSiblingPortalActive(Transform parent, string _tag, bool boolVal)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag && child.GetInstanceID() == lastPortalId)
            {
                child.gameObject.SetActive(boolVal);
            }
        }
    }
}
