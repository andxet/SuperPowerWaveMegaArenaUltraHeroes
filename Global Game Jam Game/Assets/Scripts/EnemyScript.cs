using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { TYPE1, TYPE2, TYPE3, TYPE4 };

public class EnemyScript : MonoBehaviour
{
    public float speed;
    public EnemyType enemyType;
    public int pointsWhenDie = 100;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * (speed * Time.deltaTime));

        if (transform.localPosition.z < 0f)
        {
            //TODO: gameover or damage
        }
    }

    internal void Die()
    {
        throw new NotImplementedException();
    }

    internal void Hit(EnemyType type)
    {
        if (type == enemyType)
        {
            enabled = false;
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            if (type == EnemyType.TYPE1)
                GetComponent<Rigidbody>().AddExplosionForce(2000f, Vector3.zero, 1000, 10f);
            else if (type == EnemyType.TYPE2)
                GetComponent<Rigidbody>().AddForceAtPosition(Vector3.back * 300f, new Vector3(transform.position.x, 0, transform.position.z), ForceMode.Force);
            else if(type == EnemyType.TYPE3)
                GetComponent<Rigidbody>().AddExplosionForce(2000f, Vector3.zero, 1000, 10f);
            else if (type == EnemyType.TYPE4)
                GetComponent<Rigidbody>().AddForceAtPosition(Vector3.back * 300f, new Vector3(transform.position.x, 0, transform.position.z), ForceMode.Force);
            GameManager.Instance.AddPoints(pointsWhenDie);
            StartCoroutine(Vanish());
        }
    }


    IEnumerator Vanish()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    /*void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance.MonsterCollidedHouse();
    }*/
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "House")
        GameManager.Instance.MonsterCollidedHouse();
    }
}
