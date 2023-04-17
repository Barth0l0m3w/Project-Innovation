using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesButtons : MonoBehaviour
{
    [SerializeField] private List<GameObject> positions;
    [SerializeField] private int modifier;
    [SerializeField] private bool isDefaultShown;

    private int _startingNumber;

    private void Update()
    {
        //TODO: TESSSTTTT
        if (gameObject.activeSelf)
        {
            _startingNumber = isDefaultShown ? 0 : 1;
            for (int i = _startingNumber; i < positions.Count + _startingNumber; i++)
            {
                Debug.Log("Player camera: " + Client.Instance.CameraNumberPlayer);
                Debug.Log("I to unlock note: " + (i * modifier));
                positions[i-_startingNumber].SetActive(i * modifier == Client.Instance.CameraNumberPlayer);
            }
        }
    }
}