using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDot : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject map;
    private bool mapActive = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerObject.transform.position;

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
