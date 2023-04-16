using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GamePhone : MonoBehaviour
{
    [SerializeField] private GameObject forwardButton;
   [SerializeField] private List<GameObject> notesP1;
    [SerializeField] private List<GameObject> notesP2;
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
                    notesP1[0].SetActive(true);
                    notesP1[2].SetActive(false);
                    notesP1[4].SetActive(false);
                    break;
                case 2:
                    notesP1[2].SetActive(true);
                    notesP1[4].SetActive(false);
                    notesP1[0].SetActive(false);
                    break;
                case 4:
                    notesP1[4].SetActive(true);
                    notesP1[2].SetActive(false);
                    notesP1[0].SetActive(false);
                    break;
                default:
                    notesP1[4].SetActive(false);
                    notesP1[2].SetActive(false);
                    notesP1[0].SetActive(false);
                    break;
            }
        }
        
        if (_clientPhone.PlayerNumber == 2)
        {
            switch (_clientPhone.RoomP2)
            {
                case 0:
                    notesP1[1].SetActive(true);
                    notesP1[2].SetActive(false);
                    notesP1[3].SetActive(false);
                    break;
                case 2:
                    notesP1[2].SetActive(true);
                    notesP1[3].SetActive(false);
                    notesP1[1].SetActive(false);
                    break;
                case 4:
                    notesP1[3].SetActive(true);
                    notesP1[2].SetActive(false);
                    notesP1[1].SetActive(false);
                    break;
                default:
                    notesP1[3].SetActive(false);
                    notesP1[2].SetActive(false);
                    notesP1[1].SetActive(false);
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