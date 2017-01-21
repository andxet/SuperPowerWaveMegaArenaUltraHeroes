using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public string prefix;
    private Player player;
    bool pressed;

    // Use this for initialization
    void Start()
    {
        if (prefix == null)
            Debug.Log("Input manager don'have prefix!!");
        player = GetComponent<Player>();
        if (player == null)
            Debug.Log("Input manager: player object don'have player script!!");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!pressed)
        {
            for (int i = 0; i < 4; i++)
                if (Input.GetAxis(prefix + "Fire" + i) > 0.1)
                {
                    Debug.Log("pressed axis "+ prefix + " " + Enum.GetName(typeof(ControllerKeys), (ControllerKeys)i));
                    pressed = true;
                    player.Attack((ControllerKeys)i);
                }
        }
        else
        {
            pressed = false;
            for (int i = 0; i < 4; i++)
                if (Input.GetAxis(prefix + "Fire" + i) > 0.1)
                {
                    pressed = true;
                    break;
                }
        }
    }
}
