using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTest : MonoBehaviour
{
    public GameObject[] rooms;

    void Start()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            List<Transform> debugList = GetPortalTransformsInRoom(rooms[i]);
            foreach (Transform trans in debugList)
                Debug.Log("Name: " + trans + ", position: " + trans.localPosition + ", rotation: " + trans.localRotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Transform> GetPortalTransformsInRoom(GameObject room)
    {
        List<Transform> portalPositions = new List<Transform>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == "Portal")
            {
                portalPositions.Add(child);
            }
        }
        return portalPositions;
    }
}
