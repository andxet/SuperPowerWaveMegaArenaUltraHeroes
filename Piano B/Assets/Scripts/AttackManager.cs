using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ControllerKeys { SQUARE, X, CIRCLE, TRIANGLE, R }

public class AttackManager : MonoBehaviour
{
    List<ControllerKeys> sequence = new List<ControllerKeys>(); //Contains the current button to press or a sequence
    List<ControllerKeys> lastSequence;
	ControllerKeys lastKey;
    [Tooltip("SQUARE, X, CIRCLE, TRIANGLE, R")]
    public List<GameObject> buttonImages = new List<GameObject>();
    List<GameObject> guiObjects = new List<GameObject>();
    public GameObject guiContainer;
    bool isSequence = false;
    int numKeys;
    private bool endGame = false;

    // Use this for initialization
    void Start()
    {
        numKeys = Enum.GetNames(typeof(ControllerKeys)).Length;
		lastKey = (ControllerKeys)UnityEngine.Random.Range (0, numKeys);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewKey()
    {
        isSequence = false;
        sequence.Clear();
		ControllerKeys newKey;
		do {
			newKey = (ControllerKeys)UnityEngine.Random.Range (0, numKeys);
		} while (newKey == lastKey);
		lastKey = newKey;
        sequence.Add(newKey);
        foreach (GameObject obj in guiObjects)
            Destroy(obj);
        guiObjects.Clear();
        GameObject button = Instantiate(buttonImages[(int)newKey]);
        button.transform.SetParent(guiContainer.transform);
        button.transform.localScale = Vector3.one;
        guiObjects.Add(button);
        button.GetComponent<Animation>().Play();
    }

    public void newSequence(int numButtons = 5)
    {
        sequence = new List<ControllerKeys>();
        for (int i = 0; i < numButtons; i++)
            sequence.Add((ControllerKeys)UnityEngine.Random.Range(0, numKeys));
        newSequence(sequence);
    }

    public void newSequence(List<ControllerKeys> sequence)
    {
        isSequence = true;
        this.sequence = sequence;
        lastSequence = new List<ControllerKeys>(sequence);
        foreach (GameObject obj in guiObjects)
            Destroy(obj);
        guiObjects.Clear();
        foreach (ControllerKeys key in sequence)
        {
            GameObject button = Instantiate(buttonImages[(int)key]);
            button.transform.SetParent(guiContainer.transform);
            button.transform.SetSiblingIndex(0);
            button.transform.localScale = Vector3.one;
            guiObjects.Add(button);
        }
    }

    public bool Attack(ControllerKeys key)
    {
        if (endGame)
            return false;
        if (sequence.Count == 0)
            return true;
        if (isSequence)
        {
            if (sequence[0] == key)
            {
                sequence.RemoveAt(0);
                Destroy(guiObjects[0]);
                guiObjects.RemoveAt(0);
                if (sequence.Count == 0)
                {
                    GameManagerScript.Instance.NotifyEndSequence(gameObject.GetComponent<Player>().position);
                }
                return true;
            }
            else
            {
                newSequence(lastSequence);
                return false;
            }
        }
        else
            return sequence[0] == key;
    }

    public void EndGame()
    {
        endGame = true;
        foreach (GameObject obj in guiObjects)
            Destroy(obj);
        guiObjects.Clear();
        guiContainer.SetActive(false);
    }
}
