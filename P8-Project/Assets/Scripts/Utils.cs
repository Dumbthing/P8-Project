using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    static public Vector3 worldSpacePoint = new Vector3(0.0f, 0.0f, 0.0f);

    static public   void RandomizeArray(GameObject[] arr) // Fischer-Yates shuffle
    {
        for (int i = 0; i < arr.Length; i++)
        {
            int r = Random.Range(0, arr.Length - 1);
            GameObject temp = arr[i];
            arr[i] = arr[r];
            arr[r] = temp;
        }
    }

    // Access other portals
    static public void SetActiveChild(Transform parent, bool enabled, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
                child.gameObject.SetActive(enabled);
        }
    }

    static public void SetActiveChild(Transform parent, bool enabled, string _tag1, string _tag2)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag1 || child.tag == _tag2)
                child.gameObject.SetActive(enabled);
        }
    }

    static public void SetSiblingPortalActivity(Transform parent, bool enabled, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                child.gameObject.SetActive(enabled);
            }
        }
    }

    static public void SetSiblingPortalActivity(Transform parent, bool enabled, string _tag1, string _tag2)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag1 || child.tag == _tag2)
            {
                child.gameObject.SetActive(enabled);
            }
        }
    }

    static public List<Transform> GetPortalTransformsInRoom(GameObject room, string entryPortalTag, string exitPortalTag)
    {
        List<Transform> portalPositions = new List<Transform>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == entryPortalTag || child.tag == exitPortalTag)
            {
                portalPositions.Add(child);
            }
        }
        return portalPositions;
    }

    static public List<Transform> GetPortalTransformsInRoom(GameObject room, string entryPortalTag, string exitPortalTag, float rotationParameter)
    {
        List<Transform> portalPositions = new List<Transform>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == entryPortalTag || child.tag == exitPortalTag)
            {
                Transform portalPos = child;
                portalPos.Rotate(worldSpacePoint, rotationParameter); // Rotates the vector around a point with an amount of degrees
                portalPositions.Add(portalPos);
                Debug.Log(portalPos);
            }
        }
        return portalPositions;
    }

    static public List<Vector3> GetPortalPositionInRoom(GameObject room, string entryPortalTag, string exitPortalTag)
    {
        List<Vector3> portalPositions = new List<Vector3>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == entryPortalTag || child.tag == exitPortalTag)
            {
                portalPositions.Add(child.localPosition);
            }
        }
        return portalPositions;
    }

    static public List<Quaternion> GetPortalRotationsInRoom(GameObject room, string entryPortalTag, string exitPortalTag)
    {
        List<Quaternion> portalRotations = new List<Quaternion>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == entryPortalTag || child.tag == exitPortalTag)
            {
                portalRotations.Add(child.localRotation);
            }
        }
        return portalRotations;
    }

    static public Vector3 GetARandomPortalPositionInRoom(GameObject room, string entryPortalTag, string exitPortalTag)
    {
        List<Vector3> newExitPortals = new List<Vector3>();
        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (child.tag == entryPortalTag || child.tag == exitPortalTag)
            {
                newExitPortals.Add(child.localPosition);
            }
        }
        int r = Random.Range(0, newExitPortals.Count - 1);
        if (r < 0)
            Debug.Log("No portals found in requested room");
        return newExitPortals[r];
    }

    static public GameObject[] RemoveIndices(GameObject[] IndicesArray, int RemoveAt)
    {
        GameObject[] newIndicesArray = new GameObject[IndicesArray.Length - 1];

        int i = 0;
        int j = 0;
        while (i < IndicesArray.Length)
        {
            if (i != RemoveAt)
            {
                newIndicesArray[j] = IndicesArray[i];
                j++;
            }
            i++;
        }
        return newIndicesArray;
    }

    static public void ChangeLayersRecursively(Transform trans, int newLayer)
    {
        foreach (Transform child in trans)
        {
            child.gameObject.layer = newLayer;
            ChangeLayersRecursively(child, newLayer);
        }
    }
}
