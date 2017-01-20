using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int windows = 4;
    private float life;
    float rotationTime = 0.1f;
    bool rotating = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveLeft()
    {
        if (!rotating)
            StartCoroutine(Rotate(-90f));
    }

    public void MoveRight()
    {
        if (!rotating)
            StartCoroutine(Rotate(90f));
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
