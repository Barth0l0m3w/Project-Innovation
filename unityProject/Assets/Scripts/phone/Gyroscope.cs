using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    Vector3 rot;

    void Start()
    {
        rot = Vector3.zero;
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        rot = Input.gyro.rotationRateUnbiased;
        transform.Rotate(rot);
    }
}
