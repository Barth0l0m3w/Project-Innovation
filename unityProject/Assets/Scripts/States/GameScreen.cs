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
        private bool _lockPickFinished;
        private bool _puzzleSolved;

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Welcome to the game...");
        }

        private void Update()
        {
            ReceiveAndProcessNetworkMessages();
            //PLAYER 1 --------------------
            ProcessPlayer(1, Client.RoomP1,Client.IsDoorVisibleP1, ref _setActiveProcessedP1, ref _setInactiveProcessedP1,
                Client.LockPickedLaptop, ref _player1CameraUpdated);

            //PLAYER 2 ---------------------
            ProcessPlayer(2, Client.RoomP2,Client.IsDoorVisibleP2, ref _setActiveProcessedP2, ref _setInactiveProcessedP2,
                Client.LockPickedLaptop, ref _player2CameraUpdated);

            //OTHER FUNCTIONALITY
            if (Client.ButtonClicked != 0 && Client.ButtonClicked <= 3)
            {
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

            if (Client.LockCorrect && !_puzzleSolved)
            {
                FinalLock final = new FinalLock();
                final.Solved = true;
                Client.Channel.SendMessage(final);
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

            if (pMessage is FinalLock)
            {
                FinishGame();
            }
        }

        private void FinishGame()
        {
            _puzzleSolved = true;
            Client.LockCorrect = true;
        }

        private void ProcessPlayer(int player, int playerRoom, bool isDoorVisible, ref bool setActiveProcessed,
            ref bool setInactiveProcessed, bool lockPicked, ref bool cameraUpdated)
        {
            if (isDoorVisible && !setActiveProcessed && lockPicked)
            {
                DoorActive doorActive = new DoorActive();
                doorActive.IsActive = true;
                doorActive.Player = player;
                Client.Channel.SendMessage(doorActive);
                setActiveProcessed = true;
                setInactiveProcessed = false;
            }
            
            if (!isDoorVisible && !setInactiveProcessed && lockPicked)
            {
                DoorActive doorActive = new DoorActive();
                doorActive.IsActive = false;
                doorActive.Player = player;
                Client.Channel.SendMessage(doorActive);
                setActiveProcessed = false;
                setInactiveProcessed = true;
            }
            
            if (!cameraUpdated)
            {
                ShowNotes camera = new ShowNotes();
                camera.Player = player;
                camera.PlayerRoom = playerRoom;
                Client.Channel.SendMessage(camera);
                cameraUpdated = true;
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
            Client.PlayerRoom = pMessage.PlayerRoom;
            Client.PlayerNumber = pMessage.Player;
            Client.NewPuzzle = true;
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
            } else if (pMessage.Player == 3)
            {
                Client.CameraNumberPlayer = pMessage.Camera;
                Debug.Log("Camera used: " + Client.CameraNumberPlayer);
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