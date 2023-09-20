using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePowerup : MonoBehaviour
{
    private GameObject myPlayer;
    private PowerupController controller;
    private Image image;

    private Dictionary<System.Type, Sprite> powerupToSprite;
    private void Start() {
        myPlayer = GetComponentInParent<PlayerGUIData>().player;
        controller = myPlayer.GetComponent<PowerupController>();
        image = GetComponent<Image>();
        powerupToSprite = new Dictionary<System.Type, Sprite>();
        powerupToSprite.Add(typeof(MissilePowerup), Resources.Load<Sprite>("Sprites/missile"));
        powerupToSprite.Add(typeof(MinePowerup), Resources.Load<Sprite>("Sprites/mine"));
        powerupToSprite.Add(typeof(RoadblockPowerup), Resources.Load<Sprite>("Sprites/roadblock"));
    }

    private void OnGUI() {
        Powerup currentPowerup = controller.GetCurrentPowerup();
        if(currentPowerup == null) {
            image.enabled = false;
            return;
        }
        if(image.enabled == true) {
            return;
        }
        image.enabled = true;
        image.sprite = powerupToSprite[currentPowerup.GetType()];
    }
}
