using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	public GameObject activeEnemiesGroup;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			Debug.Log ("Q pressed");
			GameObject targetEnemy = getTargetEnemy ();
			if (targetEnemy.GetComponent<EnemyScript> ().enemyType == 1) {
				targetEnemy.GetComponent<EnemyScript> ().enabled = false;
				targetEnemy.GetComponent<Collider> ().enabled = true;
				targetEnemy.GetComponent<Rigidbody> ().isKinematic = false;
				targetEnemy.GetComponent<Rigidbody> ().AddExplosionForce (2000f, Vector3.zero, 1000, 10f);
				StartCoroutine ("Vanish", targetEnemy);
			}
		} else if (Input.GetKeyDown (KeyCode.W)) {
			Debug.Log ("W pressed");
			GameObject targetEnemy = getTargetEnemy ();
			if (targetEnemy.GetComponent<EnemyScript> ().enemyType == 2) {
				targetEnemy.GetComponent<EnemyScript> ().enabled = false;
				targetEnemy.GetComponent<Collider> ().enabled = true;
				targetEnemy.GetComponent<Rigidbody> ().isKinematic = false;
				targetEnemy.GetComponent<Rigidbody> ().AddForceAtPosition (Vector3.back * 300f, new Vector3 (targetEnemy.transform.position.x, 0, targetEnemy.transform.position.z), ForceMode.Force);
				StartCoroutine ("Vanish", targetEnemy);
			}
		}
	}

	private GameObject getTargetEnemy() {
		GameObject targetEnemy = null;
		float minZ = float.MaxValue;
		for (int i = 0; i < activeEnemiesGroup.transform.childCount; i++) {
			GameObject e = activeEnemiesGroup.transform.GetChild (i).gameObject;
			if (e.transform.position.z < minZ && e.GetComponent<EnemyScript> ().enabled) {
				minZ = e.transform.position.z;
				targetEnemy = e;
			}
		}
		return targetEnemy;
	}

	IEnumerator Vanish(GameObject target) {
		yield return new WaitForSeconds(3);
		Destroy (target);
	}
}
