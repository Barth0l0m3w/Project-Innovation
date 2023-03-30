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
        return cam = ActiveCamera;
    }

    public static void SwitchCamera(CinemachineVirtualCamera cam)
    {
        cam.Priority = 10;
        ActiveCamera = cam;

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c != cam && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineVirtualCamera cam)
    {
        cameras.Add(cam);
        Debug.Log("camera Regisers" + cam);
    }

    public static void Unregister(CinemachineVirtualCamera cam)
    {
        cameras.Remove(cam);
        Debug.Log("camera Unregisers" + cam);
    }


}
