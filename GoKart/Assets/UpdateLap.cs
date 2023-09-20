using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLap : MonoBehaviour
{
    private GameObject myPlayer;
    private WaypointSystem waypointSystem;

    private Text myText;

    private void Start() {
        myPlayer = GetComponentInParent<PlayerGUIData>().player;
        myText = GetComponent<Text>();
        waypointSystem = GameObject.FindObjectOfType<WaypointSystem>();
    }

    void OnGUI() {
        myText.text = "Lap: " + Mathf.Min(waypointSystem.GetLap(myPlayer) + 1, waypointSystem.GetLapCount()) + " / " + waypointSystem.GetLapCount();
    }
}
