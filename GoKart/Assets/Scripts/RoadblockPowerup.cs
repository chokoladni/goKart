using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadblockPowerup : Powerup
{
    float lifespan = 10.0f;
    GameObject roadblockPrefab;
    GameObject roadblock;

    private void Start() {
        roadblockPrefab = Resources.Load("Prefabs/roadblock") as GameObject;
    }

    public override void Activate() {
        Vector3 horizontalOffset = new Vector3(owner.transform.forward.x, 0, owner.transform.forward.z).normalized * 3.0f;
        Vector3 verticalOffset = Vector3.up * 3.0f;

        Vector3 position = owner.transform.position - horizontalOffset;
        Quaternion rotation = Quaternion.identity;
        
        RaycastHit hitInfo;
        if (Physics.Raycast(owner.transform.position - horizontalOffset + verticalOffset, Vector3.down, out hitInfo)) {
            position = hitInfo.point;
            Vector3 cross = Vector3.Cross(hitInfo.normal, owner.transform.position - hitInfo.point).normalized;
            Vector3 forward = Vector3.Cross(cross, hitInfo.normal);
            rotation = Quaternion.LookRotation(forward, hitInfo.normal);
            Debug.Log("NORMALA: " + hitInfo.normal);
        }

        roadblock = Instantiate(roadblockPrefab, position, rotation);
        StartCoroutine("AutoDestroy");
    }

    private IEnumerator AutoDestroy() {
        yield return new WaitForSeconds(lifespan);
        Deactivate();
    }

    public override void Deactivate() {
        Destroy(roadblock);
        Destroy(this);
    }
}
