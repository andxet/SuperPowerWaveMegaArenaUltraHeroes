using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    int points;
    float difficulty;
    float playerLife;
    public GameObject GameOverPanel;
    public GameObject NewSpawnPanel;
    public Text GameStats;
    int spawnsEnabled = 1;
    public GameObject[] spawns = new GameObject[4];
    GameObject player;
    public int spawnActiveTime = 40;
    float startTime;
    public float EnemyHurtDamage = 0.33334f;


    // Use this for initialization
    void Start()
    {
        playerLife = 1;
        if (GameOverPanel == null)
            Debug.Log("The gameover panel is null");
        else
            GameOverPanel.SetActive(false);
        if (NewSpawnPanel == null)
            Debug.Log("The spawn panel is null");
        else
            NewSpawnPanel.SetActive(false);
        if (GameStats == null)
            Debug.Log("The points panel is null");
        for (int i = 1; i < spawns.Length; i++)
            spawns[i].SetActive(false);
        startTime = Time.timeSinceLevelLoad;
    }

    public void RegisterPlayer(GameObject gameObject)
    {
        player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad - startTime > spawnActiveTime * spawnsEnabled)
            EnableSpawn();
        GameStats.text = "Life: " + playerLife + "\nPoints: " + points;

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
        StartCoroutine(FlashSpawnPanel());
    }

    IEnumerator FlashSpawnPanel()
    {
        bool active = true;
        for (int i = 0; i < 8; i++)
        {
            NewSpawnPanel.SetActive(active);
            active = !active;
            yield return new WaitForSeconds(0.5f);
        }
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
