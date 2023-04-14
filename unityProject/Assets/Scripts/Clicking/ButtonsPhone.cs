using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void GoToGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MoveLeft()
    {
        Client.Instance.ButtonClicked = 1;
    }

    public void MoveForward()
    {
        Client.Instance.ButtonClicked = 2;
    }

    public void MoveRight()
    {
        Client.Instance.ButtonClicked = 3;
    }

    public void Note1()
    {
        Client.Instance.ButtonClicked = 4;
        Debug.Log("4 clicked");
    }
    public void Note2()
    {
        Client.Instance.ButtonClicked = 5;
    }
    public void Note3()
    {
        Client.Instance.ButtonClicked = 6;
    }
    public void Note4()
    {
        Client.Instance.ButtonClicked = 7;
    }
}
