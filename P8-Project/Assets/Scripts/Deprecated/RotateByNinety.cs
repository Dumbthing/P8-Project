using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByNinety : MonoBehaviour
{
    public float rotation = 90;

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            //Quaternion rotate = Quaternion.Euler(transform.position.x, transform.position.y + rotation, transform.position.z);
            Quaternion rotate = Quaternion.Euler(0.0f, rotation, 0.0f);
            Debug.Log("Before rotation: " + transform.position);
            transform.position = rotate * transform.position;
            Debug.Log("After rotation: " + transform.position);
        }
    }

    
}
