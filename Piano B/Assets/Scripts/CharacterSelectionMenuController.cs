using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionMenuController : MonoBehaviour {
    public GameObject PressStartToPlayObject;

	// Use this for initialization
	void Start () {
        PressStartToPlayObject.SetActive(false);
    }

    public void EnablePressStartToPlayObject()
    {
        PressStartToPlayObject.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetAxis("Start") > 0.1 && CharacterInfoController.Instance.AllPlayerHaveChooseACharacter())
            SceneManager.LoadScene("LoadingScreen");
    }
}
