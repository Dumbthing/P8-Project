using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTexture : MonoBehaviour {

    Camera camera;
    Material material;

	// Use this for initialization
	void Start () {
        camera = GetComponentInChildren<Camera>();
        material = GetComponent<Renderer>().material;
        //texture = GameObject.Find("/Portal1-2/RenderPlaneTemplate").GetComponent<Material>().mainTexture;
        //material = GameObject.FindGameObjectWithTag("Portal").GetComponent<Renderer>().material;
       

        if (camera.targetTexture != null)
        {
            camera.targetTexture.Release();
        }

        camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        material.mainTexture = camera.targetTexture;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
