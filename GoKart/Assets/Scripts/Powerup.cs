using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    protected GameObject owner;

    public Powerup() {
    }

    public void setOwner(GameObject owner) {
        if(!owner.CompareTag("Player")) {
            Debug.LogError("Only players can own powerups!");
            return;
        }
        this.owner = owner;
    }

    public abstract void Activate();

    public abstract void Deactivate();
}
