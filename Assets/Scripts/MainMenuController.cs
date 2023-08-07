using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartClick()
    {
        SceneManager.LoadScene("CoreGame");
    }

    public void ExitClick()
    {
        Application.Quit();
    }
}
