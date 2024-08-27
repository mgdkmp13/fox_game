using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public enum GameState
{
    GS_PAUSEMENU,
    GS_GAME,
    GS_LEVELCOMPLETED,
    GS_GAME_OVER,
    GS_OPTIONS
}
public class GameManager : MonoBehaviour
{
    public GameState currentGameState;

    public static GameManager instance;

    public Canvas inGameCanvas;

    public Canvas pauseMenuCanvas;

    public Canvas levelCompletedCanvas;

    public Canvas optionsCanvas;

    public Canvas losingCanvas;


    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text killText;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;
    public TMP_Text qualityText;

    public Slider suwak;

    private int kills = 0;
    private int score = 0;
    private float timer = 0f;

    public Image[] keysTab;
    public Image[] hearts;

    public int keysFound = 0;
    public const int keysNumber = 3;
    public int lives = 3;

    public const string keyHighScore_193195_1 = "HighScoreLevel1_193195";
    public const string keyHighScore_193195_2 = "HighScoreLevel2_193195";

    public void OnNextLevelButtonPressed()
    {
        SceneManager.LoadScene("Level2_193195");
    }

    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.GS_GAME)
            inGameCanvas.enabled = true;
        else
            inGameCanvas.enabled = false;


        currentGameState = newGameState;
        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
        levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
        optionsCanvas.enabled = (currentGameState == GameState.GS_OPTIONS);
        losingCanvas.enabled = (currentGameState == GameState.GS_GAME_OVER);
        if(currentGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.name == "Level1_193195")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore_193195_1);
                if(highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore_193195_1, highScore);
                }
                finalScoreText.text = "your score = " + score.ToString("D4");
                highScoreText.text = "the best score = " + PlayerPrefs.GetInt(keyHighScore_193195_1).ToString("D4");
            }
            if (currentScene.name == "Level2_193195")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore_193195_2);
                if (highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore_193195_2, highScore);
                }
                finalScoreText.text = "your score = " + score.ToString("D4");
                highScoreText.text = "the best score = " + PlayerPrefs.GetInt(keyHighScore_193195_2).ToString("D4");
            }

        }
    }

    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
        //Debug.Log(vol.ToString());
    }

    public void onQualityButtonUp()
    {
        QualitySettings.IncreaseLevel();
        qualityText.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void onQualityButtonDown()
    {
        QualitySettings.DecreaseLevel();
        qualityText.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void onOptionsButtonClicked()
    {
        Time.timeScale = 0;
        qualityText.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
        Options();

    }

    public void onResumeButtonClicked()
    {
        Time.timeScale = 1;
        InGame();
    }

    public void onRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onReturnToMainMenuButtonClicked()
    {
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath("StudentGames/193195/Level/Scenes/MainMenu_193195");
        if (sceneIndex >= 0)
        {
            SceneManager.LoadSceneAsync(sceneIndex); //³adowanie sceny ³¹cz¹cej gry
        }
        else
        {
            //sceneIndex jest równe -1. Nie znaleziono sceny.
            //³adowanie innej sceny docelowo na laboratorium
        }
        //SceneManager.LoadScene("MainMenu");
    }


    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString("D3");
    }

    public void addKill()
    {
        kills++;
        killText.text = kills.ToString("D2");
    }

    public void changeLives(int change)
    {
        lives+= change;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }

    public void addKeys(int keyIndex)
    {
        if(keyIndex == 0)
        {
            keysTab[keyIndex].color = new Color(1f, 1f, 1f);
        }
        if (keyIndex == 1)
        {
            keysTab[keyIndex].color = new Color(1f, 1f, 0f);
        }
        if (keyIndex == 2)
        {
            keysTab[keyIndex].color = new Color(60f / 255f, 177f / 255f, 255f / 255f);
        }
        keysFound++;
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }
    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }
    private void Awake()
    {
        InGame();
        if (!PlayerPrefs.HasKey(keyHighScore_193195_1))
        {
            PlayerPrefs.SetInt(keyHighScore_193195_1, 0);
        }
        if (!PlayerPrefs.HasKey(keyHighScore_193195_2))
        {
            PlayerPrefs.SetInt(keyHighScore_193195_2, 0);
        }
        instance = this;
        scoreText.text = score.ToString("D3");
        killText.text = kills.ToString("D2");
        for (int i = 0; i < keysTab.Length; i++)
        {
            keysTab[i].color = Color.gray;
        }
        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }
    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }




    // Start is called before the first frame update
    void Start()
    {
        suwak.onValueChanged.AddListener(delegate { SetVolume(suwak.value); });
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGameState == GameState.GS_GAME)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
            }
            else if (currentGameState == GameState.GS_PAUSEMENU)
            {
                InGame();
            }
        }
    }
}
