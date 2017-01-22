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
    public GameObject BlastFX;
    public CharacterUIController Player1UI;
    public CharacterUIController Player2UI;


    public Text countdownText;
    public Text winnerText;
    public GameObject[] bkgPlanes;
	public AudioSource audioSource;
	public AudioSource waveAudioSource;
	public AudioClip victoryAudio;
	public AudioClip countDownAudio;
	public AudioClip enter1Audio;
	public AudioClip enter2Audio;
	public AudioClip die1Audio;
	public AudioClip die2Audio;
	public AudioClip waveAudio;
	public AudioClip pioAudio;
	public AudioClip win1Audio;
	public AudioClip win2Audio;
	public AudioClip bkgAudio;

    private GamePhase phase = GamePhase.COUTNDOWN;
    private bool player1Started;
    private bool player2Started;
    public int buttonChangeInterval = 3;

    private Vector3 wave1TargetPos;
    private Vector3 wave2TargetPos;

    private bool player1FalseStart;
    private bool player2FalseStart;

    private float initialCamZ;

    void Start()
    {
        initialCamZ = Camera.main.transform.position.z;
        endGamePanel.SetActive(false);
        StartCoroutine(Countdown());
        CollisionFX.SetActive(false);
		for (int i = 0; i < bkgPlanes.Length; i++) {
			if (i == MultiMatchController.round % 2)
				bkgPlanes[i].SetActive (true);
			else
				bkgPlanes[i].SetActive (false);
		}
    }

    void Update()
    {
        if (phase == GamePhase.LAUNCH)
        {
            //Debug.Log (phase);
            if (!player1Started && IsPlayer1AlmostHit())
            {
                player1Started = true;
                StartCoroutine(LaunchWave(Onda1, ondaSpeed, PlayerPosition.LEFT));
            }
            if (!player2Started && IsPlayer2AlmostHit())
            {
                player2Started = true;
                StartCoroutine(LaunchWave(Onda2, ondaSpeed, PlayerPosition.RIGHT));
            }
        }
        else if (phase == GamePhase.BATTLE)
        {
            Onda1.transform.position = Vector3.MoveTowards(Onda1.transform.position, wave1TargetPos, 0.05f);
            Onda2.transform.position = Vector3.MoveTowards(Onda2.transform.position, wave2TargetPos, 0.05f);
            bool fastRecovery1 = Math.Abs(wave1TargetPos.x - Onda1.transform.position.x) > 0.7f;
            bool fastRecovery2 = Math.Abs(wave2TargetPos.x - Onda2.transform.position.x) > 0.7f;
            UpdateWaveFX(Onda1, fastRecovery1);
            UpdateWaveFX(Onda2, fastRecovery2);
        }

        /*
		Vector3 camPos = Camera.main.transform.position;
		camPos.x = (Onda1.transform.position.x + Onda2.transform.position.x) / 2f;
		camPos.z = initialCamZ + (0.4f * (Math.Abs (Math.Abs(Onda1.transform.localPosition.y) - Math.Abs(Onda2.transform.localPosition.y))));
		Camera.main.transform.position = camPos;
		*/
    }

    internal void Attack(PlayerPosition position)
    {
        if (phase == GamePhase.COUTNDOWN)
        {
            if (position == PlayerPosition.LEFT)
            {
                Player1.GetComponent<AttackManager>().NewEmptyKey();
                //Win(PlayerPosition.RIGHT);
                player1FalseStart = true;
                Player1.GetComponent<AttackManager>().Disable();
            }
            else if (position == PlayerPosition.RIGHT)
            {
                Player2.GetComponent<AttackManager>().NewEmptyKey();
                //Win(PlayerPosition.LEFT);
                player2FalseStart = true;
                Player2.GetComponent<AttackManager>().Disable();
            }
        }
        else if (phase == GamePhase.LAUNCH)
        {
            //Debug.Log (phase);
            if (!player1Started && position == PlayerPosition.LEFT)
            {
                player1Started = true;
                StartCoroutine(LaunchWave(Onda1, ondaSpeed, PlayerPosition.LEFT));
				if (!waveAudioSource.isPlaying)
					waveAudioSource.Play ();
            }
            if (!player2Started && position == PlayerPosition.RIGHT)
            {
                player2Started = true;
                StartCoroutine(LaunchWave(Onda2, ondaSpeed, PlayerPosition.RIGHT));
				if (!waveAudioSource.isPlaying)
					waveAudioSource.Play ();
            }
        }
        else if (phase == GamePhase.BATTLE)
        {
            if (position == PlayerPosition.LEFT)
            {
                //Vector3 s = Onda1.transform.position;
                //s.x += 0.2f;
                //Onda1.transform.position = s;
                wave1TargetPos.x += 0.2f; ;
                //s = Onda2.transform.position;
                //s.x += 0.2f;
                //Onda2.transform.position = s;
                wave2TargetPos.x += 0.2f; ;

                UpdateWaveFX(Onda1, false);
                UpdateWaveFX(Onda2, false);
            }
            if (position == PlayerPosition.RIGHT)
            {
                //Vector3 s = Onda2.transform.position;
                //s.x -= 0.2f;
                //Onda2.transform.position = s;
                wave2TargetPos.x -= 0.2f;
                //s = Onda1.transform.position;
                //s.x -= 0.2f;
                //Onda1.transform.position = s;
                wave1TargetPos.x -= 0.2f;

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
            wave1TargetPos = Onda1.transform.position;
            wave2TargetPos = Onda2.transform.position;
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

    private bool IsPlayer1AlmostHit()
    {
        return Math.Abs(Onda2.transform.localPosition.y) > 10;
    }

    private bool IsPlayer2AlmostHit()
    {
        return Math.Abs(Onda1.transform.localPosition.y) > 10;
    }

    internal void NotifyEndSequence(PlayerPosition position)
    {
        if (position == PlayerPosition.LEFT)
        {
            if (IsPlayer1Hit())
            {
                wave1TargetPos.x += 3f;
                wave2TargetPos.x += 3f;

                Vector3 p = Onda1.transform.position;
                p.x += 0.2f;
                Onda1.transform.position = p;

                UpdateWaveFX(Onda1, false);
                UpdateWaveFX(Onda2, false);

                phase = GamePhase.BATTLE;
                Player1.GetComponent<AttackManager>().NewKey();
                Player2.GetComponent<AttackManager>().NewKey();
            }
            else if (IsPlayer2Hit())
            {
                endController.Init(PlayerPosition.RIGHT);
                Win(PlayerPosition.LEFT);
            }
        }
        else if (position == PlayerPosition.RIGHT)
        {
            if (IsPlayer1Hit())
            {
                endController.Init(PlayerPosition.LEFT);
                Win(PlayerPosition.RIGHT);
            }
            else if (IsPlayer2Hit())
            {

                wave2TargetPos.x -= 3f;
                wave1TargetPos.x -= 3f;

                Vector3 p = Onda2.transform.position;
                p.x -= 0.2f;
                Onda2.transform.position = p;

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
        if (player1FalseStart && !player2FalseStart)
        {
            Player2.GetComponent<AttackManager>().NewKey(true);
            yield return new WaitForSeconds(1);
            Player1.GetComponent<AttackManager>().NewKey(true);
        }
        else if (player2FalseStart && !player1FalseStart)
        {
            Player1.GetComponent<AttackManager>().NewKey(true);
            yield return new WaitForSeconds(1);
            Player2.GetComponent<AttackManager>().NewKey(true);
        }
        else if (player2FalseStart && player1FalseStart)
        {
            yield return new WaitForSeconds(1);
            Player1.GetComponent<AttackManager>().NewKey(true);
            Player2.GetComponent<AttackManager>().NewKey(true);
        }
        else
        {
            Player1.GetComponent<AttackManager>().NewKey(true);
            Player2.GetComponent<AttackManager>().NewKey(true);
        }
    }

    IEnumerator LaunchWave(GameObject wave, float waveSpeed, PlayerPosition position)
    {
        GameObject fx;
        if (position == PlayerPosition.LEFT)
        {
            Player1.GetComponent<AttackManager>().NewEmptyKey();
            fx = Onda1.transform.GetChild(0).gameObject;
            Player1.GetComponent<Animator>().SetBool("attack", true);
        }
        else
        {
            Player2.GetComponent<AttackManager>().NewEmptyKey();
            fx = Onda2.transform.GetChild(0).gameObject;
            Player2.GetComponent<Animator>().SetBool("attack", true);
        }

        wave.transform.GetChild(0).gameObject.SetActive(true);

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
        //Debug.Log("UpdateWaveFX - p.y= " + p.y + " speed= " + fx_mm.startSpeedMultiplier);
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

    private void Win(PlayerPosition position)
    {
        phase = GamePhase.END;
        //winnerText.text = message;
        endGamePanel.SetActive(true);
        countdownText.enabled = false;
        Player1.GetComponent<AttackManager>().EndGame();
        Player2.GetComponent<AttackManager>().EndGame();
        endController.gameObject.SetActive(true);
        if (position == PlayerPosition.LEFT)
        {
            BlastFX.transform.position = Player2.transform.position;
            Onda2.transform.GetChild(0).gameObject.SetActive(false);
            Player2.GetComponent<Animator>().SetBool("death", true);
			audioSource.PlayOneShot (die2Audio);
			audioSource.PlayOneShot (win1Audio);
        }
        else
        {
            BlastFX.transform.position = Player1.transform.position;
            Onda1.transform.GetChild(0).gameObject.SetActive(false);
            Player1.GetComponent<Animator>().SetBool("death", true);
			audioSource.PlayOneShot (die1Audio);
			audioSource.PlayOneShot (win2Audio);
        }
        BlastFX.SetActive(true);
        MultiMatchController.Win(position);
        Player1UI.Refresh();
        Player2UI.Refresh();
		waveAudioSource.Stop ();
    }

	public void Restart()
    {
		PlayPio ();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	public void PlayPio() {
		audioSource.PlayOneShot (pioAudio);
	}

}
