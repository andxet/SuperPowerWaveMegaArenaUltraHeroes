using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
	public float speed;
	public int enemyType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.back * (speed * Time.deltaTime));

		if (transform.position.z < 0f) {
			//TODO: gameover or damage
		}
	}
}
