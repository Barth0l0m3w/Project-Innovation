using shared;
using UnityEngine;
using UnityEngine.Serialization;

namespace States
{
    public class GameScreen : ClientState
    {
        private bool _setActiveProcessedP1;
        private bool _setInactiveProcessedP1;
        private bool _setActiveProcessedP2;
        private bool _setInactiveProcessedP2;
        private bool _player1CameraUpdated;
        private bool _player2CameraUpdated;
        private bool _lockPickFinished = false;

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Welcome to the game...");
        }

        private void Update()
        {
            ReceiveAndProcessNetworkMessages();
            //PLAYER 1 --------------------
            if (Client.IsDoorVisibleP1 && !_setActiveProcessedP1 && Client.LockPickedLaptop)
            {
                DoorActive doorActive = new DoorActive();
                doorActive.IsActive = true;
                doorActive.Player = 1;
                Client.Channel.SendMessage(doorActive);
                _setActiveProcessedP1 = true;
                _setInactiveProcessedP1 = false;
            }
            
            if (!Client.IsDoorVisibleP1 && !_setInactiveProcessedP1 && Client.LockPickedLaptop)
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
                Client.Channel.SendMessage(camera);
                _player1CameraUpdated = true;
            }
            
            //PLAYER 2 ---------------------
            
            if (Client.IsDoorVisibleP2 && !_setActiveProcessedP2 && Client.LockPickedLaptop)
            {
                DoorActive doorActive = new DoorActive();
                doorActive.IsActive = true;
                doorActive.Player = 2;
                Client.Channel.SendMessage(doorActive);
                _setActiveProcessedP2 = true;
                _setInactiveProcessedP2 = false;
            }
            
            if (!Client.IsDoorVisibleP2 && !_setInactiveProcessedP2 && Client.LockPickedLaptop)
            {
                DoorActive doorActive = new DoorActive();
                doorActive.IsActive = false;
                doorActive.Player = 2;
                Client.Channel.SendMessage(doorActive);
                _setActiveProcessedP2 = false;
                _setInactiveProcessedP2 = true;
            }
            
            if (!_player2CameraUpdated)
            {
                ShowNotes camera = new ShowNotes();
                camera.Player = 2;
                camera.PlayerRoom = Client.RoomP2;
                Client.Channel.SendMessage(camera);
                _player2CameraUpdated = true;
            }

            //OTHER FUNCTIONALITY
            if (Client.ButtonClicked != 0 && Client.ButtonClicked <= 3)
            {
                Debug.Log("BUTTON CLICKED");
                Debug.Log(Client.ButtonClicked);
                ChooseCamera camera = new ChooseCamera();
                camera.Camera = Client.ButtonClicked;
                Client.Channel.SendMessage(camera);
                Client.ButtonClicked = 0;
            }

            if (Client.LockPickedPhone && !_lockPickFinished)
            {
                LockPickedStatus lockPick = new LockPickedStatus();
                lockPick.IsLockPicked = true;
                Client.Channel.SendMessage(lockPick);
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

            if (pMessage is LockPickedStatus)
            {
                UnlockTheDoors();
            }
        }

        private void UnlockTheDoors()
        {
            _lockPickFinished = true;
            Client.LockPickedLaptop = true;
            Client.LockPickedPhone = true;
        }

        private void ShowCorrectNotes(ShowNotes pMessage)
        {
            Client.Instance.RoomP1 = pMessage.PlayerRoom;
            Client.Instance.CameraNumberP1 = pMessage.Player;
        }

        private void SwitchCameras(ChooseCamera pMessage)
        {
            if (pMessage.Player == 1)
            {
                Client.ButtonP1 = pMessage.Camera;
                _player1CameraUpdated = false; //only laptop sees it, the phone needs to see it somehow else
            } else if (pMessage.Player == 2)
            {
                Debug.Log("Player2 camera moving");
                Client.ButtonP2 = pMessage.Camera;
                _player2CameraUpdated = false; //only laptop sees it, the phone needs to see it somehow else
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