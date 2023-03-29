using shared;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/**
 * Starting state where you can connect to the server.
 */
public class LoginScreen : ClientState
{
    [SerializeField] private string serverIP = null;
    [SerializeField] private int serverPort = 0;

    private int _deviceType;


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
        if (Client.Channel.Connect(serverIP, serverPort))
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
        PlayerJoinRequest playerJoinRequest = new PlayerJoinRequest();
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            playerJoinRequest.DeviceType = 0;
        }
        else
        {
            playerJoinRequest.DeviceType = 1;
        }
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
        Debug.Log("Player join response yes");
    }

    private void handleRoomJoinedEvent(RoomJoinedEvent pMessage)
    {
        if (pMessage.room == RoomJoinedEvent.Room.LOBBY_ROOM)
        {
            Client.SetState<LobbyScreen>();
            Debug.Log("Client joined the lobby");
        }
    }
}