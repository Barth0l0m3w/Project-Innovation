using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CameraSwitcher
{

    private static List<CinemachineVirtualCamera> camerasP1 = new List<CinemachineVirtualCamera>();
    private static List<CinemachineVirtualCamera> camerasP2 = new List<CinemachineVirtualCamera>();

    private static CinemachineVirtualCamera ActiveCamera = null;

    public static bool IsActiveCamera(CinemachineVirtualCamera cam)
    {
        return cam == ActiveCamera;
    }

    public static void RegisterP1(CinemachineVirtualCamera cam)
    {
        cam.Priority = 10;
        camerasP1.Add(cam);
    }

    public static void UnregisterP1(CinemachineVirtualCamera cam)
    {
        cam.Priority = 0;
        camerasP1.Remove(cam);
    }
    
    public static void RegisterP2(CinemachineVirtualCamera cam)
    {
        cam.Priority = 10;
        camerasP2.Add(cam);
    }

    public static void UnregisterP2(CinemachineVirtualCamera cam)
    {
        cam.Priority = 0;
        camerasP2.Remove(cam);
    }
    
    
}
