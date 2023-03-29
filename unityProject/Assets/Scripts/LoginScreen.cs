using shared;
using System.Collections.Generic;
using UnityEngine;

/**
 * Starting state where you can connect to the server.
 */
public class LoginScreen : ClientState
{
    [SerializeField] private string _serverIP = null;
    [SerializeField] private int _serverPort = 0;
    [SerializeField] private int _sceneNumber = 1;


    public override void EnterState()
    {
        base.EnterState();
        Connect();
    }

    /**
     * Connect to the server (with some client side validation)
     */
    private void Connect()
    {
        Debug.Log("Trying to connect");

        //connect to the server and on success try to join the lobby
        if (Client.Channel.Connect(_serverIP, _serverPort))
        {
            tryToJoinLobby();
        }
        else
        {
            Debug.Log("Oops, couldn't connect:" + string.Join("\n", Client.Channel.GetErrors()));
        }
    }

    private void tryToJoinLobby()
    {
        //Construct a player join request based on the user name 
        PlayerJoinRequest playerJoinRequest = new PlayerJoinRequest();
        playerJoinRequest.name = "player";
        Client.Channel.SendMessage(playerJoinRequest);
    }

    /// //////////////////////////////////////////////////////////////////
    ///                     NETWORK MESSAGE PROCESSING
    /// //////////////////////////////////////////////////////////////////
    private void Update()
    {
        //if we are connected, start processing messages
        if (Client.Channel.Connected) receiveAndProcessNetworkMessages();
    }


    protected override void handleNetworkMessage(ASerializable pMessage)
    {
        if (pMessage is PlayerJoinResponse) handlePlayerJoinResponse(pMessage as PlayerJoinResponse);
        else if (pMessage is RoomJoinedEvent) handleRoomJoinedEvent(pMessage as RoomJoinedEvent);
    }


    private void handlePlayerJoinResponse(PlayerJoinResponse pMessage)
    {
        //Dont do anything with this info at the moment, just leave it to the RoomJoinedEvent
        //We could handle duplicate name messages, get player info etc here

        
    }

    private void handleRoomJoinedEvent(RoomJoinedEvent pMessage)
    {
        if (pMessage.room == RoomJoinedEvent.Room.LOBBY_ROOM)
        {
            Client.SetState<LobbyScreen>();
            Debug.Log("Client joined the room");
        }
    }
}