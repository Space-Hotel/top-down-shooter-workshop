using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public AudioManager audioManager;
    public TextMeshProUGUI gameOverText;

    [HideInInspector]
    public bool gamePaused = false;
    void Start()
    {
        main = this;
        gameOverText.enabled = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            Time.timeScale = (gamePaused) ? 0f : 1f;
        }
    }

    public void GameOver()
    {
        gameOverText.enabled = true;
        audioManager.PlayGameOver();
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("CoreGame");
    }
}
