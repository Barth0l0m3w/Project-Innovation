using System;
using shared;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
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

    public override void ExitState()
    {
        base.ExitState();
    }

    private void Start()
    {
    }

    /**
     * Called when you enter text and press enter.
     */
    private void onTextEntered(string pText)
    {
        ChatMessage message = new ChatMessage();
        message.message = pText;
        Client.Channel.SendMessage(message);      
    }

    /**
     * Called when you click on the ready checkbox
     */
    private void onReadyToggleClicked(bool pNewValue)
    {
        ChangeReadyStatusRequest msg = new ChangeReadyStatusRequest();
        msg.ready = pNewValue;
        Client.Channel.SendMessage(msg);
    }

    private void addOutput(string pInfo)
    {
        //view.AddOutput(pInfo);
    }

    /// //////////////////////////////////////////////////////////////////
    ///                     NETWORK MESSAGE PROCESSING
    /// //////////////////////////////////////////////////////////////////

    private void Update()
    {
        receiveAndProcessNetworkMessages();
        if (Client.PlayerOneClicked && !_playerProcessed)
        {
            _playerProcessed = true;
            ChoosePlayer choose = new ChoosePlayer();
            choose.characterID = 1;
            Client.Channel.SendMessage(choose);
        } else if (Client.PlayerTwoClicked && !_playerProcessed)
        {
            _playerProcessed = true;
            ChoosePlayer choose = new ChoosePlayer();
            choose.characterID = 2;
            Client.Channel.SendMessage(choose);
        }
    }
    
    protected override void handleNetworkMessage(ASerializable pMessage)
    {
        if (pMessage is ChatMessage) handleChatMessage(pMessage as ChatMessage);
        else if (pMessage is RoomJoinedEvent) handleRoomJoinedEvent(pMessage as RoomJoinedEvent);
        else if (pMessage is LobbyInfoUpdate) handleLobbyInfoUpdate(pMessage as LobbyInfoUpdate);
        else if (pMessage is CharacterNotAccepted) handleNotAccepted(pMessage as CharacterNotAccepted);
        else if (pMessage is ChangeReadyStatusRequest) updateReady(pMessage as ChangeReadyStatusRequest);
        else if (pMessage is GoToNextScene) changeScene(pMessage as GoToNextScene);
    }

    private void changeScene(GoToNextScene pMessage)
    {
        if (pMessage.goToScene)
        {
            Debug.Log("AAAAAAAAAAA");
        }
    }

    private void updateReady(ChangeReadyStatusRequest pMessage)
    {
        ChangeReadyStatusRequest changeReadyStatusRequest = new ChangeReadyStatusRequest();
        changeReadyStatusRequest.ready = pMessage.ready;
        changeReadyStatusRequest.characterID = pMessage.characterID;
        Client.Channel.SendMessage(changeReadyStatusRequest);
    }

    private void handleNotAccepted(CharacterNotAccepted pMessage)
    {
        _playerProcessed = pMessage.NotAccepted;
    }

    private void handleChatMessage(ChatMessage pMessage)
    {
        //just show the message
        addOutput(pMessage.message);
    }

    private void handleRoomJoinedEvent(RoomJoinedEvent pMessage)
    {
        //did we move to the game room?
        if (pMessage.room == RoomJoinedEvent.Room.GAME_ROOM)
        {
            Debug.Log("Starting the game");
            //SceneManager.LoadScene(2);
            //Client.SetState(GameState);
        }
    }

    private void handleLobbyInfoUpdate(LobbyInfoUpdate pMessage)
    {
        Debug.Log("I received something");
        Debug.Log(pMessage.ready);
        if (pMessage.characterID == 1 && !Client.IsPlayerOneReady)
        {
            Client.IsPlayerOneReady = true;
        }
        else if (pMessage.characterID == 2 && !Client.IsPlayerTwoReady)
        {
            Client.IsPlayerTwoReady = true;
        }

        if (pMessage.ready)
        {
            SceneManager.LoadScene(2);
            Debug.Log("Going to the next scene!");
        }
    }

}
