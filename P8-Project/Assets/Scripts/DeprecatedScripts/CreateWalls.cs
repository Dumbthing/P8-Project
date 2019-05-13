using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWalls : MonoBehaviour {

    public float wallIntersectionHeight = 2;
    public GameObject mousePointer;
    public GameObject wallIntersectionPrefab;
    public GameObject wallPrefab;

    private float snapValue = 0.5f;
    private bool currentlyCreatingWall;
    GameObject lastIntersection;
	
	// Update is called once per frame
	void Update () {
        mousePointer.transform.position = SnapPosition(GetWorldPoint());
        GetMouseInput();
	}
    
    // Gets the world point of the cursor
    public Vector3 GetWorldPoint()
    {
        Camera cam = GetComponent<Camera>();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if(Physics.Raycast(ray, out rayHit))
        {
            return rayHit.point;
        }
        return Vector3.zero;
    }

    // Snaps position to a whole number (based on a snap value)
    public Vector3 SnapPosition(Vector3 original)
    {
        Vector3 snapped;
        snapped.x = Mathf.Floor(original.x + snapValue);
        snapped.y = Mathf.Floor(original.y + snapValue);
        snapped.z = Mathf.Floor(original.z + snapValue);
        return snapped;
    }

    void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            StartWall();
        else if (Input.GetMouseButtonUp(0))
            SetWall();
        else
        {
            if (currentlyCreatingWall)
                UpdateWall();
        }
    }

    private void StartWall()
    {
        currentlyCreatingWall = true;
        Vector3 startPos = GetWorldPoint();
        startPos = SnapPosition(startPos);
        GameObject startWall = Instantiate(wallIntersectionPrefab, startPos, Quaternion.identity);
        startWall.transform.position = new Vector3(startPos.x, startPos.y + (wallIntersectionHeight/2), startPos.z);
        lastIntersection = startWall;
    }

    private void SetWall()
    {
        currentlyCreatingWall = false;
    }

    private void UpdateWall()
    {
        Vector3 current = GetWorldPoint();
        current = SnapPosition(current);
        current = new Vector3(current.x, current.y + (wallIntersectionHeight / 2), current.z);
        if(!current.Equals(lastIntersection.transform.position))
        {
            CreateWallSegment(current);
        }
    }

    private void CreateWallSegment(Vector3 current)
    {
        GameObject newIntersection = Instantiate(wallIntersectionPrefab, current, Quaternion.identity);
        Vector3 middle = Vector3.Lerp(newIntersection.transform.position, lastIntersection.transform.position, 0.5f);
        GameObject newWall = Instantiate(wallPrefab, middle, Quaternion.identity);
        newWall.transform.LookAt(lastIntersection.transform);
        lastIntersection = newIntersection;
    }
}
