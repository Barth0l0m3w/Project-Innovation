using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraInput : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera view1;
    [SerializeField] CinemachineVirtualCamera view2;

    private void OnEnable()
    {
        CameraSwitcher.Register(view1);
        CameraSwitcher.Register(view2);

        CameraSwitcher.SwitchCamera(view1);
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(view1);
        CameraSwitcher.Unregister(view2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CameraSwitcher.IsActiveCamera(view1))
            {
                CameraSwitcher.SwitchCamera(view2);
            }
            else if(CameraSwitcher.IsActiveCamera(view2))
            {
                CameraSwitcher.SwitchCamera(view1);
            }
        }
    }
}
