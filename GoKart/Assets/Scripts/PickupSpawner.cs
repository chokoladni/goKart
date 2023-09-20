using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);
    float respawnTimer = 5.0f;
    GameObject pickupPrefab;
    GameObject currentPickup;

    void Start()
    {
        pickupPrefab = Resources.Load("Prefabs/powerup pickup") as GameObject;
        SpawnNewPickup();
    }

    void SpawnNewPickup() {
        if (currentPickup != null) {
            Debug.LogError("Current pickup still exists.");
            return;
        }

        currentPickup = Instantiate(pickupPrefab, transform.position + offset, pickupPrefab.transform.rotation);
        ((Pickup)currentPickup.GetComponent<Pickup>()).AddListener(OnPickupCollected);
    }

    void OnPickupCollected() {
        StartCoroutine("SpawnCoroutine");
    }

    IEnumerator SpawnCoroutine() {
        yield return new WaitForSeconds(respawnTimer);
        SpawnNewPickup();
    }
}
