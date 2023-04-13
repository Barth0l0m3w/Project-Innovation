using shared;
using UnityEngine;

namespace States
{
    public class GameScreen : ClientState
    {
        private bool _setActiveProcessedP1;
        private bool _setInactiveProcessedP1;
        private bool _player1CameraUpdated;
        private bool _player2CameraUpdated;

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
                DoorActive doorActive = new DoorActive();
                doorActive.IsActive = true;
                doorActive.Player = 1;
                Client.Channel.SendMessage(doorActive);
                _setActiveProcessedP1 = true;
                _setInactiveProcessedP1 = false;
            }
            
            if (!Client.IsDoorVisibleP1 && !_setInactiveProcessedP1)
            {
                DoorActive doorActive = new DoorActive();
                doorActive.IsActive = false;
                doorActive.Player = 1;
                Client.Channel.SendMessage(doorActive);
                _setActiveProcessedP1 = false;
                _setInactiveProcessedP1 = true;
            }
            
            if (!_player1CameraUpdated)
            {
                ShowNotes camera = new ShowNotes();
                camera.Player = 1;
                camera.PlayerRoom = Client.RoomP1;
                Debug.Log("Player room: " + Client.RoomP1);
                Client.Channel.SendMessage(camera);
                _player1CameraUpdated = true;
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

            if (pMessage is ShowNotes)
            {
                ShowCorrectNotes(pMessage as ShowNotes);
            }
        }

        private void ShowCorrectNotes(ShowNotes pMessage)
        {
            //Client.Instance.CameraNumberP1 = pMessage.PlayerCamera;
            Client.Instance.RoomP1 = pMessage.PlayerRoom;
            Client.Instance.CameraNumberP1 = pMessage.Player;
        }

        private void SwitchCameras(ChooseCamera pMessage)
        {
            if (pMessage.Player == 1)
            {
                Client.ButtonP1 = pMessage.Camera;
                Debug.Log(Client.ButtonP1);
                _player1CameraUpdated = false;
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
            }
            else
            {
                Client.ShowDoorButton = false;
            }
        }
    }
}