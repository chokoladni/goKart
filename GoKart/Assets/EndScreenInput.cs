using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            LoadGameScene();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            BackToMainMenu();
        }
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("WaypointsTest");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }

}
