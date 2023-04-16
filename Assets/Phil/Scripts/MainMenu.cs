using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnInstructionsClicked()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void OnStartClicked()
    {
        SceneManager.LoadScene("Easter Egg AREA 1");
    }

    public void OnApplicationQuitClicked()
    {
        Application.Quit();
    }
}
