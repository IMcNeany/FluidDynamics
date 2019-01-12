using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBounds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(transform.position.z  > 11)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
        else if(transform.position.z < -1)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 11);
        }

        if (transform.position.x > 14)
        {
            gameObject.transform.position = new Vector3(1, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < 1)
        {
            gameObject.transform.position = new Vector3(14, transform.position.y, transform.position.z);
        }
    }
}
