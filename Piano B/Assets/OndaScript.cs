using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaScript : MonoBehaviour {
	public bool collided;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Onda") {
			collided = true;
		}
	}

}
