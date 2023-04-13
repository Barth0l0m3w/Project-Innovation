using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePhone : MonoBehaviour
{
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private List<GameObject> notes;

    private void Update()
    {
        if (Client.Instance.ShowDoorButton)
        {
            forwardButton.SetActive(true);
        }
        else
        {
            forwardButton.SetActive(false);
        }

        if (Client.Instance.CameraNumberP1 == 1)
        {
            switch (Client.Instance.RoomP1)
            {
                case 0:
                    notes[0].SetActive(true);
                    notes[2].SetActive(false);
                    notes[4].SetActive(false);
                    break;
                case 2:
                    notes[2].SetActive(true);
                    notes[4].SetActive(false);
                    notes[0].SetActive(false);
                    break;
                case 4:
                    notes[4].SetActive(true);
                    notes[2].SetActive(false);
                    notes[0].SetActive(false);
                    break;
                default:
                    notes[4].SetActive(false);
                    notes[2].SetActive(false);
                    notes[0].SetActive(false);
                    break;
            }
        }
    }
}