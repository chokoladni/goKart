using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCarsOnMenuScreen : MonoBehaviour
{
    float RotationSpeed = 20;
    public GameObject Car;
        
    private void Update()
    {
        Car.transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
    }
}
