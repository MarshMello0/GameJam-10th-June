using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;
using Pixeye.Unity;
using Slider = UnityEngine.UI.Slider;

public class GameManager : MonoBehaviour
{
    [Foldout("Session Info")] [SerializeField]
    private int score;
    [Foldout("Session Info")] [SerializeField]
    private float currentTime;

    private int currentHoles;
    private int coinCount;

    [Foldout("Settings")] [SerializeField]
    private int collectableScore;

    [Header("Objects")]
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private List<Section> sections = new List<Section>();
    [SerializeField]
    private Transform[] coinSpawns;
    [SerializeField]
    private GameObject killPrefab, finishPrefab, coinPrefab, paintingPrefab;
    [SerializeField]
    private Transform spawnLeft,SpawnRight, player;
    [SerializeField] 
    private Slider timerSlider;
    [SerializeField] 
    private PlayerController playerController;
    [SerializeField] 
    private Animator animator;
    [SerializeField] 
    private GameObject deathParticle;

    [Foldout("Game Over")] [SerializeField] 
    private GameObject gameOverText;
    [Foldout("Game Over")] [SerializeField]
    private TextMeshProUGUI finalScoreText;
    [Foldout("Game Over")] [SerializeField]
    private TextMeshProUGUI finalHighScoreText;
    [Foldout("Game Over")] [SerializeField]
    private NumberManager numberManager;
    

    private bool timerRunning;
    private List<GameObject> finishes;
    private List<GameObject> coins = new List<GameObject>();
    private List<GameObject> paintings = new List<GameObject>(3);
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
                KillPlayer(true);
                return;
            }

            timerSlider.value = Mathf.Lerp(1,0 , currentTime / 5);
            text.text = "\nScore:" + score;
        }
    }
    public void AddScore()
    {
        score += collectableScore;
    }
    public void KillPlayer(bool NoTimeLeft)
    {
        ShowGaps();
        timerRunning = false;
        playerController.isDead = true;
        
        if (NoTimeLeft)
            animator.Play("Drop");
        else
        {
            Instantiate(deathParticle, player.position, Quaternion.Euler(0, 0, 0));
            player.gameObject.SetActive(false);
        } 
        
        GameOver();
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
        GeneratePaintings(3);

        int amountofHoles = Random.Range(0,maxHoles);
        List<Section> openHoles = new List<Section>(amountofHoles);

        for (int i = 0; i <= amountofHoles; i++)
        {
            int sectionIndex = Random.Range(1, sections.Count - 2);
            int whileCount = 0;
            while (!sections[sectionIndex - 1].go.activeInHierarchy || !sections[sectionIndex + 1].go.activeInHierarchy || !sections[sectionIndex].go.activeInHierarchy)
            {
                sectionIndex = Random.Range(1, sections.Count - 2);
                //This count is to stop it endlessly going
                whileCount++;
                if (whileCount > sections.Count)
                    break;
            }

            if (whileCount > sections.Count)
                break;
            
            
            Section lastSection = sections[sectionIndex];
            lastSection.go.SetActive(false);
            lastSection.hasFinish = true;
            openHoles.Add(lastSection);
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

        StartCoroutine(HideGaps());

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
    private void GeneratePaintings(int amount)
    {
        paintings = new List<GameObject>(amount);
        for (int i = 0; i <= amount; i++)
        {
            GameObject lastPainting = Instantiate(paintingPrefab);
            lastPainting.transform.position = coinSpawns[Random.Range(0, coinSpawns.Length - 1)].position;
            paintings.Add(lastPainting);
        }
    }
    IEnumerator HideGaps()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Section section in sections)
        {
            if (!section.go.activeInHierarchy)
            {
                section.go.SetActive(true);
                section.col.enabled = false;
            }
        }

        foreach (GameObject finish in finishes)
        {
            finish.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void ShowGaps()
    {
        foreach (Section section in sections)
        {
            if (section.hasFinish)
            {
                section.go.SetActive(false);
            }
        }
        foreach (GameObject finish in finishes)
        {
            finish.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void ResetLevel()
    {
        score++;
        foreach (Section section in sections)
        {
            section.go.SetActive(true);
            section.col.enabled = true;
            section.hasFinish = false;
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

        for (int i = 0; i < paintings.Count; i++)
        {
            Destroy(paintings[i]);
        }
    }
    private void GameOver()
    {
        int highscore = ScoreManager.highScore;
        if (score > highscore)
        {
            highscore = score;
            ScoreManager.highScore = score;
        }
        gameOverText.SetActive(true);
        finalScoreText.text = "" + score;
        finalHighScoreText.text = "" + highscore;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Replay()
    {
        SceneManager.LoadScene(1);
    }
}
[Serializable]
public class Section
{
    public GameObject go;
    public BoxCollider2D col;
    public Transform bottomPoint;
    public bool hasFinish;
}
