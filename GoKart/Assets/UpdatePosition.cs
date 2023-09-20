using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePosition : MonoBehaviour
{

    private GameObject myPlayer;
    private WaypointSystem waypointSystem;

    private Text myText;

    private void Start() {
        myPlayer = GetComponentInParent<PlayerGUIData>().player;
        myText = GetComponent<Text>();
        waypointSystem = GameObject.FindObjectOfType<WaypointSystem>();
    }

    void OnGUI()
    {
        myText.text = "Position: " + waypointSystem.GetPosition(myPlayer) + " / " + waypointSystem.GetPlayerCount();
    }
}
