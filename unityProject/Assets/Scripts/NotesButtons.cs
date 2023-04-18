using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesButtons : MonoBehaviour
{
    [SerializeField] private List<GameObject> positions;
    [SerializeField] private List<GameObject> interactions;
    [SerializeField] private int modifier;
    [SerializeField] private bool isDefaultShown;

    private int _startingNumber;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            _startingNumber = isDefaultShown ? 0 : 1;
            for (int i = _startingNumber; i < positions.Count + _startingNumber; i++)
            {
                positions[i-_startingNumber].SetActive(i * modifier == Client.Instance.CameraNumberPlayer);
                
            }
        }
    }
}