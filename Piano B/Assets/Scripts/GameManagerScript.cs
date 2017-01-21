using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public enum GamePhase { COUTNDOWN, LAUNCH, BATTLE, FINAL_DUEL, END }

public class GameManagerScript : Singleton<GameManagerScript>
{
    public GameObject Onda1;
    public GameObject Onda2;
    public float ondaSpeed;

    public GameObject endGamePanel;
    public GameObject Player1;
    public GameObject Player2;
    public Text countdownText;
    public Text winnerText;

    private GamePhase phase = GamePhase.COUTNDOWN;
    private bool player1Started;
    private bool player2Started;
    public int buttonChangeInterval = 3;

    void Start()
    {
        endGamePanel.SetActive(false);
        StartCoroutine(Countdown());
    }

    internal void Attack(PlayerPosition position)
    {
        if (phase == GamePhase.COUTNDOWN)
        {
            if (position == PlayerPosition.LEFT)
            {
                Win("Player 2 Wins!!");
            }
            else if (position == PlayerPosition.RIGHT)
            {
                Win("Player 1 Wins!!");
            }
        }
        else if (phase == GamePhase.LAUNCH)
        {
            //Debug.Log (phase);
            if (!player1Started && position == PlayerPosition.LEFT)
            {
                player1Started = true;
                StartCoroutine(LaunchWave(Onda1, ondaSpeed));
            }
            if (!player2Started && position == PlayerPosition.RIGHT)
            {
                player2Started = true;
                StartCoroutine(LaunchWave(Onda2, -ondaSpeed));
            }
        }
        else if (phase == GamePhase.BATTLE)
        {
            if (position == PlayerPosition.LEFT)
            {
                Vector3 s = Onda1.transform.localScale;
                s.y += 0.2f;
                Onda1.transform.localScale = s;
                s = Onda2.transform.localScale;
                s.y += 0.2f;
                Onda2.transform.localScale = s;
            }
            if (position == PlayerPosition.RIGHT)
            {
                Vector3 s = Onda2.transform.localScale;
                s.y -= 0.2f;
                Onda2.transform.localScale = s;
                s = Onda1.transform.localScale;
                s.y -= 0.2f;
                Onda1.transform.localScale = s;
            }

            if (Onda1.transform.localScale.y < 0)
            {
                //Win("Player 2 Wins!!");
                Player1.GetComponent<AttackManager>().newSequence(5);
                Player2.GetComponent<AttackManager>().newSequence(5);
                phase = GamePhase.FINAL_DUEL;
            }
            else if (Onda2.transform.localScale.y > 0)
            {
                //Win("Player 1 Wins!!");
                Player1.GetComponent<AttackManager>().newSequence(5);
                Player2.GetComponent<AttackManager>().newSequence(5);
                phase = GamePhase.FINAL_DUEL;
            }
        } 
           
    }

    public void RegisterPlayer(GameObject go, PlayerPosition playerPosition)
    {
        if (playerPosition == PlayerPosition.LEFT)
            Player1 = go;
        else
            Player2 = go;
    }

    public void WavesCollided()
    {
        if (phase == GamePhase.LAUNCH)
        {
            phase = GamePhase.BATTLE;
            //Player1.GetComponent<AttackManager>().NewKey();
            //Player2.GetComponent<AttackManager>().NewKey();
            RefreshButton();
        }
    }

    internal void NotifyEndSequence(PlayerPosition position)
    {
        if (position == PlayerPosition.LEFT)
        {
            if (Onda1.transform.localScale.y < 0)
            {
                Vector3 s = Onda1.transform.localScale;
                s.y += 1f;
                Onda1.transform.localScale = s;
                s = Onda2.transform.localScale;
                s.y += 1f;
                Onda2.transform.localScale = s;

                phase = GamePhase.BATTLE;
                Player1.GetComponent<AttackManager>().NewKey();
                Player2.GetComponent<AttackManager>().NewKey();
            }
            else if (Onda2.transform.localScale.y > 0)
            {
                Win("Player 1 Wins!!");
            }
        }
        else if (position == PlayerPosition.RIGHT)
        {
            if (Onda1.transform.localScale.y < 0)
            {
                Win("Player 2 Wins!!");
            }
            else if (Onda2.transform.localScale.y > 0)
            {
                Vector3 s = Onda2.transform.localScale;
                s.y -= 1f;
                Onda2.transform.localScale = s;
                s = Onda1.transform.localScale;
                s.y -= 1f;
                Onda1.transform.localScale = s;

                phase = GamePhase.BATTLE;
                Player1.GetComponent<AttackManager>().NewKey();
                Player2.GetComponent<AttackManager>().NewKey();
            }
        }
    }

    internal void NotifyWrongSequence(PlayerPosition position)
    {
        if (position == PlayerPosition.LEFT)
        {
            NotifyEndSequence(PlayerPosition.RIGHT);
        }
        else if (position == PlayerPosition.RIGHT)
        {
            NotifyEndSequence(PlayerPosition.LEFT);
        }
    }
    IEnumerator Countdown()
    {
        int count = 3;
        do
        {
            countdownText.text = count.ToString();
            count--;
            yield return new WaitForSeconds(1);
        } while (count > 0);
        phase = GamePhase.LAUNCH;
        countdownText.enabled = false;
    }

    IEnumerator LaunchWave(GameObject wave, float waveSpeed)
    {
        while (phase == GamePhase.LAUNCH)
        {
            Vector3 s = wave.transform.localScale;
            s.y += (waveSpeed * Time.deltaTime);
            wave.transform.localScale = s;
            yield return null;
        }
    }

    public void RefreshButton()
    {
        if(phase == GamePhase.BATTLE)
        {
            Player1.GetComponent<AttackManager>().NewKey();
            Player2.GetComponent<AttackManager>().NewKey();
        }
        Invoke("RefreshButton", buttonChangeInterval);
    }

    private void Win(string message)
    {
        phase = GamePhase.END;
        winnerText.text = message;
        endGamePanel.SetActive(true);
        countdownText.enabled = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
