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
    void Start()
    {
        main = this;
        gameOverText.enabled = false;
    }

    public void Update()
    {
        
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
