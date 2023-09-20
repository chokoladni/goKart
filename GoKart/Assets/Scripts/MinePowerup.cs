using UnityEngine;
using UnityEditor;

public class MinePowerup : Powerup
{
    GameObject minePrefab;
    private float spawnDistance = 2.3f;

    private void Awake()
    {
        minePrefab = (GameObject)Resources.Load("Prefabs/Mine");
    }

    public override void Activate()
    {
        Vector3 rotation = owner.transform.rotation.eulerAngles;
        Instantiate(minePrefab,
                    owner.transform.position - owner.transform.forward * spawnDistance,
                    Quaternion.Euler(0.0f, owner.transform.rotation.eulerAngles.y, 0.0f));

        Deactivate();
    }

    public override void Deactivate()
    {
        Destroy(this);
    }
}