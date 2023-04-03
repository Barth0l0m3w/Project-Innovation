using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CameraSwitcher
{

    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    static CinemachineVirtualCamera ActiveCamera = null;

    public static bool IsActiveCamera(CinemachineVirtualCamera cam)
    {
        return cam == ActiveCamera;
    }

    public static void Register(CinemachineVirtualCamera cam)
    {
        cam.Priority = 10;
        cameras.Add(cam);
    }

    public static void Unregister(CinemachineVirtualCamera cam)
    {
        cam.Priority = 0;
        cameras.Remove(cam);
    }
}
