using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScript : MonoBehaviour
{
    public Image LogoImage;
    public float fadeInTime = 1;
    public float showTime = 3;
    public float fadeOutTime = 1;

	// Use this for initialization
	void Start () {
        StartCoroutine(DoMenu());
	}

    IEnumerator DoMenu()
    {
        LogoImage.canvasRenderer.SetAlpha(0.01f);
        LogoImage.CrossFadeAlpha(1, fadeInTime, false);
        yield return new WaitForSeconds(fadeInTime + showTime);
        LogoImage.canvasRenderer.SetAlpha(1f);
        LogoImage.CrossFadeAlpha(0, fadeOutTime, false);
        yield return new WaitForSeconds(fadeOutTime + 0.1f);
        SceneManager.LoadScene("MainScreen");
    }
}
