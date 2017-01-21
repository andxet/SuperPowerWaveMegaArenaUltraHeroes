using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawn : MonoBehaviour {
	public float spawnInterval;
	public GameObject[] enemiesPrefabs;

	private float lastSpanwTime = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float timeElapsed = Time.time - lastSpanwTime;
		if (lastSpanwTime == 0 || timeElapsed > spawnInterval) {
			lastSpanwTime =  Time.time;
			Quaternion q = transform.rotation;
			transform.rotation = Quaternion.identity;
            int typeChoose = Random.Range(0, enemiesPrefabs.Length);
            GameObject enemy = Instantiate (enemiesPrefabs [typeChoose]);
			enemy.transform.parent = transform;
			//float x = Random.Range (-2, 2);
			enemy.transform.localPosition = new Vector3 ((typeChoose - 2) * 0.5f, 1, 20);
			transform.rotation = q;
		}
	}
}
