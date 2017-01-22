using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUIController : MonoBehaviour {
    public GameObject firstRoundImage;
    public GameObject secondRoundImage;
    public PlayerPosition player;

    // Use this for initialization
    void Start () {
        Refresh();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Refresh()
    {
        if (player == PlayerPosition.LEFT)
        {
            firstRoundImage.SetActive(MultiMatchController.Player1Wins >= 1);
            secondRoundImage.SetActive(MultiMatchController.Player1Wins >= 2);
        }
        else if (player == PlayerPosition.RIGHT)
        {
            firstRoundImage.SetActive(MultiMatchController.Player2Wins >= 1);
            secondRoundImage.SetActive(MultiMatchController.Player2Wins >= 2);
        }
    }
}
