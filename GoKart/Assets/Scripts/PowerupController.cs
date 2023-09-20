using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    Powerup currentPowerup;

    public KeyCode activatePowerupKeyCode = KeyCode.Space;
    private static System.Type[] availablePowerups = { typeof(MissilePowerup), typeof(MinePowerup), typeof(RoadblockPowerup) };
    //private static System.Type[] availablePowerups = { typeof(RoadblockPowerup) };

    public void onPowerupPickedUp(Pickup pickup) {
        if (currentPowerup == null) {
            currentPowerup = (Powerup) this.gameObject.AddComponent(availablePowerups[Random.Range(0, availablePowerups.Length)]);
            Debug.Log("Picked up:"+currentPowerup.GetType());
            currentPowerup.setOwner(this.gameObject);
            pickup.PickupSuccessful();
        }
    }

    private void Update() {
        int playerID = GetComponent<BasicCarController>().playerID;
       
        if (Input.GetButton("Joystick" + playerID + "ActivatePowerUp") && currentPowerup != null)
        {
            currentPowerup.Activate();
            currentPowerup = null;
        }
        
        
    }

    public Powerup GetCurrentPowerup() {
        return currentPowerup;
    }
}
