using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPrefabMaterials : MonoBehaviour
{
    /// Public variables, visible in the Inspector

    /// Public variables, hidden from the Inspector. Use keyword [HideInInspector] before every variable!
    [HideInInspector] public List<GameObject> layoutList; // Public, since other scripts need access to it
    [HideInInspector] public static GameObject[] startRooms, endRooms, rooms;
    [HideInInspector] public int currentRoom = 0;

    /* DESCRIPTION NEEDED
     * x
     * x
     * x
     */
    private void Awake()
    {
        layoutList = new List<GameObject>();    // We choose to use a list since we don't know the final size of the layout
        LoadPrefabsToList();
        for (int i = 0; i < startRooms.Length; i++)
        {
            layoutList.Add(Instantiate(startRooms[i], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity));
        }
        for (int i = 0; i < rooms.Length; i++)
        {
            layoutList.Add(Instantiate(rooms[i], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity));
        }
        for (int i = 0; i < endRooms.Length; i++)
        {
            layoutList.Add(Instantiate(endRooms[i], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity));
        }
        SetStencilShader(0);
    }

    private void LoadPrefabsToList()
    {
        startRooms = Resources.LoadAll<GameObject>("Start-Rooms");
        endRooms = Resources.LoadAll<GameObject>("End-Rooms");
        rooms = Resources.LoadAll<GameObject>("Rooms");
    }

    public void SetStencilShader(int currentRoom)
    {
        for (int i = 0; i < layoutList.Count; i++)
        {
            foreach (Renderer r in layoutList[i].GetComponentsInChildren<Renderer>())
            {
                if (r.tag != "EntryPortal" && r.tag != "ExitPortal" && r.tag != "Stencil")
                {
                    r.sharedMaterial = Resources.Load("Materials/Default", typeof(Material)) as Material;
                }
            }
        }
    }

}
