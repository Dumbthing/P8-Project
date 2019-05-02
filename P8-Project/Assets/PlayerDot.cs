using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDot : MonoBehaviour
{
    public GameObject keyboardPlayer;
    public GameObject vrPlayer;
    GameObject playerRef;

    public GameObject map;
    private bool mapActive = false;

    void Start()
    {
        // Set player ref depending on which player is enabled. If both are enabled it should just choose the VR player

        if (vrPlayer.activeSelf == true)
            playerRef = vrPlayer;
        else if (keyboardPlayer.activeSelf == true)
            playerRef = keyboardPlayer;
    }

    void Update()
    {
        transform.position = playerRef.transform.position;

        if (Input.GetKeyDown(KeyCode.M) && mapActive == false)
        {
            map.SetActive(true);
            mapActive = true;
        }

        else if (Input.GetKeyDown(KeyCode.M) && mapActive == true)
        {
            map.SetActive(false);
            mapActive = false;
        }
    }
}
