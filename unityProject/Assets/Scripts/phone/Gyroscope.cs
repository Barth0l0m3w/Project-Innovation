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
        rot.z = Input.gyro.rotationRateUnbiased.z;

        transform.Rotate(rot);

        //Debug.Log(rot);
    }
}
