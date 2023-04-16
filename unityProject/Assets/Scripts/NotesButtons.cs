using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesButtons : MonoBehaviour
{
    [SerializeField] private List<GameObject> positions;
    [SerializeField] private int modifier;
    [SerializeField] private bool isDefaultShown;

    private void Update()
    {
        //TODO: TESSSTTTT
        if (gameObject.activeSelf)
        {
            for (int i = isDefaultShown ? 1 : 0; i < positions.Count; i++)
            {
                positions[i].SetActive(i * modifier == Client.Instance.CameraNumberP1);
            }
        }
    }
}