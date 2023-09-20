using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTimer : MonoBehaviour
{
    private GameObject myPlayer;
    private WaypointSystem waypointSystem;

    private Text myText;

    private void Start() {
        myPlayer = GetComponentInParent<PlayerGUIData>().player;
        myText = GetComponent<Text>();
        waypointSystem = GameObject.FindObjectOfType<WaypointSystem>();
    }

    private void OnGUI() {
        float time = waypointSystem.getRaceTime(myPlayer);
        string text = "Time: ";
        int hours = Mathf.FloorToInt(time / 3600);
        if (hours > 0) {
            text += hours.ToString("00") + ":";
        }
        time %= 3600;
        int minutes = Mathf.FloorToInt(time / 60);
        text += minutes.ToString("00") + ":";
        float seconds = time % 60;

        text += seconds.ToString("F2").PadLeft(5, '0');


        myText.text = text;
    }
}
