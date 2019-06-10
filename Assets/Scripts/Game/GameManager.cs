using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Session Info")]
    [SerializeField]
    private int score;
    [SerializeField]
    private float currentTime;

    [Header("Settings")]
    [SerializeField]
    private int collectableScore;

    [Header("Objects")]
    [SerializeField]
    private TextMeshProUGUI text;

    private bool timerRunning;
    [SerializeField]
    private bool disableTimer;

    private void Start()
    {
        currentTime = 5;
        text.text = currentTime + "\nScore:" + score;
        timerRunning = true;
    }

    private void FixedUpdate()
    {
        if (timerRunning && !disableTimer)
        {
            currentTime -= Time.fixedDeltaTime;
            if (currentTime <= 0)
            {
                KillPlayer();
                return;
            }
            text.text = Math.Round(currentTime,2) + "\nScore:" + score;
        }
            

    }

    public void AddScore()
    {
        score += collectableScore;
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene(0);
    }
}
