using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public GameObject playerCar;
    public GameObject type;
    ScoreManager scoreManager;

    int lastChangeCounter;

    void Start()
    {
        Cursor.visible = true;
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        lastChangeCounter = 0;
    }

    void Update()
    {
        if (scoreManager.GetChangeCounter() == lastChangeCounter)
        {
            return;
        }

        lastChangeCounter = scoreManager.GetChangeCounter();

        string[] names = scoreManager.GetPlayerNames("time");

        for (int i = 1; i <= names.Length; i++)
        {
            string gameObjectName = "PlayerCar" + i;
            string typeS = scoreManager.GetScore(names[i - 1], "car");

            playerCar = GameObject.Find(gameObjectName);
            type = GameObject.Find(typeS);

            if (names.Length >= i)
            {
                playerCar = Instantiate(type, playerCar.transform.position, Quaternion.Euler(0, -140, 0)) as GameObject;
                playerCar.transform.parent = transform;
            }
            else
            {
                playerCar.SetActive(false);
            }
        }
    }
}
