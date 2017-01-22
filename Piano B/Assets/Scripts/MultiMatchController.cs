using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMatchController : MonoBehaviour
{
    public static int Player1Wins, Player2Wins = 0;
	public static int round = 0;

    // Use this for initialization
	/*
    void Start()
    {
        DontDestroyOnLoad(this);
    }
    */

	public static void NewGame() {
		round = 0;
		Player1Wins = 0;
		Player2Wins = 0;
	}

    public static bool Win(PlayerPosition player)
    {
		round++;
        if (player == PlayerPosition.LEFT)
            Player1Wins++;
        else
            Player2Wins++;
        if (Player1Wins == 2 || Player2Wins == 2)
            return true;
        else
            return false;
    }
}
