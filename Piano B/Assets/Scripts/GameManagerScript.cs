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
    public EndGamePanelController endController;

    public GameObject endGamePanel;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject CollisionFX;
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
        CollisionFX.SetActive(false);
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
                Onda1.transform.GetChild(0).gameObject.SetActive(true);
                StartCoroutine(LaunchWave(Onda1, ondaSpeed, PlayerPosition.LEFT));
            }
            if (!player2Started && position == PlayerPosition.RIGHT)
            {
                player2Started = true;
                Onda2.transform.GetChild(0).gameObject.SetActive(true);
                StartCoroutine(LaunchWave(Onda2, ondaSpeed, PlayerPosition.RIGHT));
            }
        }
        else if (phase == GamePhase.BATTLE)
        {
            if (position == PlayerPosition.LEFT)
            {
                Vector3 s = Onda1.transform.position;
                s.x += 0.2f;
                Onda1.transform.position = s;
                s = Onda2.transform.position;
                s.x += 0.2f;
                Onda2.transform.position = s;

                UpdateWaveFX(Onda1, false);
                UpdateWaveFX(Onda2, false);
            }
            if (position == PlayerPosition.RIGHT)
            {
                Vector3 s = Onda2.transform.position;
                s.x -= 0.2f;
                Onda2.transform.position = s;
                s = Onda1.transform.position;
                s.x -= 0.2f;
                Onda1.transform.position = s;

                UpdateWaveFX(Onda1, false);
                UpdateWaveFX(Onda2, false);
            }

            if (IsPlayer1Hit())
            {
                //Win("Player 2 Wins!!");
                Player1.GetComponent<AttackManager>().newSequence(5);
                Player2.GetComponent<AttackManager>().newSequence(5);
                phase = GamePhase.FINAL_DUEL;
            }
            else if (IsPlayer2Hit())
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
            CollisionFX.SetActive(true);
        }
    }

    private bool IsPlayer1Hit()
    {
        return Onda1.transform.localPosition.y > 0;
    }

    private bool IsPlayer2Hit()
    {
        return Onda2.transform.localPosition.y > 0;
    }

    internal void NotifyEndSequence(PlayerPosition position)
    {
        if (position == PlayerPosition.LEFT)
        {
            if (IsPlayer1Hit())
            {
                Vector3 s = Onda1.transform.position;
                s.x += 1f;
                Onda1.transform.position = s;
                s = Onda2.transform.position;
                s.x += 1f;
                Onda2.transform.position = s;

                UpdateWaveFX(Onda1, false);
                UpdateWaveFX(Onda2, false);

                phase = GamePhase.BATTLE;
                Player1.GetComponent<AttackManager>().NewKey();
                Player2.GetComponent<AttackManager>().NewKey();
            }
            else if (IsPlayer2Hit())
            {
                endController.Init(PlayerPosition.RIGHT);
                Win("Player 1 Wins!!");
            }
        }
        else if (position == PlayerPosition.RIGHT)
        {
            if (IsPlayer1Hit())
            {
                endController.Init(PlayerPosition.LEFT);
                Win("Player 2 Wins!!");
            }
            else if (IsPlayer2Hit())
            {
                Vector3 s = Onda2.transform.position;
                s.x -= 1f;
                Onda2.transform.position = s;
                s = Onda1.transform.position;
                s.x -= 1f;
                Onda1.transform.position = s;

                UpdateWaveFX(Onda1, false);
                UpdateWaveFX(Onda2, false);

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

    IEnumerator LaunchWave(GameObject wave, float waveSpeed, PlayerPosition position)
    {
        GameObject fx;
        if (position == PlayerPosition.LEFT)
        {
            fx = Onda1.transform.GetChild(0).gameObject;
        }
        else
        {
            fx = Onda2.transform.GetChild(0).gameObject;
        }

        fx.SetActive(true);
        while (phase == GamePhase.LAUNCH)
        {
            Vector3 s = wave.transform.localPosition;
            s.y += (waveSpeed * Time.deltaTime * -1);
            wave.transform.localPosition = s;
            UpdateWaveFX(wave, true);
            yield return null;
        }
        UpdateWaveFX(wave, false);
    }

    private void UpdateWaveFX(GameObject wave, bool launch)
    {
        GameObject fx = wave.transform.GetChild(0).gameObject;
        ParticleSystem.MainModule fx_mm = fx.GetComponent<ParticleSystem>().main;
        Vector3 p = wave.transform.localPosition;
        if (launch)
            fx_mm.startSpeedMultiplier = 1f + (-p.y * (5.5f - 1f) / 13f);
        else
            fx_mm.startSpeedMultiplier = -p.y * (6f / 13f);
        Debug.Log("UpdateWaveFX - p.y= " + p.y + " speed= " + fx_mm.startSpeedMultiplier);
    }

    public void RefreshButton()
    {
        if (phase == GamePhase.BATTLE)
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
        Player1.GetComponent<AttackManager>().EndGame();
        Player2.GetComponent<AttackManager>().EndGame();
        endController.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
