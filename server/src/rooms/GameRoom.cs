using shared;
using System;
using System.Dynamic;

namespace server
{
    /**
	 * This room runs a single Game (at a time). 
	 * 
	 * The 'Game' is very simple at the moment:
	 *	- all client moves are broadcasted to all clients
	 *	
	 * The game has no end yet (that is up to you), in other words:
	 * all players that are added to this room, stay in here indefinitely.
	 */
    class GameRoom : SimpleRoom
    {
        public bool IsGameInPlay { get; private set; }
        private TcpMessageChannel _player1;
        private TcpMessageChannel _player2;
        private TcpMessageChannel _laptop;
        private int _startCameraP1;
        private int _startCameraP2;

        //wraps the board to play on...
        private TicTacToeBoard _board = new TicTacToeBoard();

        public GameRoom(TCPGameServer pOwner) : base(pOwner)
        {
        }

        public void StartGame(TcpMessageChannel pPlayer1, TcpMessageChannel pPlayer2, TcpMessageChannel laptop)
        {
            if (IsGameInPlay) throw new Exception("Programmer error duuuude.");

            IsGameInPlay = true;
            addMember(pPlayer1);
            addMember(pPlayer2);
            addMember(laptop);
            if (_server.GetPlayerInfo(pPlayer1).characterID == 1)
            {
                _player1 = pPlayer1;
            }
            else
            {
                _player2 = pPlayer1;
            }

            if (_server.GetPlayerInfo(pPlayer2).characterID == 1)
            {
                _player1 = pPlayer2;
            }
            else
            {
                _player2 = pPlayer2;
            }

            _laptop = laptop;
            RoomEntered roomEntered = new RoomEntered();
            roomEntered.player1 = _server.GetPlayerInfo(pPlayer1);
            roomEntered.player2 = _server.GetPlayerInfo(pPlayer2);
            roomEntered.laptop = _server.GetPlayerInfo(laptop);
            sendToAll(roomEntered);
        }

        protected override void addMember(TcpMessageChannel pMember)
        {
            base.addMember(pMember);

            //notify client he has joined a game room 
            RoomJoinedEvent roomJoinedEvent = new RoomJoinedEvent();
            roomJoinedEvent.room = RoomJoinedEvent.Room.GAME_ROOM;
            pMember.SendMessage(roomJoinedEvent);
        }

        public override void Update()
        {
            //demo of how we can tell people have left the game...
            int oldMemberCount = memberCount;
            base.Update();
            int newMemberCount = memberCount;
            if (oldMemberCount != newMemberCount)
            {
                Log.LogInfo("People left the game...", this);
            }
        }

        protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
        {
            if (pMessage is DoorActive)
            {
                SetTheDoorActive(pMessage as DoorActive);
            }

            if (pMessage is ChooseCamera)
            {
                ChooseCorrectCamera(pMessage as ChooseCamera, pSender);
            }

            if (pMessage is ShowNotes)
            {
                ShowNotesForThePlayer(pMessage as ShowNotes);
            }

            if (pMessage is LockPickedStatus)
            {
                UpdateLockPickStatus(pMessage as LockPickedStatus);
            }
        }

        private void UpdateLockPickStatus(LockPickedStatus pMessage)
        {
            LockPickedStatus lockPick = new LockPickedStatus();
            lockPick.IsLockPicked = true;
            sendToAll(lockPick);
        }

        private void ShowNotesForThePlayer(ShowNotes pMessage)
        {
            if (pMessage.Player == 1)
            {
                ShowNotes notes = new ShowNotes();
                notes.PlayerRoom = pMessage.PlayerRoom;
                notes.Player = 1;
                _player1.SendMessage(notes);
            }

            if (pMessage.Player == 2)
            {
                ShowNotes notes = new ShowNotes();
                notes.PlayerRoom = pMessage.PlayerRoom;
                notes.Player = 2;
                _player2.SendMessage(notes);
            }
        }

        private void ChooseCorrectCamera(ChooseCamera pMessage, TcpMessageChannel pSender)
        {
            int character = _server.GetPlayerInfo(pSender).characterID;

            if (character == 1)
            {
                ChooseCamera camera = new ChooseCamera();
                camera.Camera = pMessage.Camera;
                camera.Player = 1;
                _laptop.SendMessage(camera);
                ChooseCamera cameraPhone = new ChooseCamera();
                _startCameraP1 = NewActiveCamera(_startCameraP1, pMessage.Camera);
                Console.WriteLine(_startCameraP1);
                cameraPhone.Camera = _startCameraP1;
                cameraPhone.Player = 3;
                pSender.SendMessage(cameraPhone);
            }

            if (character == 2)
            {
                ChooseCamera camera = new ChooseCamera();
                camera.Camera = pMessage.Camera;
                camera.Player = 2;
                _laptop.SendMessage(camera);
                ChooseCamera cameraPhone = new ChooseCamera();
                cameraPhone.Camera = pMessage.Camera;
                cameraPhone.Player = 3;
                pSender.SendMessage(cameraPhone);
            }
        }

        private int NewActiveCamera(int startCamera, int button)
        {
            Console.WriteLine(button);
            int activeCamera = startCamera;
            if (button == 3)
            {
                activeCamera++;
                if (activeCamera >= 3)
                {
                    activeCamera = 0;
                }
            } else if (button == 1)
            {
                activeCamera--;
                if (activeCamera <= -1)
                {
                    activeCamera = 3;
                }
            }
            else
            {
                activeCamera = 0;
            }
            return activeCamera;
        }

        private void SetTheDoorActive(DoorActive doorActive)
        {
            if (doorActive.Player == 1)
            {
                DoorActive sendDoor = new DoorActive();
                sendDoor.IsActive = doorActive.IsActive;
                _player1.SendMessage(sendDoor);
            }
            else
            {
                DoorActive sendDoor = new DoorActive();
                sendDoor.IsActive = doorActive.IsActive;
                _player2.SendMessage(sendDoor);
            }
        }
    }
}