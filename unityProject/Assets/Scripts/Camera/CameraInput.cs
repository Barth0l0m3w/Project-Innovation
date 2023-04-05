using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CameraInput : MonoBehaviour
{
    [SerializeField] private Image player1Walking;
    [SerializeField] private List<CinemachineVirtualCamera> camerasP1;
    [SerializeField] private List<CinemachineVirtualCamera> camerasP2;
    [SerializeField] private int numberOfRooms;
    private int _camerasInOneRoom;
    private int _room;
    private int _currentCameraIndexP1;
    private int _currentCameraIndexP2;

    private CinemachineVirtualCamera CurrentCameraP1
    {
        set => SetProperCamera(camerasP1, value,1);
    }

    private CinemachineVirtualCamera CurrentCameraP2
    {
        set => SetProperCamera(camerasP2, value, 2);
    }

    /// <summary>
    /// Sets proper camera for chosen player.
    /// </summary>
    /// <param name="player"> Used to make sure that proper current camera will be assigned</param>
    private void SetProperCamera(List<CinemachineVirtualCamera> cameras, CinemachineVirtualCamera newCamera, int player)
    {
        //deactivate current camera
        if (player == 1)
        {
            CameraSwitcher.UnregisterP1(cameras[_currentCameraIndexP1]);
        }
        else
        {
            CameraSwitcher.UnregisterP2(cameras[_currentCameraIndexP2]);
        }

        //get the index of the new camera
        int newIndex = cameras.IndexOf(newCamera);

        //check is the camera is valid
        if (newIndex >= 0 && newIndex < cameras.Count)
        {
            //set the new camera index and activate the new camera
            if (player == 1)
            {
                _currentCameraIndexP1 = newIndex;
                CameraSwitcher.RegisterP1(cameras[newIndex]);
                
            }
            else
            {
                _currentCameraIndexP2 = newIndex;
                CameraSwitcher.RegisterP2(cameras[newIndex]);
            }
            
        }
        else
        {
            Debug.Log("Invalid camera selection");
        }
    }

    private void Start()
    {
        //activate the 1st camera in the list
        if (camerasP1.Count > 0)
        {
            CurrentCameraP1 = camerasP1[_currentCameraIndexP1];
        }

        if (camerasP2.Count > 0)
        {
            CurrentCameraP2 = camerasP2[_currentCameraIndexP2];
        }

        player1Walking.enabled = false;
        _camerasInOneRoom = camerasP1.Count / numberOfRooms;
    }

    void Update()
    {
        // Check for keyboard input to cycle through the cameras
        if (Input.GetKeyUp(KeyCode.D))
        {
            CurrentCameraP2 = camerasP2[(_currentCameraIndexP2 + 1) % camerasP2.Count];
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            // Set the current camera to the previous camera in the list
            CurrentCameraP2 = camerasP2[(_currentCameraIndexP2 - 1 + camerasP2.Count) % camerasP2.Count];
        }
        // if (Input.GetKeyUp(KeyCode.RightArrow))
        // {
        //     // Set the current camera to the next camera in the list
        //     CurrentCameraP1 = camerasP1[(_currentCameraIndexP1 + 1) % camerasP1.Count];
        // }
        // else if (Input.GetKeyUp(KeyCode.LeftArrow))
        // {
        //     // Set the current camera to the previous camera in the list
        //     CurrentCameraP1 = camerasP1[(_currentCameraIndexP1 - 1 + camerasP1.Count) % camerasP1.Count];
        // }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SwitchToCamera(1);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SwitchToCamera(2);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SwitchToCamera(3);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SwitchToCamera(4);
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            //Go forward
            GoToTheRoom(1);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            //Go backwards
            GoToTheRoom(numberOfRooms-1);
        }
    }

    private void SwitchToCamera(int camera)
    {
        CurrentCameraP1 = camerasP1[_camerasInOneRoom*(_room%numberOfRooms)+(camera - 1)];
    }

    private void GoToTheRoom(int room)
    {
        _room += room;
        player1Walking.enabled = true;
        Invoke(nameof(ShowImage), 2f);
        CurrentCameraP1 = camerasP1[4*(_room%numberOfRooms)];
    }

    private void ShowImage()
    {
        player1Walking.enabled = false;
    }
}
