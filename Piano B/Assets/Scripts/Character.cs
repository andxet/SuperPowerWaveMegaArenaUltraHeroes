using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {
    public string Name;
    public Image MenuThumbnail;
    public GameObject CharacterMesh;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Select()
    {
        CharacterInfoController.Instance.SelectPG(this, PlayerPosition.LEFT);
    }
}
