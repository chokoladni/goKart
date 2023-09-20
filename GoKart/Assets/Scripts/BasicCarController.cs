using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;
    public WheelCollider frontLeft,frontRight,rearLeft,rearRight;
    public Transform frontLeftT,frontRightT,rearLeftT,rearRightT;
    private Rigidbody rb;
    public Light RearLeftLight;
    public Light RearRightLight;
    public MeshRenderer RearLeftLightQuad;
    public MeshRenderer RearRightLightQuad;
    public float maxSteerAngle =30;
    public float motorTorque=50;
    public float maxSpeed=30;
    public int playerID=1;
    private float rearLightIntensity;
    private float rearLightQuadIntensity;
    public float rearLightIntensityBrakingBoost=2;
    public float rearLightQuadIntensityBrakingBoost = 100;
    private WaypointSystem waypointSystem;

    private bool isEnabled = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rearLightIntensity = RearLeftLight.intensity;
        rearLightQuadIntensity=RearLeftLightQuad.material.GetFloat("_EmissiveIntensity");
        waypointSystem = GameObject.FindObjectOfType<WaypointSystem>();
    }
    public void GetInput(){
        if(!isEnabled) {
            horizontalInput = 0;
            verticalInput = 0;
            return;
        }
        if(playerID==1){
            horizontalInput=Input.GetAxis("Horizontal1");
            if (Input.GetAxis("Horizontal1Key") != 0)
            {
                horizontalInput = Input.GetAxis("Horizontal1Key");
            }
            float verticalInput1 =Input.GetAxis("Accelerate1");
            float verticalInput2 = Input.GetAxis("Break1");
            if (Mathf.Abs(verticalInput1) > Mathf.Abs(verticalInput2))
            {
                verticalInput = verticalInput1;
            }
            else
            {
                verticalInput = verticalInput2;
            }
            if (Input.GetAxis("Vertical1")!=0)
            {
                verticalInput = Input.GetAxis("Vertical1");
            }
            if (Input.GetKey(KeyCode.R) ||Input.GetKey(KeyCode.Joystick1Button5)){
                BackOnTrack();
            }
        }else if(playerID==2){
            horizontalInput=Input.GetAxis("Horizontal2");
            if (Input.GetAxis("Horizontal2Key") != 0)
            {
                horizontalInput = Input.GetAxis("Horizontal2Key");
            }
            float verticalInput1 = Input.GetAxis("Accelerate2");
            float verticalInput2 = Input.GetAxis("Break2");
            if (Mathf.Abs(verticalInput1) > Mathf.Abs(verticalInput2))
            {
                verticalInput = verticalInput1;
            }
            else
            {
                verticalInput = verticalInput2;
            }
            if (Input.GetAxis("Vertical2") != 0)
            {
                verticalInput = Input.GetAxis("Vertical2");
            }
            if (Input.GetKey(KeyCode.RightShift)|| Input.GetKey(KeyCode.Joystick2Button5)) { 
                BackOnTrack();
            }
        }
        else if (playerID == 3)
        {
            horizontalInput = Input.GetAxis("Horizontal3");
            if (Input.GetAxis("Horizontal3Key") != 0)
            {
                horizontalInput = Input.GetAxis("Horizontal3Key");
            }
            float verticalInput1 = Input.GetAxis("Accelerate3");
            float verticalInput2 = Input.GetAxis("Break3");
            if (Mathf.Abs(verticalInput1) > Mathf.Abs(verticalInput2))
            {
                verticalInput = verticalInput1;
            }
            else
            {
                verticalInput = verticalInput2;
            }
            if (Input.GetAxis("Vertical3") != 0)
            {
                verticalInput = Input.GetAxis("Vertical3");
            }
            if (Input.GetKey(KeyCode.U) || Input.GetKey(KeyCode.Joystick3Button5))
            {
                BackOnTrack();
            }
        }
        else if (playerID == 4)
        {
            horizontalInput = Input.GetAxis("Horizontal4");
            if (Input.GetAxis("Horizontal4Key") != 0)
            {
                horizontalInput = Input.GetAxis("Horizontal4Key");
            }
            float verticalInput1 = Input.GetAxis("Accelerate4");
            float verticalInput2 = Input.GetAxis("Break4");
            if (Mathf.Abs(verticalInput1) > Mathf.Abs(verticalInput2))
            {
                verticalInput = verticalInput1;
            }
            else
            {
                verticalInput = verticalInput2;
            }
            if (Input.GetAxis("Vertical4") != 0)
            {
                verticalInput = Input.GetAxis("Vertical4");
            }
            if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.Joystick4Button5))
            {
                BackOnTrack();
            }
        }


    }
    public void BackOnTrack()
    {
        rb.isKinematic = true;
        rb.position = waypointSystem.GetResetPosition(this.gameObject) + Vector3.up * 2.0f;
        rb.rotation = waypointSystem.GetResetRotation(this.gameObject);
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
    }
    public void Steer(){
        steeringAngle=maxSteerAngle*horizontalInput;
        frontLeft.steerAngle=steeringAngle;
        frontRight.steerAngle=steeringAngle;
    }
    public void Accelerate(){
        if (verticalInput < 0)
        {
            frontLeft.motorTorque = verticalInput * motorTorque/2;
            frontRight.motorTorque = verticalInput * motorTorque/2;
            RearLeftLight.intensity = rearLightIntensity * rearLightIntensityBrakingBoost;
            RearRightLight.intensity = rearLightIntensity * rearLightIntensityBrakingBoost;
            RearLeftLightQuad.material.SetFloat("_EmissiveIntensity", rearLightQuadIntensity * rearLightQuadIntensityBrakingBoost);
            RearRightLightQuad.material.SetFloat("_EmissiveIntensity", rearLightQuadIntensity * rearLightQuadIntensityBrakingBoost);
        }
        else
        {
            frontLeft.brakeTorque = 0;
            frontRight.brakeTorque = 0;
            frontLeft.motorTorque=verticalInput*motorTorque;
            frontRight.motorTorque=verticalInput*motorTorque;
            RearLeftLight.intensity = rearLightIntensity;
            RearRightLight.intensity = rearLightIntensity;
            RearLeftLightQuad.material.SetFloat("_EmissiveIntensity", rearLightQuadIntensity );
            RearRightLightQuad.material.SetFloat("_EmissiveIntensity", rearLightQuadIntensity );
        }
        


        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
    public void UpdateWheelPoses(){
        UpdateWheelPose(frontLeft,frontLeftT);
        UpdateWheelPose(frontRight,frontRightT);
        UpdateWheelPose(rearLeft,rearLeftT);
        UpdateWheelPose(rearRight,rearRightT);
    }
    public void UpdateWheelPose(WheelCollider collider,Transform transform){
        Vector3 position=transform.position;
        Quaternion quaternion=transform.rotation;
        collider.GetWorldPose(out position,out quaternion);
        transform.position=position;
        transform.rotation=quaternion;

    }
    private void FixedUpdate() {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }
    
    public void SetInputEnabled(bool enabled) {
        this.isEnabled = enabled;
    }
}
