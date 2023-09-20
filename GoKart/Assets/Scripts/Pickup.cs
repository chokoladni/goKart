using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    UnityEvent pickedUpEvent;
    public GameObject normalCrate;
    public GameObject fracturedCrate;
    private void Awake() {
        pickedUpEvent = new UnityEvent();
    }

    public void PickupSuccessful() {
        GetComponent<BoxCollider>().enabled = false;
        DestroyCrate();
        pickedUpEvent.Invoke();
        Destroy(this.gameObject,3);
    }

    public void AddListener(UnityAction callback) {
        pickedUpEvent.AddListener(callback);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            var powerupController = other.GetComponentInParent<PowerupController>();
            if (powerupController == null) {
                Debug.LogError("Couldn't find PowerupController in player.");
                return;
            }
            powerupController.onPowerupPickedUp(this);
        }
    }
    private void DestroyCrate()
    {
        normalCrate.SetActive(false);
       // GetComponent<Animator>().SetTrigger("pickedUp");
        GetComponent<Animator>().speed = 0;
        fracturedCrate.SetActive(true);
    }
}
