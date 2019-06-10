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
    private float chance = 1;

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
        currentHoles += 1;
        chance -= 0.1f;
        GenerateLevel(chance, currentHoles,0.5f,5);
    }

    private void GenerateLevel(float chance, int maxHoles, float coinsChance, int maxCoins)
    {
        ResetLevel();
        GenerateCoins(coinsChance, maxCoins);
        if (chance > 1)
            chance = chance / 10f;

        int amountofHoles = 0;
        List<Section> openHoles = new List<Section>();

        for (int i = 0; i < sections.Count; i++)
        {
            if (i > 0 && amountofHoles <= maxHoles && sections[i - 1].go.activeInHierarchy && Random.Range(0,10) >= chance * 10 ||
                i == 0 && amountofHoles <= maxHoles && Random.Range(0, 10) >= chance * 10 ||
                i == sections.Count - 1 && openHoles.Count == 0)
            {
                sections[i].go.SetActive(false);
                amountofHoles++;
                openHoles.Add(sections[i]);
            }
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
    private void GenerateCoins(float coinsChance, int maxCoins)
    {
        int coinsCount = 0;
        coins = new List<GameObject>();
        foreach (Transform spawn in coinSpawns)
        {
            if (coinsCount <= maxCoins && Random.Range(0, 10) >= coinsChance)
            {
                GameObject lastCoin = Instantiate(coinPrefab);
                lastCoin.GetComponent<Collectable>().gm = this;
                lastCoin.transform.position = spawn.position;
                coins.Add(lastCoin);
                coinsCount++;
            }
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
