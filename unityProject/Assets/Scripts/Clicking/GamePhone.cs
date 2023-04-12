using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePhone : MonoBehaviour
{
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private bool isCameraForward;

    private void Update()
    {
        if (isCameraForward)
        {
            forwardButton.SetActive(true);
        }
        else
        {
            forwardButton.SetActive(false);
        }
    }
}
