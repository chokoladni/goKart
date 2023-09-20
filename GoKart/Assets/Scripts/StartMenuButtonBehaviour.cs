using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuButtonBehaviour : MonoBehaviour
{
    public Button ActiveButton;
    public GameObject MainMenu;
    public GameObject options;
    private void Start()
    {
        Cursor.visible = true;
    }

    public void MouseOverButton(Button ActiveButton)
    {
        //ActiveButton.transform.localScale += new Vector3(0.15F, 0.15F, 0);
    }
    public void MouseNotOverButton(Button ActiveButton)
    {
        //ActiveButton.transform.localScale -= new Vector3(0.15F, 0.15F, 0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        MainMenu.SetActive(true);
        options.SetActive(false);
    }
}
