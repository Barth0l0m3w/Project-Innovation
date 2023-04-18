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
        //_beginPosition = _cinemachineVirtualCamera.transform.position;
        _beginPosition = new Vector3(0, 0, 0);
        _beginRotation = _cinemachineVirtualCamera.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // if (_cinemachineVirtualCamera != null && _cinemachineVirtualCamera.m_Priority == 10)
        // {
        //     HandleCameraZoom();
        // }
    }

    public void HandleCameraZoom()
    {
        //To do Kama
        //instead of keycode, use the zoom button on phone
        
    }

    public void ZoomCamera()
    {
        
            _cinemachineVirtualCamera.transform.position = position1;
            _cinemachineVirtualCamera.transform.eulerAngles = rotation1;
        
    }

    public void ZoomOutCamera()
    {
        _cinemachineVirtualCamera.transform.localPosition = _beginPosition;
        _cinemachineVirtualCamera.transform.eulerAngles = _beginRotation;
    }
}
