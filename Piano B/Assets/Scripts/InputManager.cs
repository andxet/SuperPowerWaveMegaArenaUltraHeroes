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
					ResetStickRotation ();
                }
			if (CheckStickRotation ())
				player.Attack (ControllerKeys.R);
            if (Input.GetAxis("Start") > 0.1)
            {
                Debug.Log("pressed start");            
                GameManagerScript.Instance.Restart();
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

	private bool[] stickBits = new bool[4];

	private bool CheckStickRotation () {
		//Debug.Log ("CheckStickRotation before" + stickBits.ToString());
		if (Input.GetAxis (prefix + "Horizontal") > 0.5) {
			stickBits [0] = true;
			//Debug.Log ("CheckStickRotation 0");
		}
		if (Input.GetAxis (prefix + "Horizontal") < -0.5) {
			stickBits [1] = true;
			//Debug.Log ("CheckStickRotation 1");
		}
		if (Input.GetAxis (prefix + "Vertical") > 0.5) {
			stickBits [2] = true;
			//Debug.Log ("CheckStickRotation 2");
		}
		if (Input.GetAxis (prefix + "Vertical") < -0.5) {
			stickBits [3] = true;
			//Debug.Log ("CheckStickRotation 3");
		}

		bool complete = true;
		foreach (bool b in stickBits) {
			if (!b) {
				complete = false;
				break;
			}
		}

		if (complete) {
			Debug.Log ("stick rotation complete");
			ResetStickRotation ();
			return true;
		} else
			return false;
	}

	private void ResetStickRotation () {
		for (int i = 0; i < 4; i++)
			stickBits [i] = false;
	}
}
