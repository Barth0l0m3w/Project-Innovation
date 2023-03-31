using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeIconScript : MonoBehaviour
{
    [SerializeField] private Image player1;
    [SerializeField] private Image player2;

    private void Update()
    {
        if (Client.Instance.IsPlayerOneReady)
        {
            player1.color = new Color(0, 1, 0);
        }
        else
        {
            player1.color = new Color(1, 1, 1);
        }
        
        if (Client.Instance.IsPlayerTwoReady)
        {
            player2.color = new Color(0, 1, 0);
        }
        else
        {
            player2.color = new Color(1, 1, 1);
        }
    }
}
