using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMatchController : MonoBehaviour
{
    public int Player1Wins, Player2Wins = 0;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    bool Win(PlayerPosition player)
    {
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
