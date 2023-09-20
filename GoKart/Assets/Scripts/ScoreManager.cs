using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    
    Dictionary<string, Dictionary<string, string>> playerScores;

    int changeCounter = 0;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Init() {
        if (playerScores != null)
            return;

        playerScores = new Dictionary<string, Dictionary<string, string>>();
    }

    public string GetScore(string username, string scoreType) {
        Init();

        if (playerScores.ContainsKey(username) == false) {
            return "0";
        }

        if (playerScores[username].ContainsKey(scoreType) == false) {
            return "0";
        }

        return playerScores[username][scoreType];
    }

    public void SetScore(string username, string scoreType, string value) {
        Init();

        changeCounter++;

        if (playerScores.ContainsKey(username) == false) {
            playerScores[username] = new Dictionary<string, string>();
        }


        playerScores[username][scoreType] = value;
    }

    public string[] GetPlayerNames() {
        Init();

        return playerScores.Keys.ToArray();
    }

    public string[] GetPlayerNames(string sortingScoreType) {
        Init();
        if(sortingScoreType == "time") {
            return playerScores.Keys.OrderBy(n => float.Parse(GetScore(n, sortingScoreType))).ToArray();
        }
        return playerScores.Keys.OrderBy(n => GetScore(n, sortingScoreType)).ToArray();
    }

    public int GetChangeCounter() {
        return changeCounter; 
    }
}
