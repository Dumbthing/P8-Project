using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePrefabs : MonoBehaviour
{
    public GameObject[] rooms; // start rooms only contain EXIT portals, end rooms only contain ENTRY portals, rooms contain both
    public float rotationParameter = 0.0f;
    public List<GameObject> layout;

    void Start()
    {
        layout = new List<GameObject>();
        //Vector3 portalPos = new Vector3(1.0f, 0.0f, 0.5f);
        //Quaternion rotationNinety = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        //Quaternion rotationOneEighty = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        //Quaternion rotationTwoSeventy = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        //portalPos = rotationNinety * portalPos;
        //portalPos = new Vector3(1.0f, 0.0f, 0.5f);
        //portalPos = rotationOneEighty * portalPos;
        //portalPos = new Vector3(1.0f, 0.0f, 0.5f);
        //portalPos = rotationTwoSeventy * portalPos;
        for (int i = 0; i < rooms.Length; i++)
        {
            //List<Vector3> noRotation = GetPortalPositionsInRoom(rooms[i], "EntryPortal", "ExitPortal", 0.0f);
            //List<Vector3> rotation = GetPortalPositionsInRoom(rooms[i], "EntryPortal", "ExitPortal", rotationParameter);
            //for (int j = 0; j < rotation.Count; j++)
            //{
            //    Debug.Log("No rotation world potions for portals in room " + i + ", number " + j + ": " + noRotation[j]);
            //    Debug.Log(rotationParameter + " degree rotation of world potion for portals in room " + i + ", number " + j + ": " + rotation[j]);
            //}

            
            layout.Add(Instantiate(rooms[i], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f))); // Normal placement
            foreach (Vector3 vec in GetPortalPositionsInRoom(layout[i*2], "EntryPortal", "ExitPortal", 0.0f)) {
                Debug.Log("Layout[" + i * 2 + "] portal: " + vec);
            }
            layout.Add(Instantiate(rooms[i], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, rotationParameter, 0.0f)));
            foreach (Vector3 vec in GetPortalPositionsInRoom(layout[i*2+1], "EntryPortal", "ExitPortal", rotationParameter))
            {
                Debug.Log("Layout[" + i * 2 + 1 + "] portal: " + vec);
            }
        }
    }

    static public void RotateChildren(GameObject parent, float rotation)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            child.position = Quaternion.Euler(0.0f, rotation, 0.0f) * child.position;
            child.localRotation = new Quaternion(child.localRotation.x, child.localRotation.y + rotation, child.localRotation.z, child.localRotation.z);
        }
    }

    static public List<Vector3> GetPortalPositionsInRoom(GameObject room, string entryPortalTag, string exitPortalTag, float roomRotation)
    {
        List<Vector3> portalPositions = new List<Vector3>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == entryPortalTag || child.tag == exitPortalTag)
            {
                Transform portalPos = child;
                portalPos.Rotate(new Vector3(0.0f, 0.0f, 0.0f), roomRotation); // Rotates the vector around a point with an amount of degrees
                portalPositions.Add(portalPos.position);
            }
        }
        return portalPositions;
    }

}
