using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GamePhone : MonoBehaviour
{
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private List<GameObject> notesP1;
    [SerializeField] private List<GameObject> notesP2;
    [SerializeField] protected List<GameObject> possibleInteractions;
    [SerializeField] private GameObject lockCellDoor;
    [SerializeField] private GameObject finalLock;
    [SerializeField] private GameObject safe;

    private Client _clientPhone;

    private void Start()
    {
        // lockCellDoor.SetActive(false);
        // finalLock.SetActive(false);
        // safe.SetActive(false);
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

        List<GameObject> usedNotes;
        if (_clientPhone.PlayerNumber == 1)
        {
            usedNotes = notesP1;
            Debug.Log("I am player 1");
        }
        else
        {
            usedNotes = notesP2;
            Debug.Log("I am player 2");
        }
        
        if (_clientPhone.PlayerNumber == 1)
        {
            for (int i = 0; i < notesP1.Count; i++)
            {
                notesP1[i].SetActive(_clientPhone.PlayerRoom == i);
            }
            
        }
        
        if (_clientPhone.PlayerNumber == 2)
        {
            for (int i = 0; i < notesP2.Count; i++)
            {
                notesP2[i].SetActive(_clientPhone.PlayerRoom == i);
            }
            
        }

        // //Add first only p1
        // foreach (GameObject interaction in possibleInteractions)
        // {
        //     _clientPhone.Interactions.Add(interaction,false);
        // }

        if (_clientPhone.ButtonClicked == 4)
        {
            //Make use of disctionary
        }
        

        // if (_clientPhone.PlayerNumber == 2)
        // {
        //     switch (_clientPhone.RoomP2)
        //     {
        //         case 0:
        //             notesP1[1].SetActive(true);
        //             notesP1[2].SetActive(false);
        //             notesP1[3].SetActive(false);
        //             break;
        //         case 2:
        //             notesP1[2].SetActive(true);
        //             notesP1[3].SetActive(false);
        //             notesP1[1].SetActive(false);
        //             break;
        //         case 4:
        //             notesP1[3].SetActive(true);
        //             notesP1[2].SetActive(false);
        //             notesP1[1].SetActive(false);
        //             break;
        //         default:
        //             notesP1[3].SetActive(false);
        //             notesP1[2].SetActive(false);
        //             notesP1[1].SetActive(false);
        //             break;
        //     }
        // }

        if (_clientPhone.ButtonClicked == 4 && _clientPhone.PlayerRoom == 0)
        {
            lockCellDoor.SetActive(true);
        }

        if (_clientPhone.LockPickedPhone)
        {
            lockCellDoor.SetActive(false);
        }

        // if (_clientPhone.ButtonClicked == 4 && _clientPhone.PlayerRoom == 2)
        // {
        //     finalLock.SetActive(false);
        // }
        //
        // if (_clientPhone.LockCorrect)
        // {
        //     Debug.Log("Congratulations");
        // }
        //
        // if (_clientPhone.ButtonClicked == 4 && _clientPhone.PlayerRoom == 3)
        // {
        //     safe.SetActive(true);
        // }
        //
        // if (_clientPhone.SafeOpened)
        // {
        //     safe.SetActive(false);
        // }
    }
}