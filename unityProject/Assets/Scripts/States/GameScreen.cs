using shared;
using UnityEngine;

namespace States
{
    public class GameScreen : ClientState
    {
        private bool _setActiveProcessedP1;
        private bool _setInactiveProcessedP1;

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
                _setInactiveProcessedP1 = false;
            }
            
            if (!Client.IsDoorVisibleP1 && !_setInactiveProcessedP1)
            {
                Debug.Log("Player one does not see the door");
                DoorActive doorActive = new DoorActive();
                doorActive.IsActive = false;
                doorActive.Player = 1;
                Client.Channel.SendMessage(doorActive);
                _setActiveProcessedP1 = false;
                _setInactiveProcessedP1 = true;
            }
            

            if (Client.ButtonClicked != 0)
            {
                ChooseCamera camera = new ChooseCamera();
                camera.Camera = Client.ButtonClicked;
                Client.Channel.SendMessage(camera);
                Client.ButtonClicked = 0;
            }
        }

        protected override void HandleNetworkMessage(ASerializable pMessage)
        {
            if (pMessage is GameFinished)
            {
                handleGameFinished(pMessage as GameFinished);
            }

            if (pMessage is DoorActive)
            {
                HandleDoorActive(pMessage as DoorActive);
            }

            if (pMessage is ChooseCamera)
            {
                SwitchCameras(pMessage as ChooseCamera);
            }
        }

        private void SwitchCameras(ChooseCamera pMessage)
        {
            if (pMessage.Player == 1)
            {
                Client.ButtonP1 = pMessage.Camera;
            }
        }

        private void handleGameFinished(GameFinished pMessage)
        {
            PlayerJoinRequest join = new PlayerJoinRequest();
            Client.Channel.SendMessage(join);
            Client.SetState<LobbyScreen>();
        }

        private void HandleDoorActive(DoorActive pMessage)
        {
            if (pMessage.IsActive)
            {
                Client.ShowDoorButton = true;
                Debug.Log("SHOW BUTTON");
            }
            else
            {
                Client.ShowDoorButton = false;
                Debug.Log("DO NOT SHOW BUTTON");
            }
        }
    }
}