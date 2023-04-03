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
        //addOutput("(noone else will see this because I broke the chat on purpose):"+pText);        
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
            Debug.Log("Pressing A");
            ChoosePlayer choose = new ChoosePlayer();
            choose.characterID = 1;
            Client.Channel.SendMessage(choose);
            // ChangeReadyStatusRequest msg = new ChangeReadyStatusRequest();
            // msg.ready = true;
            // msg.characterID = 1;
            // Client.Channel.SendMessage(msg);
            //TODO: Check if available
        } else if (Client.PlayerTwoClicked)
        {
            Debug.Log("Pressing B");
            ChoosePlayer choose = new ChoosePlayer();
            choose.characterID = 2;
            Client.Channel.SendMessage(choose);
            // ChangeReadyStatusRequest msg = new ChangeReadyStatusRequest();
            // msg.ready = true;
            // msg.characterID = 2;
            // Client.Channel.SendMessage(msg);
        }
    }
    
    protected override void handleNetworkMessage(ASerializable pMessage)
    {
        if (pMessage is ChatMessage) handleChatMessage(pMessage as ChatMessage);
        else if (pMessage is RoomJoinedEvent) handleRoomJoinedEvent(pMessage as RoomJoinedEvent);
        else if (pMessage is LobbyInfoUpdate) handleLobbyInfoUpdate(pMessage as LobbyInfoUpdate);
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
            //Client.SetState(GameState);
        }
    }

    private void handleLobbyInfoUpdate(LobbyInfoUpdate pMessage)
    {
        Debug.Log("I received something");
        if (pMessage.characterID == 1)
        {
            Debug.Log("Player one is chosen!");
            Client.IsPlayerOneReady = true;
        }
        else
        {
            Debug.Log("I cannot read");
        }
    }

}
