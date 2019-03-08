using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Vector3 position;
	private float speed = 2.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		position = transform.position;
		transform.Translate(speed*Input.GetAxis("Horizontal") * Time.deltaTime, 0f, speed*Input.GetAxis("Vertical") * Time.deltaTime);
	}

	
     
//    public void Move()
//     {
     
//     	Vector3 Movement = new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
     
//    	position += Movement * speed * Time.deltaTime;
     
//	}

}