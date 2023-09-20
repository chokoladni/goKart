using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void SceneLoader(string SceneName) {
        SceneManager.LoadScene(SceneName);
    }
}
