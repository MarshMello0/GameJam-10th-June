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

    [Header("Settings")]
    [SerializeField]
    private int collectableScore;

    [Header("Objects")]
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private List<Section> sections = new List<Section>();
    [SerializeField]
    private GameObject killPrefab, finishPrefab;
    [SerializeField]
    private Transform spawnLeft,SpawnRight, player;


    private bool timerRunning;
    private List<GameObject> finishes;
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

    void OnGUI()
    {
        if (GUI.Button(new Rect(0,0,100,100),"Generate"))
        {
            GenerateLevel(0.5f, 5);
        }
    }

    public void StartNewLevel()
    {
        
        player.transform.position = Random.Range(0,10) >= 5? spawnLeft.transform.position : SpawnRight.transform.position;
        currentTime = 5;
        text.text = currentTime + "\nScore:" + score;
        timerRunning = true;
        GenerateLevel(0.5f, 9);
    }

    private void GenerateLevel(float chance, int maxHoles)
    {
        ResetLevel();

        if (chance > 1)
            chance = chance / 10f;

        int amountofHoles = 0;
        List<Section> openHoles = new List<Section>();

        for (int i = 0; i < sections.Count; i++)
        {
            if (i > 0 && amountofHoles <= maxHoles && sections[i - 1].go.activeInHierarchy && Random.Range(0,10) >= chance * 10 ||
                i == 0 && amountofHoles <= maxHoles && Random.Range(0, 10) >= chance * 10)
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

    }
}
[Serializable]
public class Section
{
    public GameObject go;
    public BoxCollider2D col;
    public Transform bottomPoint;
}
