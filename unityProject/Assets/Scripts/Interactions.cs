using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    [SerializeField] private List<GameObject> interactions;
    [SerializeField] private int modifier;
    [SerializeField] private bool isDefaultShown;

    private int _startingNumber;

    private void Update()
    {
        if (gameObject.activeSelf && Client.Instance.ButtonClicked == 4)
        {
            _startingNumber = isDefaultShown ? 0 : 1;
            for (int i = _startingNumber; i < interactions.Count + _startingNumber; i++)
            {
                interactions[i-_startingNumber].SetActive(i * modifier == Client.Instance.CameraNumberPlayer);
            }
        }
    }
}
