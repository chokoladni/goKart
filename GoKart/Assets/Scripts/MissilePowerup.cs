using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePowerup : Powerup
{
    GameObject missilePrefab;
    private float spawnDistance = 3.0f;

    private void Awake() {
        missilePrefab = (GameObject)Resources.Load("Prefabs/Missile");
    }

    public override void Activate() {
        Vector3 rotation = owner.transform.rotation.eulerAngles;
        Instantiate(missilePrefab,
                    owner.transform.position + owner.transform.forward * spawnDistance,
                    Quaternion.Euler(0.0f, owner.transform.rotation.eulerAngles.y, 0.0f));

        Deactivate();
    }

    public override void Deactivate() {
        Destroy(this);
    }
}
