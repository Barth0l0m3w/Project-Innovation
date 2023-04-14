using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePhone : MonoBehaviour
{
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private List<GameObject> notes;
    [SerializeField] private GameObject safe;

    private Client _clientPhone;

    private void Start()
    {
        safe.SetActive(false);
        _clientPhone = Client.Instance;
    }

    private void Update()
    {
        if (_clientPhone.ShowDoorButton)
        {
            forwardButton.SetActive(true);
        }
        else
        {
            forwardButton.SetActive(false);
        }

        if (_clientPhone.PlayerNumber == 1)
        {
            switch (_clientPhone.RoomP1)
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
        
        if (_clientPhone.PlayerNumber == 2)
        {
            switch (_clientPhone.RoomP2)
            {
                case 0:
                    notes[1].SetActive(true);
                    notes[2].SetActive(false);
                    notes[3].SetActive(false);
                    break;
                case 2:
                    notes[2].SetActive(true);
                    notes[3].SetActive(false);
                    notes[1].SetActive(false);
                    break;
                case 4:
                    notes[3].SetActive(true);
                    notes[2].SetActive(false);
                    notes[1].SetActive(false);
                    break;
                default:
                    notes[3].SetActive(false);
                    notes[2].SetActive(false);
                    notes[1].SetActive(false);
                    break;
            }
        }
        
        if (Client.Instance.ButtonClicked == 4 && Client.Instance.RoomP1 == 0)
        {
            safe.SetActive(true);
        }

        if (_clientPhone.LockPickedPhone)
        {
            safe.SetActive(false);
        }
    }
}