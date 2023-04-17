using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeIconScript : MonoBehaviour
{
    [SerializeField] private Image p1Gretel;
    [SerializeField] private Image p2Hansel;

    private void Update()
    {
        if (Client.Instance.IsPlayerOneReady)
        {
            p1Gretel.gameObject.SetActive(true);
        }
        else
        {
            p1Gretel.gameObject.SetActive(false);
        }
        
        if (Client.Instance.IsPlayerTwoReady)
        {
            p2Hansel.gameObject.SetActive(true);
        }
        else
        {
            p2Hansel.gameObject.SetActive(false);
        }
    }
}
