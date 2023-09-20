using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    public float followSpeed=10;
    public float lookSpeed=10;
    public void LookAtTarget(){
        Vector3 lookDirection=objectToFollow.position-transform.position;
        Quaternion rotation=Quaternion.LookRotation(lookDirection,Vector3.up);
        transform.rotation=Quaternion.Slerp(transform.rotation,rotation,lookSpeed*Time.deltaTime);
    }
    public void MoveToTarget(){
        Vector3 targetPosition=objectToFollow.position+objectToFollow.forward*offset.z+objectToFollow.right*offset.x+objectToFollow.up*offset.y;
        transform.position=Vector3.Lerp(transform.position,targetPosition,followSpeed*Time.deltaTime);
    }
 private void FixedUpdate() {
        LookAtTarget();
        MoveToTarget();
    }
    
}
