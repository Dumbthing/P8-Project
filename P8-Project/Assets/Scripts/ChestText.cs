﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestText : MonoBehaviour
{
    public Canvas thisCanvas;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if (thisCanvas.enabled)
        {
            thisCanvas.enabled = false;
            player = GameObject.FindGameObjectWithTag("Player");
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current angle = " + transform.localEulerAngles.z);
        if (!thisCanvas.enabled && transform.localEulerAngles.z >= 40f)
        {
            thisCanvas.enabled = true;
        }
        else if (thisCanvas.enabled && transform.localEulerAngles.z < 40f)
        {
            thisCanvas.enabled = false;
        }
        if (thisCanvas.enabled)
        {
            thisCanvas.transform.LookAt(player.transform.position);
            thisCanvas.transform.Rotate(thisCanvas.transform.rotation.x, 180, thisCanvas.transform.rotation.z);
        }

    }
}
