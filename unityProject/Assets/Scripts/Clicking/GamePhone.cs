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
    [SerializeField] private GameObject cox;
    [SerializeField] private GameObject teddy;
    [SerializeField] private GameObject poster1;
    [SerializeField] private GameObject poster2;
    [SerializeField] private GameObject lockedDoorPrompt;
    [SerializeField] private GameObject manual;
    [SerializeField] private GameObject number;
    

    private Client _clientPhone;
    private List<GameObject> _usedNotes;
    private GameObject _activePuzzle;
    private bool _buttonClicked;

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

        if (_clientPhone.PlayerNumber == 1)
        {
            _usedNotes = notesP1;
            for (int i = 0; i < notesP1.Count; i++)
            {
                notesP1[i].SetActive(_clientPhone.PlayerRoom == i);
            }

            
            
             // if (_clientPhone.ButtonClicked == 4)
             // {
                 switch (_clientPhone.PlayerRoom)
                 {
                     case 0:
                         if (_clientPhone.ButtonClicked == 4&& _clientPhone.NewPuzzle) OpenPuzzle(lockedDoorPrompt);
                         break;
                     case 1:
                         if (_clientPhone.ButtonClicked == 4&& _clientPhone.NewPuzzle) OpenPuzzle(number);
                         break;
                     case 2:
                         if (_clientPhone.ButtonClicked == 4&& _clientPhone.NewPuzzle) OpenPuzzle(poster1);
                         break;
                     case 3: 
                         if (_clientPhone.ButtonClicked == 4&& _clientPhone.NewPuzzle) OpenPuzzle(finalLock);
                         break;
                     case 4: 
                         if (_clientPhone.ButtonClicked == 4&& _clientPhone.NewPuzzle) OpenPuzzle(cox);
                         break;
                 }
                 
            //}
            
            // if (_clientPhone.ButtonClicked == 5)
            // {
                switch (_clientPhone.PlayerRoom)
                {
                    case 0:
                        if (_clientPhone.ButtonClicked == 5 && _clientPhone.NewPuzzle) OpenPuzzle(lockCellDoor);
                        break;
                    case 4: 
                        if (_clientPhone.ButtonClicked == 5 && _clientPhone.NewPuzzle) OpenPuzzle(teddy);
                        break;
                }
                // }


            if (_clientPhone.PuzzleSolved)
            {
                SolvePuzzle(_activePuzzle);
            }
        }
        
        if (_clientPhone.PlayerNumber == 2)
        {
            _usedNotes = notesP2;
            for (int i = 0; i < notesP2.Count; i++)
            {
                notesP2[i].SetActive(_clientPhone.PlayerRoom == i);
            }
        }

        // if (_clientPhone.ButtonClicked == 4)
        // {
        //     //Make use of disctionary
        // }
        //
        // if (_clientPhone.ButtonClicked == 4 && _clientPhone.PlayerRoom == 0)
        // {
        //     lockCellDoor.SetActive(true);
        // }
        //
        // if (_clientPhone.LockPickedPhone)
        // {
        //     lockCellDoor.SetActive(false);
        // }

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
    
    private void SolvePuzzle(GameObject puzzle){
        puzzle.SetActive(false);
    }

    private void OpenPuzzle(GameObject puzzle)
    {
        _clientPhone.NewPuzzle = false;
        puzzle.SetActive(true);
        _activePuzzle = puzzle;
    }
}