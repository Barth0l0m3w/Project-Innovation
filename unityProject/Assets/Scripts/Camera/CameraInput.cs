using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CameraInput : MonoBehaviour
{
    [SerializeField] private List<CinemachineVirtualCamera> camerasP1;
    [SerializeField] private List<CinemachineVirtualCamera> camerasP2;
    [SerializeField] private List<CinemachineVirtualCamera> transitionCameras;
    [SerializeField] private CinemachineVirtualCamera[] exitCameras = new CinemachineVirtualCamera[2];

    [SerializeField]
    private Dictionary<CinemachineVirtualCamera, Zoom> zooms = new Dictionary<CinemachineVirtualCamera, Zoom>();
    [SerializeField] private int numberOfRooms;

    private Client _client;
    
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
        if (camerasP1.Count != 0)
        {
            CurrentCameraP1 = camerasP1[_currentCameraIndexP1];
        }

        if (camerasP2.Count != 0)
        {
            CurrentCameraP2 = camerasP2[_currentCameraIndexP2];
        }
        _camerasInOneRoom = camerasP1.Count / numberOfRooms;
        _client = Client.Instance;

        foreach (var cam in camerasP1)
        {
            if (cam.GetComponent<Zoom>())
            {
                zooms.Add(cam, cam.GetComponent<Zoom>());
            }
        }
        
        foreach (var cam in camerasP2)
        {
            if (cam.GetComponent<Zoom>())
            {
                zooms.Add(cam, cam.GetComponent<Zoom>());
            }
        }
    }

    void Update()
    {
        //PLAYER 1 ---------------------------
        if (Input.GetKeyUp(KeyCode.Alpha3) || _client.ButtonP1 == 3)
        {
            SwitchToCamera(1, 1);
            _client.ButtonP1 = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1) || _client.ButtonP1 == 1)
        {
            SwitchToCamera(_camerasInOneRoom - 1, 1);
            _client.ButtonP1 = 0;
        }

        if (transitionCameras.Contains(CurrentCameraP1) && _client.LockPickedLaptop)
        {
            _client.IsDoorVisibleP1 = true;
            if (Input.GetKeyUp(KeyCode.Alpha2) || _client.ButtonP1 == 2)
            {

                if (transitionCameras.IndexOf(CurrentCameraP1) >= 8)
                {
                    GoToTheRoom(-1, 1);
                }
                else
                {
                    GoToTheRoom(1, 1);
                }

                _client.ButtonP1 = 0;
            }
        }
        else
        {
            _client.IsDoorVisibleP1 = false;
        }

        if (exitCameras.Contains(CurrentCameraP1) && _client.LockCorrect)
        {
            Debug.Log("GO OUT");
        }

        if ((_client.ButtonClicked == 4 || _client.ButtonClicked == 5) && _client.PlayerNumber == 1)
        {
            if (zooms.ContainsKey(CurrentCameraP1))
            {
                zooms[CurrentCameraP1].ZoomCamera();
            }
        } else if (_client.ButtonClicked == 6 && _client.PlayerNumber == 1)
        {
            if (zooms.ContainsKey(CurrentCameraP1))
            {
                zooms[CurrentCameraP1].ZoomOutCamera();
            }
        }


    //PLAYER 2 --------------------------------
    if (Input.GetKeyUp(KeyCode.E) || _client.ButtonP2 == 3)
        {
            SwitchToCamera(1, 2);
            _client.ButtonP2 = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Q) || _client.ButtonP2 == 1)
        {
            SwitchToCamera(_camerasInOneRoom - 1, 2);
            _client.ButtonP2 = 0;
        }
        
        if (transitionCameras.Contains(CurrentCameraP2) && _client.LockPickedLaptop)
        {
            _client.IsDoorVisibleP2 = true;
            if (Input.GetKeyUp(KeyCode.W)|| _client.ButtonP2 == 2)
            {
                if (exitCameras.Contains(CurrentCameraP2) && _client.LockCorrect)
                {
                    Debug.Log("GO OUT");
                }
                if (transitionCameras.IndexOf(CurrentCameraP2) >= 8)
                {
                    GoToTheRoom(-1, 2);
                }
                else
                {
                    GoToTheRoom(1, 2);
                }
                _client.ButtonP2 = 0;
            }
        }
        else
        {
            _client.IsDoorVisibleP2 = false;
        }
        //BOTH PLAYERS ZOOM IN IDK WHY
        if ((_client.ButtonClicked == 4 || _client.ButtonClicked == 5) && _client.PlayerNumber == 2)
        {
            if (zooms.ContainsKey(CurrentCameraP2))
            {
                zooms[CurrentCameraP2].ZoomCamera();
            }
        } else if (_client.ButtonClicked == 6 && _client.PlayerNumber == 2)
        {
            if (zooms.ContainsKey(CurrentCameraP2))
            {
                zooms[CurrentCameraP2].ZoomOutCamera();
            }
        }
    }

    private void SwitchToCamera(int cameraValue, int player)
    {
        if (player == 1)
        {
            //Client.Instance.CameraNumberP1 = _roomP1 % numberOfRooms + Math.Abs(_currentCameraIndexP1 + cameraValue) % _camerasInOneRoom;
            CurrentCameraP1 = ChooseCorrectCamera(camerasP1, cameraValue, _currentCameraIndexP1, _roomP1);
        }
        else if (player == 2)
        {
            //Client.Instance.CameraNumberP2 = _roomP2 % numberOfRooms + Math.Abs(_currentCameraIndexP2 + cameraValue) % _camerasInOneRoom;
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
            Client.Instance.RoomP1 = _roomP1;
        }
        else if (player == 2)
        {
            _roomP2 += room;
            Client.Instance.RoomP2 = _roomP2;
        }

        SetTransitionCamera(player);
    }
}