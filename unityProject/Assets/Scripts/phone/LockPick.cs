using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class LockPick : MonoBehaviour
{
    //images for the turning linked
    [SerializeField]
    private Transform innerLock;
    [SerializeField]
    private Transform pickPosition;

    //unlock mechanism variables
    [SerializeField]
    private float lockSpeed = 5f;
    [SerializeField]
    private float lockRange;
    [SerializeField]
    private float unlockAngle;

    [SerializeField]
    private float differenceAngle;
    private float eulerAngle;
    private Vector3 pickRotation;

    //stuff
    [SerializeField]
    private int timesTurned;
    private bool movePick = true;

    //timer variables
    [SerializeField]
    private float timerTime;
    private float countdownTime = 1f;
    private bool isRunning;

    void Start()
    {
        NewLock();
        pickRotation = Vector3.zero;
        Input.gyro.enabled = true;
        
    }

    private void OnEnable()
    {
        Client.Instance.PuzzleSolved = false;
    }

    void Update()
    {
        //set the rotation of the pick to the rotation of the gyro
        pickRotation.z = Input.gyro.rotationRateUnbiased.z;

        //when not checking the position by touching the pick is movable
        if (movePick)
        {
            //give eulerAngle the gyro information to make calculations
            eulerAngle = Input.gyro.attitude.eulerAngles.z;

            transform.rotation = Quaternion.Euler(0, 0, eulerAngle + 90);

            //calculate the difference between the unlockable angle and the angle the pick is in
            //NEED: make a function that gives an indication when the player is in the correct spot with the pick
            differenceAngle = eulerAngle - unlockAngle;

            //unity works with 360 degrees. when the angle of the pick is over 180, reverse the numbers so the difference can be calculated correctly
            if (differenceAngle > 180)
            {
                differenceAngle = Mathf.Abs(360 - differenceAngle);
            }
        }

        //touching the phone to check the lock position
        if (Input.touchCount > 0)
        {
            movePick = false;

            if (differenceAngle < lockRange)
            {
                Debug.Log("unlock");
                ActivateTimer();
            }
        }
        else
        {
            movePick = true;
        }

        //timer stuff, only running when the function is needed
        if (isRunning)
        {
            RotateOuter();
        }

        if (GetTurned() == 3)
        {
            GameDone();
        }
    }

    private void RotateOuter()
    {
        //when called start the timer, disable the moving of the pick and start rotating the inner lock
        countdownTime -= Time.deltaTime;
        movePick = false;
        RotateInner();

        if (countdownTime <= 0)
        {
            //when done: up the int, reset timer, enable moving the pick and stop the function
            SetTurned();
            NewLock();
            countdownTime = timerTime;
            movePick = true;
            isRunning = false;
        }
    }

    private int GetTurned()
    {
        return timesTurned;
    }

    private void SetTurned()
    {
        timesTurned++;
    }

    void GameDone()
    {
        Debug.Log("game finished, yay");
        Client.Instance.LockPickedPhone = true;
        Client.Instance.PuzzleSolved = true;
    }

    void ActivateTimer()
    {
        isRunning = true;
    }

    void RotateInner()
    {
        //rotate the inner lock image
        innerLock.transform.Rotate(new Vector3(0, 0, -10 * lockSpeed) * Time.deltaTime);
    }

    void NewLock()
    {
        if (timesTurned == 0)
        {
            unlockAngle = 90;
        }
        if (timesTurned == 1)
        {
            unlockAngle = 15;
        }
        if (timesTurned == 2)
        {
            unlockAngle = 90;
        }
    }
}
