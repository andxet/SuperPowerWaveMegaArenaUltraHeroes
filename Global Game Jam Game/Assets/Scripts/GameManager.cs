using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
    int points;
    float difficulty;
    float playerLife;
    public GameObject GameOverPanel;
    int spawnsEnabled = 1;
    public GameObject[] spawns = new GameObject[4];
    GameObject player;
    public int spawnActiveTime = 40;
    float startTime;
    public float EnemyHurtDamage = 0.33334f;


	// Use this for initialization
	void Start () {
        playerLife = 1;
        if (GameOverPanel == null)
            Debug.Log("The gameover panel is null");
        else
        GameOverPanel.SetActive(false);
        for (int i = 1; i < spawns.Length; i++)
            spawns[i].SetActive(false);
        startTime = Time.timeSinceLevelLoad;
    }

    public void RegisterPlayer(GameObject gameObject)
    {
        player = gameObject;
    }

    // Update is called once per frame
    void Update () {
        if (Time.timeSinceLevelLoad - startTime > spawnActiveTime * spawnsEnabled)
            EnableSpawn();
	}

    public void AddPoints(int points)
    {
        this.points += points;
    }

    public void MonsterCollidedHouse()
    {
        playerLife -= EnemyHurtDamage;
        if (playerLife < 0)
            GameOver();
    }

    public void GameOver()
    {
        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);
    }

    public void EnableSpawn()
    {
        if (spawnsEnabled >= spawns.Length)
            return;
        spawns[spawnsEnabled++].SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    internal GameObject GetActiveEnemiesGroup(PlayerPosition position)
    {
        return spawns[(int)position];
    }
}
