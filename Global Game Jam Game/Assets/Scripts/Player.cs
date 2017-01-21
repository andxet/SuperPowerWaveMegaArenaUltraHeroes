using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPosition {NORTH, EAST, SOUTH, WEST};

public class Player : MonoBehaviour
{
	public float attackRechargeTime;
    private int windows = 4;
    private float life;
    private float rotationTime = 0.1f;
    private bool rotating = false;
    private PlayerPosition position = PlayerPosition.NORTH;
	private float lastAttackTime = 0;

    // Use this for initialization
    void Start()
    {
        GameManager.Instance.RegisterPlayer(gameObject);
		GameManager.Instance.RechargeLabel.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotating)
        {
			EnemyType? type = null;
			float timeFromLastAttack = Time.time - lastAttackTime;
			if (timeFromLastAttack > attackRechargeTime) {
				//GameManager.Instance.RechargeLabel.text = "GUN READY";
				//GameManager.Instance.RechargeLabel.color = Color.green;
				GameManager.Instance.RechargeLabel.enabled = false;
				if (Input.GetKeyDown (KeyCode.Z)) {
					Debug.Log ("z pressed");
					type = EnemyType.TYPE1;
				} else if (Input.GetKeyDown (KeyCode.X)) {
					Debug.Log ("x pressed");
					type = EnemyType.TYPE2;
				} else if (Input.GetKeyDown (KeyCode.C)) {
					Debug.Log ("c pressed");
					type = EnemyType.TYPE3;
				} else if (Input.GetKeyDown (KeyCode.V)) {
					Debug.Log ("v pressed");
					type = EnemyType.TYPE4;
				}
			}
            // else if(Input.GetAxis("Horizontal")> 0.1f)
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
            }
            //else if (Input.GetAxis("Horizontal") < -0.1f)
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
				
			if (type != null)
            {
                GameObject targetEnemy = getTargetEnemy();
				if (targetEnemy != null && targetEnemy.GetComponent<EnemyScript> () != null) {
					bool hit = targetEnemy.GetComponent<EnemyScript> ().Hit ((EnemyType)type);
					if (!hit) {
						lastAttackTime = Time.time;
						//GameManager.Instance.RechargeLabel.text = "RECHARGING GUN";
						//GameManager.Instance.RechargeLabel.color = Color.red;
						GameManager.Instance.RechargeLabel.enabled = true;
					}
				}
            }
        }

    }

    private GameObject getTargetEnemy()
    {
        GameObject activeEnemiesGroup = GameManager.Instance.GetActiveEnemiesGroup(position);
        GameObject targetEnemy = null;
        float minZ = float.MaxValue;
        for (int i = 0; i < activeEnemiesGroup.transform.childCount; i++)
        {
            GameObject e = activeEnemiesGroup.transform.GetChild(i).gameObject;
			if (e.transform.localPosition.z < minZ && e.GetComponent<EnemyScript>().enabled)
            {
				minZ = e.transform.localPosition.z;
                targetEnemy = e;
            }
        }
        return targetEnemy;
    }

    
    public void MoveLeft()
    {
        if (!rotating)
        {
            StartCoroutine(Rotate(-90f));
            //position = (position + 3) % 4;
            position = (PlayerPosition)(((int)position + 3) % 4);
        }
    }

    public void MoveRight()
    {
        if (!rotating)
        {
            StartCoroutine(Rotate(90f));
            position = (PlayerPosition)(((int)position + 1) % 4);
        }
    }

    public void Shoot()
    {

    }

    IEnumerator Rotate(float degrees)
    {
        rotating = true;
        Quaternion fromRotation = transform.rotation;
        Quaternion toRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, degrees, 0));

        for (var t = 0f; t < 1; t += Time.deltaTime / rotationTime)
        {
            transform.rotation = Quaternion.Slerp(fromRotation, toRotation, t);
            yield return null;
        }
        transform.rotation = toRotation;
        rotating = false;
    }
}
