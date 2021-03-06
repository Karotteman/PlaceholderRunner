﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public LevelManager levelManager;
    public PlayerMouvement playerScript;
    public AudioSource mainTheme;
    public int level;

    bool gameIsOver = false;
    int previousScore = 0;
    float timer = 0;
    public int Score { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(Score%25 == 0 && Score != previousScore)
        {
            level++;
            levelManager.maxDanger += 5;
            previousScore = Score;
            if(level == 4)
            {
                levelManager.SwitchMaterial();
            }
        }

        if(uiManager != null)
        {
            uiManager.ScoreUpdate(
                Score, 
                Math.Round(timer, 2).ToString(),
                level
                );
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
        else if (SceneManager.GetActiveScene().name == "TitleScreen" && Input.GetButtonDown("Jump")
            || gameIsOver && Input.GetButtonDown("Jump"))
        {
;           SceneManager.LoadScene("Runner");
        }
    }

    public void GameOver()
    {
        mainTheme.Stop();
        uiManager.DisplayDeathScreen(Score);
        gameIsOver = true;
        playerScript.enabled = false;
    }
}
