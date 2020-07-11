using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTargetYPos : MonoBehaviour {


    public Transform target;
    Vector3 destination;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
        destination = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(destination);
		
	}
}
