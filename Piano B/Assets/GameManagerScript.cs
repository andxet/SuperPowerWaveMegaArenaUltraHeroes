using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {
	public GameObject Onda1;
	public GameObject Onda2;
	public float ondaSpeed;
	public GameObject endGamePanel;
	public Text countdownText;
	public Text winnerText;

	private int phase = 0;
	private bool player1Started;
	private bool player2Started;

	// Use this for initialization
	void Start () {
		endGamePanel.SetActive (false);
		StartCoroutine (Countdown ());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (phase == 1 && Onda1.transform.GetChild(0).GetComponent<OndaScript> ().collided) {
			phase = 2;
		}

		if (phase == 0) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				WinPlayer2 ();
			} else if (Input.GetKeyDown (KeyCode.P)) {
				WinPlayer1 ();
			}
		} else if (phase == 1) {
			Debug.Log (phase);
			if (player1Started || Input.GetKeyDown (KeyCode.Q)) {
				player1Started = true;
				Vector3 s = Onda1.transform.localScale;
				s.y += (ondaSpeed * Time.deltaTime);
				Onda1.transform.localScale = s;
			} 
			if (player2Started || Input.GetKeyDown (KeyCode.P)) {
				player2Started = true;
				Vector3 s = Onda2.transform.localScale;
				s.y -= (ondaSpeed * Time.deltaTime);
				Onda2.transform.localScale = s;
			} 
		} else if (phase == 2) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				Vector3 s = Onda1.transform.localScale;
				s.y += 0.2f;
				Onda1.transform.localScale = s;
				s = Onda2.transform.localScale;
				s.y += 0.2f;
				Onda2.transform.localScale = s;
			}
			if (Input.GetKeyDown (KeyCode.P)) {
				Vector3 s = Onda2.transform.localScale;
				s.y -= 0.2f;
				Onda2.transform.localScale = s;
				s = Onda1.transform.localScale;
				s.y -= 0.2f;
				Onda1.transform.localScale = s;
			}

			if (Onda1.transform.localScale.y < 0) {
				WinPlayer2 ();
			} else if (Onda2.transform.localScale.y > 0) {
				WinPlayer1 ();
			}
		}
	}

	IEnumerator Countdown() {
		int count = 3;
		do {
			countdownText.text = count.ToString();
			count--;
			yield return new WaitForSeconds(1);
		} while (count > 0);
		phase = 1;
		countdownText.enabled = false;
	}

	private void WinPlayer1 () {
		phase = 3;
		winnerText.text = "Player 1 win!";
		endGamePanel.SetActive (true);
		countdownText.enabled = false;
	}

	private void WinPlayer2 () {
		phase = 3;
		winnerText.text = "Player 2 win!";
		endGamePanel.SetActive (true);
		countdownText.enabled = false;
	}

	public void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

}
