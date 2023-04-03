using System;
using shared;
using States;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * 'Chat' state while you are waiting to start a game where you can signal that you are ready or not.
 */
public class LobbyScreen : ClientState
{
    private bool _isPlayerReady;
    private int _playerNumber;
    private bool _playerProcessed;

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Welcome to the Lobby...");
    }
    
    private void Update()
    {
        ReceiveAndProcessNetworkMessages();
        if (Client.PlayerOneClicked && !_playerProcessed)
        {
            ChoosePlayerMessage(1);
            Client.PlayerOneClicked = false;
        } 
        else if (Client.PlayerTwoClicked && !_playerProcessed)
        {
            ChoosePlayerMessage(2);
            Client.PlayerTwoClicked = false;
        }
    }

    /// <summary>
    /// Creates new ChoosePlayer packet depending on what player was clicked
    /// </summary>
    private void ChoosePlayerMessage(int playerID)
    {
        _playerProcessed = true;
        ChoosePlayer choose = new ChoosePlayer();
        choose.characterID = playerID;
        Client.Channel.SendMessage(choose);
    }
    
    protected override void HandleNetworkMessage(ASerializable pMessage)
    {
        if (pMessage is LobbyInfoUpdate) HandleLobbyInfoUpdate(pMessage as LobbyInfoUpdate);
        else if (pMessage is CharacterNotAccepted) HandleNotAccepted(pMessage as CharacterNotAccepted);
        else if (pMessage is ChangeReadyStatusRequest) UpdateReady(pMessage as ChangeReadyStatusRequest);
        else if (pMessage is GoToNextScene) ChangeScene(pMessage as GoToNextScene);
    }

    /// <summary>
    /// Changes the unity scene when players join the game
    /// </summary>
    private void ChangeScene(GoToNextScene pMessage)
    {
        if (pMessage.goToScene)
        {
            //THIS NUMBER WILL CHANGE!!!
            SceneManager.LoadScene(2);
            Client.SetState<GameScreen>();
        }
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

    /// <summary>
    /// If player does not get accepted (the character is already picked) let him choose again
    /// </summary>
    private void HandleNotAccepted(CharacterNotAccepted pMessage)
    {
        _playerProcessed = pMessage.NotAccepted;
    }

    /// <summary>
    /// Used to show which character got already chosen on the laptop
    /// </summary>]
    private void HandleLobbyInfoUpdate(LobbyInfoUpdate pMessage)
    {
        if (pMessage.characterID == 1 && !Client.IsPlayerOneReady)
        {
            Client.IsPlayerOneReady = true;
        }
        else if (pMessage.characterID == 2 && !Client.IsPlayerTwoReady)
        {
            Client.IsPlayerTwoReady = true;
        }
    }

}
