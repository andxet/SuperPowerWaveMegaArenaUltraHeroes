using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ControllerKeys { SQUARE, X, CIRCLE, TRIANGLE }

public class AttackManager : MonoBehaviour
{
    List<ControllerKeys> sequence = new List<ControllerKeys>(); //Contains the current button to press or a sequence
    [Tooltip("SQUARE, X, CIRCLE, TRIANGLE")]
    public List<GameObject> buttonImages = new List<GameObject>();
    List<GameObject> guiObjects = new List<GameObject>();
    public GameObject guiContainer;
    bool isSequence = false;
    int numKeys;

    // Use this for initialization
    void Start()
    {
        numKeys = Enum.GetNames(typeof(ControllerKeys)).Length;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewKey()
    {
        isSequence = false;
        sequence.Clear();
        ControllerKeys newKey = (ControllerKeys)UnityEngine.Random.Range(0, numKeys);
        sequence.Add(newKey);
        foreach (GameObject obj in guiObjects)
            Destroy(obj);
        guiObjects.Clear();
        GameObject button = Instantiate(buttonImages[(int)newKey]);
        button.transform.SetParent(guiContainer.transform);

    }

    public void newSequence(int numButtons = 5)
    {
        isSequence = true;
        sequence.Clear();
        for (int i = 0; i < numButtons; i++)
            sequence.Add((ControllerKeys)UnityEngine.Random.Range(0, numKeys));
    }

    public bool Attack(ControllerKeys key)
    {
        if (sequence.Count == 0)
            return true;
        if (isSequence)
        {
            if (sequence[0] == key)
            {
                sequence.RemoveAt(0);
                if (sequence.Count == 0)
                {
                    GameManagerScript.Instance.NotifyEndSequence(gameObject);
                }
                return true;
            }
            else
                return false;
        }
        else
            return sequence[0] == key;
    }


}
