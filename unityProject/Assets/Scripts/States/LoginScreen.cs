using System;
using shared;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/**
 * Starting state where you can connect to the server.
 */
public class LoginScreen : ClientState
{
    [Tooltip("Server's IP. Remember to change is also on the server!!!")]
    [SerializeField] private string serverIP = null;
    [Tooltip("Port on the laptop so we can connect. 55558 if we want to run it on kama's laptop")]
    [SerializeField] private int serverPort = 0;
    [Tooltip("Debug thing to check the device's type. In final version we will have auto detection :)")]
    [SerializeField] private int debugDeviceType;


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
            try
            {
                tryToJoinLobby();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.Log("Oops, couldn't connect:" + string.Join("\n", Client.Channel.GetErrors()));
                throw;
            }
        }
    }

    private void tryToJoinLobby()
    {
        PlayerJoinRequest playerJoinRequest = new PlayerJoinRequest();
        if (debugDeviceType == 0)               //(SystemInfo.deviceType == DeviceType.Desktop)
        {
            //LAPTOP
            playerJoinRequest.DeviceType = 0;
        }
        else
        {
            //PHONE
            playerJoinRequest.DeviceType = 1;
        }
        Client.Channel.SendMessage(playerJoinRequest);
    }
    
    private void Update()
    {
        //if we are connected, start processing messages
        if (Client.Channel.Connected) ReceiveAndProcessNetworkMessages();
    }


    protected override void HandleNetworkMessage(ASerializable pMessage)
    {
        if (pMessage is RoomJoinedEvent) HandleRoomJoinedEvent(pMessage as RoomJoinedEvent);
    }

    private void HandleRoomJoinedEvent(RoomJoinedEvent pMessage)
    {
        if (pMessage.room == RoomJoinedEvent.Room.LOBBY_ROOM)
        {
            Client.SetState<LobbyScreen>();
            Debug.Log("Client joined the lobby");
        }
    }
}