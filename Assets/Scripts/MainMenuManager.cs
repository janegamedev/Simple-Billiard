using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadGameLevel()
    {
        SceneManager.LoadScene("MainGameLevel",LoadSceneMode.Single);
    } 

    public void ExitGame()
    {
        Application.Quit();
    }
}
