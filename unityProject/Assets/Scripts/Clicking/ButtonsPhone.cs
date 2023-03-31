using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPhone : MonoBehaviour
{
    public void PlayerOneReady()
    {
        Client.Instance.PlayerOneClicked = true;
    }

    public void PlayerTwoReady()
    {
        Client.Instance.PlayerTwoClicked = true;
    }
}
