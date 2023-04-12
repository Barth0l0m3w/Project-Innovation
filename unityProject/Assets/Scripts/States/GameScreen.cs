using shared;
using UnityEngine;

namespace States
{
    public class GameScreen : ClientState
    {
        private bool _setActiveProcessedP1;
        private bool _setInactiveProcessedP2;
        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Welcome to the game...");
        }
        
        private void Update()
        {
            ReceiveAndProcessNetworkMessages();
            if (Client.IsDoorVisibleP1 && !_setActiveProcessedP1)
            {
                Debug.Log("Player 1 sees the door");
                DoorActive doorActive = new DoorActive();
                doorActive.IsActive = true;
                doorActive.Player = 1;
                Client.Channel.SendMessage(doorActive);
                _setActiveProcessedP1 = true;
                _setInactiveProcessedP2 = false;
            }
            // else if (!Client.IsDoorVisibleP1 && !_setInactiveProcessedP2)
            // {
            //     DoorActive doorActive = new DoorActive();
            //     doorActive.IsActive = false;
            //     doorActive.Player = 1;
            //     Client.Channel.SendMessage(doorActive);
            //     _setActiveProcessedP1 = false;
            //     _setInactiveProcessedP2 = true;
            // }
        }
        
        protected override void HandleNetworkMessage(ASerializable pMessage)
        {
            if (pMessage is GameFinished)
            {
                handleGameFinished(pMessage as GameFinished);
            }

            if (pMessage is DoorActive)
            {
                Debug.Log("Door is active");
            }
        }
        
        private void handleGameFinished(GameFinished pMessage)
        {
            PlayerJoinRequest join = new PlayerJoinRequest();
            Client.Channel.SendMessage(join);
            Client.SetState<LobbyScreen>();
        }
    }
}