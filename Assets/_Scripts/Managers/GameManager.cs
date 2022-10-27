using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI deathScreenScore;
    public TextMeshProUGUI difficultyText;
    private GameObject _player;
    public GameObject deathScreen, difficultyScreen;
    public int score;
    public bool IsPlayerDead { get; set; }
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerDead)
        {
            deathScreen.SetActive(true);
            deathScreenScore.text = "Your score is: " + ColorChange.Instance.scoreInColorChange;
        }
        else
        {
            deathScreen.SetActive(false);
        }
    }
    public void StartNewGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public IEnumerator DifficultyWarning(string message, float delay)
    {
        difficultyText.text = message;
        difficultyScreen.SetActive(true);
        yield return new WaitForSeconds(delay);
        difficultyScreen.SetActive(false);
    }
}
