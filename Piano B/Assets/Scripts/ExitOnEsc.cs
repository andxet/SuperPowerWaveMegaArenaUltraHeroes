﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitOnEsc : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Exit();
	}

    public void Exit()
    {
        Application.Quit();
    }
}
