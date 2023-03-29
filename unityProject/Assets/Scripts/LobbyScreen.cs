using shared;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * 'Chat' state while you are waiting to start a game where you can signal that you are ready or not.
 */
public class LobbyScreen : ClientState
{
    [Tooltip("Should we enter the lobby in a ready state or not?")]
    [SerializeField] private bool autoQueueForGame = false;

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Welcome to the Lobby...");
    }

    public override void ExitState()
    {
        base.ExitState();
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
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("Pressing A");
            ChoosePlayer choose = new ChoosePlayer();
            choose.characterID = 1;
            Client.Channel.SendMessage(choose);
            ChangeReadyStatusRequest msg = new ChangeReadyStatusRequest();
            msg.ready = true;
            msg.characterID = 1;
            Client.Channel.SendMessage(msg);
        } else if (Input.GetKeyUp(KeyCode.B))
        {
            Debug.Log("Pressing B");
            ChoosePlayer choose = new ChoosePlayer();
            choose.characterID = 2;
            Client.Channel.SendMessage(choose);
            ChangeReadyStatusRequest msg = new ChangeReadyStatusRequest();
            msg.ready = true;
            msg.characterID = 2;
            Client.Channel.SendMessage(msg);
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
        //update the lobby heading
        //view.SetLobbyHeading($"Welcome to the Lobby ({pMessage.memberCount} people, {pMessage.readyCount} ready)");
        //Put it in the game scene
        SceneManager.LoadScene(pMessage.sceneNumber);
    }

}
