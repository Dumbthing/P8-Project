using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnabler : MonoBehaviour
{
    PortalManager portalScript;
    Camera currentLayerCam, nextLayerCam, previousLayerCam, skyboxCam;
    void Start()
    {
        portalScript = GetComponent<PortalManager>();
        skyboxCam = GameObject.Find("SkyboxCam").GetComponent<Camera>();
        skyboxCam.enabled = true;
        previousLayerCam = GameObject.Find("PreviousLayerCam").GetComponent<Camera>();
        previousLayerCam.enabled = true;
        nextLayerCam = GameObject.Find("NextLayerCam").GetComponent<Camera>();
        nextLayerCam.enabled = true;
        currentLayerCam = GetComponentInChildren<Camera>(); // Main camera
        currentLayerCam.enabled = true;
    }

    public void SetNewCullingMasks(int currentRoom)
    {
        // Reset all culling masks
        previousLayerCam.cullingMask = 0;
        currentLayerCam.cullingMask = 0;
        nextLayerCam.cullingMask = 0;
        
        // Bit-shift new culling masks based on layer of current room (depending on camera), without going out of bounds.
        currentLayerCam.cullingMask |= 1 << portalScript.layout[currentRoom].layer;
        if (currentRoom > 0)
            previousLayerCam.cullingMask |= 1 << portalScript.layout[currentRoom - 1].layer;
        if (currentRoom < portalScript.maxRooms)
            nextLayerCam.cullingMask |= 1 << portalScript.layout[currentRoom + 1].layer;
    }

    public void SetPreviousLayer(int onOrOff, int newPreviousLayer)
    {
        previousLayerCam.cullingMask |= onOrOff << newPreviousLayer;
    }

    public void SetCurrentLayer(int onOrOff, int newCurrentLayer)
    {
        currentLayerCam.cullingMask |= onOrOff << newCurrentLayer;
    }

    public void SetNextLayer(int onOrOff, int newNextLayer)
    {
        nextLayerCam.cullingMask |= onOrOff << newNextLayer;
    }

    private void HideLayer(Camera cam, int layerToHide)
    {
        cam.cullingMask |= 0 << layerToHide;
    }
    private void ShowLayer(Camera cam, int layerToShow)
    {
        cam.cullingMask |= 1 << layerToShow;
    }
    //private void ReplaceLayer(Camera cam, int layerToShow, int layerToHide) // UNTESTED
    //{
    //    cam.cullingMask &= (~(1 << layerToHide | 1 << layerToShow));
    //}
}
