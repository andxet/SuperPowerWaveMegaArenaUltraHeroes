using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoController : Singleton<CharacterInfoController> {
    Character player1;
    Character player2;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectPG(Character character, PlayerPosition player)
    {
        if (player == PlayerPosition.LEFT)
            player1 = character;
        else
            player2 = character;

    }

    public bool AllPlayerHaveChooseACharacter()
    {
        return player1 != null && player2 != null;
    }
}
