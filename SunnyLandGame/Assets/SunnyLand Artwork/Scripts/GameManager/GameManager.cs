using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameData gameData; // Gán Scriptable Object trong Inspector
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public TextMeshProUGUI text5;

    private int NumOfEnemy1 = 38;//38;
    private int NumOfEnemy2 = 43;//43;
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    public GameObject levelCompletedUI;
    private AudioManager audioManager;
    private Boss Boss;
    public GameObject flag;
    public GameObject flag1;
    private Playercontroller playercontroller;
    private bool isPauseGame = false;
    private bool isGameOver = false;
    private bool isWinGame = false;
    private bool isLevelComplete = false;

    void Start()
    {
        UpdateUI();
        audioManager = FindAnyObjectByType<AudioManager>();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
        levelCompletedUI.SetActive(false);
        flag.SetActive(false);
        flag1.SetActive(false);
        playercontroller = FindObjectOfType<Playercontroller>();
        Boss = FindObjectOfType<Boss>();
    }

    void Update()
    {
        CheckY();
        if (gameData.health <= 0 && !isGameOver)
        {
            GameOver();
        }
        if (NumOfEnemy1 == 0)
        {
            flag.SetActive(true);
        }
        if (NumOfEnemy2 == 0)
        {
            flag1.SetActive(true);
        }
        if(Boss != null && Boss.GetIsDie() && !isWinGame)
        {
            WinGame();
        }
    }

    public void AddScore1(int points)
    {
        gameData.AddScore1(points);
        UpdateUI();
    }

    public void AddScore2(int points)
    {
        gameData.AddScore2(points);
        UpdateUI();
    }

    public void MinusHealth(int amount)
    {
        gameData.MinusHealth(amount);
        UpdateUI();
    }

    void CheckY()
    {
        if (playercontroller.transform.position.y < -7 && !isGameOver)
        {
            gameData.health = 0;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        text1.text = gameData.score1.ToString(); // Vàng
        text2.text = gameData.score2.ToString(); // Kim cương
        text3.text = gameData.health.ToString(); // Máu
        text4.text = NumOfEnemy1.ToString();
        text5.text = NumOfEnemy2.ToString();
    }
    public void MinusEnemy(int points)
    {
        NumOfEnemy1 -= points;
        UpdateUI();
        Debug.Log(NumOfEnemy1 + "man1");
    }

    public void MinusEnemy1(int points)
    {
        NumOfEnemy2 -= points;
        UpdateUI();
        Debug.Log(NumOfEnemy2 + "man2");
    }
    public void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        audioManager.PauseBackgroundAudioSource();
        audioManager.PlayGameOverSound();
    }

    public void LevelComplete()
    {
        isLevelComplete = true;
        levelCompletedUI.SetActive(true);
        Time.timeScale = 0;
        audioManager.PauseBackgroundAudioSource();
        audioManager.PlayGameWinSound();
    }

    public void SetIsLevelCompleted()
    {
        isLevelComplete = false;

    }

    public bool GetIsLevelComplete()
    {
        return isLevelComplete;
    }

    public void LoadScene(int  sceneId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneId);
    }
    public void WinGame()
    {
        isWinGame = true;
        gameWinUI.SetActive(true);
        Time.timeScale = 0;
        audioManager.PauseBackgroundAudioSource();
        audioManager.PlayGameWinSound();
    }
    public void PauseGame()
    {
        isPauseGame = true;
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        isPauseGame = false;
        Time.timeScale = 1;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main menu");
        gameData.ResetData();
        UpdateUI();
    }

    public void NewStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
        gameData.ResetData();
        UpdateUI();
    }

    public void NewStart1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level2");
        gameData.ResetData();
        gameData.health = 5;
        UpdateUI();
    }

    public void NewStart2()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level3");
        gameData.ResetData();
        gameData.health = 4;
        UpdateUI();
    }
    public bool IsPauseGame()
    {
        return isPauseGame;
    }
}


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score1 = 0;
    public int score2 = 0;
    public int health = 5;
    private int NumOfEnemy1 = 3;
    private int NumOfEnemy2 = 5;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;

    public GameObject gameOverUI;
    private AudioManager audioManager;
    public GameObject flag;
    public GameObject flag1;
    private Playercontroller playercontroller;

    private bool isPauseGame = false;
    private bool isGameOver = false;
    void Start()
    {
        UpdateScore1();
        UpdateScore2();
        UpdateHealth();
        audioManager = FindAnyObjectByType<AudioManager>();
        gameOverUI.SetActive(false);
        flag.SetActive(false);
        flag1.SetActive(false);
        playercontroller = FindObjectOfType<Playercontroller>();
    }


    void Update()
    {
        CheckY();
        if (health == 0 && !isGameOver)
        {
            GameOver();
        }
        if (NumOfEnemy1 == 0)
        {
            flag.SetActive(true);
        }
        if (NumOfEnemy2 == 0)
        {
            flag1.SetActive(true);
        }
    }

    public void AddScore1(int points)
    {
        score1 += points;
        UpdateScore1();
    }

    public void AddScore2(int points)
    {
        score2 += points;
        UpdateScore2();
    }

    public void MinusEnemy(int points)
    {
        NumOfEnemy1 -= points;
        Debug.Log(NumOfEnemy1 + "man1");
    }

    public void MinusEnemy1(int points)
    {
        NumOfEnemy2 -= points;
        Debug.Log(NumOfEnemy2 + "man2");
    }
    public void MinusHealth(int health1)
    {
        health -= health1;
        UpdateHealth();
    }

    void CheckY()
    {
        if (playercontroller.transform.position.y < -7)
            health = 0;
    }
    void UpdateScore1()
    {
        text1.text = score1.ToString();
        Debug.Log("In ra số tiền ban đầu");
    }

    void UpdateScore2()
    {
        text2.text = score2.ToString();
        Debug.Log("In ra số kim cương");
    }

    void UpdateHealth()
    {
        text3.text = health.ToString();
        Debug.Log(health);
    }

    public int Health()
    {
        return health;
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        audioManager.PauseBackgroundAudioSource();
        audioManager.PlayGameOverSound();
    }
    public void PauseGame()
    {
        isPauseGame = true;
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        isPauseGame = false;
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main menu");
        health = 5;
        UpdateHealth();
    }

    public void NewStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
        health = 5;
        UpdateHealth();
    }

    public void NewStart1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level2");
        health = 5;
        UpdateHealth();
    }

    public bool IsPauseGame()
    {
        return isPauseGame;
    }
}*/