using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreList : MonoBehaviour 
{
    public GameObject playerScoreEntry;

    ScoreManager scoreManager;

    int lastChangeCounter = 0;

    void Start() {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
    }

    void Update() {
        if (scoreManager.GetChangeCounter() == lastChangeCounter) {
            return;
        }

        lastChangeCounter = scoreManager.GetChangeCounter();

        string[] names = scoreManager.GetPlayerNames("time");

        for (int i = 1; i <= 4; i++) {
            string gameObjectName = "PlayerScoreEntry" + i;

            playerScoreEntry = GameObject.Find(gameObjectName);

            if (names.Length >= i) {
                playerScoreEntry.transform.Find("Name").GetComponent<Text>().text = names[i - 1];
                
                float time = float.Parse(scoreManager.GetScore(names[i - 1], "time"));
                string text = "";
                int hours = Mathf.FloorToInt(time / 3600);
                if (hours > 0) {
                    text += hours.ToString("00") + ":";
                }
                time %= 3600;
                int minutes = Mathf.FloorToInt(time / 60);
                text += minutes.ToString("00") + ":";
                float seconds = time % 60;

                text += seconds.ToString("F2").PadLeft(5, '0');

                playerScoreEntry.transform.Find("Time").GetComponent<Text>().text = text;
            }
            else {
                playerScoreEntry.SetActive(false);
            }
        }
    }
}
