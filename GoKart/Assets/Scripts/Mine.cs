using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    float velocity = 20.0f;
    float turn = 1.0f;
    float lockOnRadius = 15.0f;
    float explodeRadius = 5.0f;
    float maxExplodeForce = 500000.0f;
    float selfDestructionTimer = 5.0f;

    private GameObject target;
    private Rigidbody homingMissile;
    private GameObject missileObject;
    private GameObject explosion;


    void Start()
    {
        //TODO: add launch particles?
        homingMissile = GetComponent<Rigidbody>();
    }
    
    GameObject CheckForTarget() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, lockOnRadius);
        GameObject target = null;
        foreach(Collider col in colliders) {
            if(col.gameObject.CompareTag("Player")) {
                float angle = Vector3.Angle(transform.forward, col.gameObject.transform.position - transform.position);
                if(angle < 90.0f) {
                    target = col.gameObject;
                }
            }
        }

        return target;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider col in colliders) {
            if (col.gameObject.CompareTag("Player")) {
                col.attachedRigidbody.AddExplosionForce(maxExplodeForce, transform.position, explodeRadius);
            }
        }
        //TODO: explosion particles
        var explosionPrefab = Resources.Load("Prefabs/ParticleEffects/BigExplosion") as GameObject;
        explosion = GameObject.Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(explosion, 2);
        Destroy(this.gameObject);
    }

    /*private void FixedUpdate() {
        selfDestructionTimer -= Time.fixedDeltaTime;
        if(selfDestructionTimer <= 0.0f) {
            Explode();
        }
        if (target == null) {
            target = CheckForTarget();
        }
        homingMissile.velocity = transform.forward * velocity;
        
        if(target != null) {
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            homingMissile.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
        }
    }
    */


}
