using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LockPick : MonoBehaviour
{
    [SerializeField]
    private Transform innerLock;
    [SerializeField]
    private Transform pickPosition;

    [SerializeField]
    private float lockSpeed = 5f;

    [SerializeField]
    private float lockRange = 10;

    private float differenceAngle;
    private float eulerAngle;

    [SerializeField]
    private float unlockAngle;

    [SerializeField]
    private int timesTurned = 0;

    private Vector3 pickRotation;

    private bool movePick = true;

    private bool correct = false;

    void Start()
    {
        NewLock();
        pickRotation = Vector3.zero;
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //set the rotation of the pick to the rotation of the gyro
        transform.localPosition = pickPosition.position;
        pickRotation.z = Input.gyro.rotationRateUnbiased.z;

        //when not checking the position by touching the pick is movable
        if (movePick)
        {
            //give eulerAngle the gyro information to make calculations
            eulerAngle = Input.gyro.attitude.eulerAngles.z;

            transform.rotation = Quaternion.Euler(0, 0, eulerAngle);

            //calculate the difference between the unlockable angle and the angle the pick is in
            differenceAngle = eulerAngle - unlockAngle;

            //unity works with 360 degrees. when the angle of the pick is over 180, reverse the numbers so the difference van be calculated correctly
            if (differenceAngle > 180)
            {
                differenceAngle = Mathf.Abs(360 - differenceAngle);
            }
        }

        //touching the phone to check the lock position
        if (Input.touchCount > 0)
        {
            movePick = false;

            if(differenceAngle < lockRange)
            {
                Debug.Log("unlock");
                RotateInner();
                NewLock();
            }
        }
        else
        {
            movePick = true;
        }
    }

    void RotateInner()
    {
        innerLock.transform.Rotate(new Vector3(0, 0, -10 * lockSpeed) * Time.deltaTime);
    }
    void NewLock()
    {
        unlockAngle = Mathf.Abs(Random.Range(0, 180));
    }
}
