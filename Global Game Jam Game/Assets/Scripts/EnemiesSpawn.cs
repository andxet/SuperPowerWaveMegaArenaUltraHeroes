using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawn : MonoBehaviour {
	public float spawnInterval;
	public GameObject[] enemiesPrefabs;
    public bool CasualPositionSpawn = false;

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
            float x;
            if (CasualPositionSpawn)
                x = Random.Range(-2, 2);
            else
                x = (typeChoose - 2) * 0.5f;
            enemy.transform.localPosition = new Vector3 (x, 1, 20);
			transform.rotation = q;
		}
	}
}
