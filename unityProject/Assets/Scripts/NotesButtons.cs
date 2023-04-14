using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesButtons : MonoBehaviour
{
    [SerializeField] private List<GameObject> positions;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            
                for (int i = 0; i < positions.Count; i++)
                {
                    if (i == Client.Instance.CameraNumberP1)
                    {
                        positions[i].SetActive(true);
                    }
                    else
                    {
                        positions[i].SetActive(false);
                    }
                }
            
        }
    }
}
