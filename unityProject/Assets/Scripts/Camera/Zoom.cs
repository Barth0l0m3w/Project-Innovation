using System;
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

    private Vector3 _beginPosition;
    private Vector3 _beginRotation;

    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponentInParent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _beginPosition = _cinemachineVirtualCamera.transform.position;
        _beginRotation = _cinemachineVirtualCamera.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cinemachineVirtualCamera != null)
        {
            HandleCameraZoom();
        }
    }

    private void HandleCameraZoom()
    {
        //To do Kama
        //instead of keycode, use the zoom button on phone
        if (Input.GetKeyDown(KeyCode.Alpha4) || Client.Instance.ButtonClicked == 4 || Client.Instance.ButtonClicked == 5)
        {
            _cinemachineVirtualCamera.transform.position = position1;
            _cinemachineVirtualCamera.transform.eulerAngles = rotation1;
        }
        
        else
        {
            _cinemachineVirtualCamera.transform.localPosition = _beginPosition;
            _cinemachineVirtualCamera.transform.eulerAngles = _beginRotation;
        }
    }
}
