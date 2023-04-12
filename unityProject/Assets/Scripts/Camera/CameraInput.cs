using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CameraInput : MonoBehaviour
{
    [SerializeField] private List<CinemachineVirtualCamera> camerasP1;
    [SerializeField] private List<CinemachineVirtualCamera> camerasP2;
    [SerializeField] private List<CinemachineVirtualCamera> transitionCameras;
    [SerializeField] private int numberOfRooms;
    private int _camerasInOneRoom;
    private int _roomP1;
    private int _roomP2;
    private int _currentCameraIndexP1;
    private int _currentCameraIndexP2;

    private CinemachineVirtualCamera CurrentCameraP1
    {
        get => camerasP1[_currentCameraIndexP1];
        set => SetProperCamera(camerasP1, value, 1);
    }

    private CinemachineVirtualCamera CurrentCameraP2
    {
        get => camerasP2[_currentCameraIndexP2];
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

        //player1Walking.enabled = false;
        _camerasInOneRoom = camerasP1.Count / numberOfRooms;
    }

    void Update()
    {
        //PLAYER 1 ---------------------------
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SwitchToCamera(1, 1);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SwitchToCamera(_camerasInOneRoom - 1, 1);
        }
        
        if (transitionCameras.Contains(CurrentCameraP1))
        {
            Client.Instance.IsDoorVisibleP1 = true;
            Debug.Log("Looking at the door");
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                if (transitionCameras.IndexOf(CurrentCameraP1) >= 8)
                {
                    GoToTheRoom(-1, 1);
                }
                else
                {
                    GoToTheRoom(1, 1);
                }
            }
            
        }
        

        //PLAYER 2 --------------------------------
        if (Input.GetKeyUp(KeyCode.E))
        {
            SwitchToCamera(1, 2);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            SwitchToCamera(_camerasInOneRoom - 1, 2);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (transitionCameras.Contains(CurrentCameraP2))
            {
                if (transitionCameras.IndexOf(CurrentCameraP2) >= 8)
                {
                    GoToTheRoom(-1, 2);
                }
                else
                {
                    GoToTheRoom(1, 2);
                }
            }
        }
    }

    private void SwitchToCamera(int cameraValue, int player)
    {
        if (player == 1)
        {
            CurrentCameraP1 = ChooseCorrectCamera(camerasP1, cameraValue, _currentCameraIndexP1, _roomP1);
        }
        else if (player == 2)
        {
            CurrentCameraP2 = ChooseCorrectCamera(camerasP2, cameraValue, _currentCameraIndexP2, _roomP2);
        }
    }

    private void SetTransitionCamera(int player)
    {
        if (player == 1)
        {
            CurrentCameraP1 = camerasP1[_camerasInOneRoom * (_roomP1 % numberOfRooms)];
        }
        else if (player == 2)
        {
            CurrentCameraP2 = camerasP2[_camerasInOneRoom * (_roomP2 % numberOfRooms)];
        }
    }

    private CinemachineVirtualCamera ChooseCorrectCamera(List<CinemachineVirtualCamera> cameras, int cameraValue,
        int currentCameraIndex, int room)
    {
        int goToTheCamera = Math.Abs(currentCameraIndex + cameraValue);
        return cameras[_camerasInOneRoom * (room % numberOfRooms) + goToTheCamera % _camerasInOneRoom];
    }

    private void GoToTheRoom(int room, int player)
    {
        if (player == 1)
        {
            _roomP1 += room;
        }
        else if (player == 2)
        {
            _roomP2 += room;
        }

        SetTransitionCamera(player);
    }
}