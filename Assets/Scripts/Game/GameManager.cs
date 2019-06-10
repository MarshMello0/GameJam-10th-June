using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Session Info")]
    [SerializeField]
    private int score;
    [SerializeField]
    private float currentTime;

    private int currentHoles;
    private int coinCount;

    [Header("Settings")]
    [SerializeField]
    private int collectableScore;

    [Header("Objects")]
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private List<Section> sections = new List<Section>();
    [SerializeField]
    private Transform[] coinSpawns;
    [SerializeField]
    private GameObject killPrefab, finishPrefab, coinPrefab;
    [SerializeField]
    private Transform spawnLeft,SpawnRight, player;


    private bool timerRunning;
    private List<GameObject> finishes;
    private List<GameObject> coins = new List<GameObject>();
    [SerializeField]
    private bool disableTimer;

    private void Start()
    {
        StartNewLevel();
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

    public void StartNewLevel()
    {
        
        player.transform.position = Random.Range(0,10) >= 5 ? spawnLeft.transform.position : SpawnRight.transform.position;
        currentTime = 5;
        text.text = currentTime + "\nScore:" + score;
        timerRunning = true;

        //Changing the variables for the next level
        currentHoles += 1;
        coinCount += 1;

        GenerateLevel(currentHoles,coinCount);
    }

    private void GenerateLevel(int maxHoles, int maxCoins)
    {
        ResetLevel();
        GenerateCoins(maxCoins);

        int amountofHoles = Random.Range(0,maxHoles);
        List<Section> openHoles = new List<Section>(amountofHoles);

        for (int i = 0; i <= amountofHoles; i++)
        {
            int sectionIndex = Random.Range(0, sections.Count - 1);
            sections[sectionIndex].go.SetActive(false);
            openHoles.Add(sections[sectionIndex]);
        }

        //This is the correct whole which has the finish flag
        int correctHole = Random.Range(0, openHoles.Count - 1);
        finishes = new List<GameObject>(openHoles.Count);

        for (int i = 0; i < openHoles.Count; i++)
        {
            GameObject bottom = Instantiate(i == correctHole ? finishPrefab : killPrefab);
            bottom.transform.position = openHoles[i].bottomPoint.position;
            ObjectCollision oc = bottom.AddComponent<ObjectCollision>();
            oc.isFinish = i == correctHole;
            oc.gm = this;
            finishes.Add(bottom);
        }
        
    }
    private void GenerateCoins(int maxCoins)
    {
        int coinsCount = Random.Range(0,maxCoins);
        coins = new List<GameObject>(coinCount);
        for (int i = 0; i <= coinsCount; i++)
        {
            GameObject lastCoin = Instantiate(coinPrefab);
            lastCoin.GetComponent<Collectable>().gm = this;
            lastCoin.transform.position = coinSpawns[Random.Range(0,coinSpawns.Length - 1)].position;
            coins.Add(lastCoin);
        }
    }
    private void ResetLevel()
    {
        foreach (Section section in sections)
        {
            section.go.SetActive(true);
        }


        if (finishes != null)
        {
            for (int i = 0; i < finishes.Count; i++)
            {
                Destroy(finishes[i]);
            }
        }

        for (int i = 0; i < coins.Count; i++)
        {
            Destroy(coins[i]);
        }

    }
}
[Serializable]
public class Section
{
    public GameObject go;
    public BoxCollider2D col;
    public Transform bottomPoint;
}
