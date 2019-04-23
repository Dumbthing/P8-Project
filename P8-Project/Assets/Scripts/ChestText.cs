using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestText : MonoBehaviour
{
    public Canvas thisCanvas;
    // Start is called before the first frame update
    void Start()
    {
        if(thisCanvas.enabled) {
            thisCanvas.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current angle = " + transform.localEulerAngles.z);
        if(!thisCanvas.enabled && transform.localEulerAngles.z >= 40f) {
            thisCanvas.enabled = true;
        }
        else if(thisCanvas.enabled && transform.localEulerAngles.z < 40f) {
            thisCanvas.enabled = false;
        }
    }
}
