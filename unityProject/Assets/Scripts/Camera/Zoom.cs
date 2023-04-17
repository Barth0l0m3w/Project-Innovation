using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Zoom : MonoBehaviour
{

    [SerializeField] private Vector3 position1;
    [SerializeField] private Vector3 rotation1;

    [SerializeField] private Vector3 position2;
    [SerializeField] private Vector3 rotation2;

    private Vector3 beginPosition;
    private Vector3 beginRotation;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;



    // Start is called before the first frame update
    void Start()
    {
        beginPosition = cinemachineVirtualCamera.transform.position;
        beginRotation = cinemachineVirtualCamera.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (cinemachineVirtualCamera != null)
        {
            HandleCameraZoom();
        }
    }

    private void HandleCameraZoom()
    {
        //To do Kama
        //instead of keycode, use the zoom button on phone
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            cinemachineVirtualCamera.transform.position = position1;
            cinemachineVirtualCamera.transform.eulerAngles = rotation1;
        }
        else if (position2 != null && Input.GetKeyDown(KeyCode.Alpha5))
        {
            cinemachineVirtualCamera.transform.position = position2;
            cinemachineVirtualCamera.transform.eulerAngles = rotation2;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            cinemachineVirtualCamera.transform.localPosition = beginPosition;
            cinemachineVirtualCamera.transform.eulerAngles = beginRotation;
        }
    }
}
