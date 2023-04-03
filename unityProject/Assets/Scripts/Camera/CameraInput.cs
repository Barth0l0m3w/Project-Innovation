using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraInput : MonoBehaviour
{
    public List<CinemachineVirtualCamera> camerasP1;
    private int currentCameraIndexP1 = 0;

    public CinemachineVirtualCamera CurrentCamera
    {
        get { return camerasP1[currentCameraIndexP1]; }
        set
        {
            //deactivate current camera
            CameraSwitcher.Unregister(camerasP1[currentCameraIndexP1]);

            //get the index of the new camera
            int newIndex = camerasP1.IndexOf(value);

            //check is the camera is falid
            if (newIndex >= 0 && newIndex < camerasP1.Count)
            {
                //set the new camera index and activate the new camera
                currentCameraIndexP1 = newIndex;
                CameraSwitcher.Register(camerasP1[newIndex]);
            }
            else
            {
                Debug.Log("invalif camera selection");
            }
        }
    }

    private void Start()
    {
        CinemachineVirtualCamera[] allCameras = FindObjectsOfType<CinemachineVirtualCamera>();

        //filter the cameras by label "Player1" and add them to the list
        foreach (CinemachineVirtualCamera camera in allCameras)
        {
            if (camera.gameObject.CompareTag("Player1"))
            {
                camerasP1.Add(camera);
            }
        }

        //activate the 1st camera in the list
        if (camerasP1.Count > 0)
        {
            CurrentCamera = camerasP1[currentCameraIndexP1];
        }
    }

    void Update()
    {
        // Check for keyboard input to cycle through the cameras
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Set the current camera to the next camera in the list
            CurrentCamera = camerasP1[(currentCameraIndexP1 + 1) % camerasP1.Count];
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Set the current camera to the previous camera in the list
            CurrentCamera = camerasP1[(currentCameraIndexP1 - 1 + camerasP1.Count) % camerasP1.Count];
        }
    }
}
