using System;
using shared;
using States;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * 'Chat' state while you are waiting to start a game where you can signal that you are ready or not.
 */
public class EndScreen : ClientState
{
    private bool _isPlayerReady;
    private int _playerNumber;
    private bool _playerProcessed;

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Welcome to the Lobby...");
        SceneManager.LoadScene(3);
    }
    
    private void Update()
    {
        ReceiveAndProcessNetworkMessages();
    }

   
    
    
    protected override void HandleNetworkMessage(ASerializable pMessage)
    {
       
         if (pMessage is ChangeReadyStatusRequest) UpdateReady(pMessage as ChangeReadyStatusRequest);
    }

    

    /// <summary>
    /// Updates the ready status so the server can react
    /// </summary>
    private void UpdateReady(ChangeReadyStatusRequest pMessage)
    {
        ChangeReadyStatusRequest changeReadyStatusRequest = new ChangeReadyStatusRequest();
        changeReadyStatusRequest.ready = pMessage.ready;
        changeReadyStatusRequest.characterID = pMessage.characterID;
        Client.Channel.SendMessage(changeReadyStatusRequest);
    }
    

}
