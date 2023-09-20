using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonLoadGame : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject carselect;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7)||
            Input.GetKeyDown(KeyCode.Joystick2Button7) ||
            Input.GetKeyDown(KeyCode.Joystick3Button7) ||
            Input.GetKeyDown(KeyCode.Joystick4Button7))
        {
            LoadGameScene();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
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
        MainMenu.SetActive(true);
        carselect.SetActive(false);
    }
}
