using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Start") > 0.1)
		{
			Debug.Log("pressed start");            
			StartGame ();
		}
	}

    public void StartGame()
    {
		MultiMatchController.NewGame ();
        SceneManager.LoadScene("Stage");
    }
		
}
