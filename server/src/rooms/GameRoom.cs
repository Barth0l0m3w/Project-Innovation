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

		//wraps the board to play on...
		private TicTacToeBoard _board = new TicTacToeBoard();

		public GameRoom(TCPGameServer pOwner) : base(pOwner)
		{
		}

		public void StartGame (TcpMessageChannel pPlayer1, TcpMessageChannel pPlayer2, TcpMessageChannel laptop)
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
			
			// if (pMessage is MakeMoveRequest)
			// {
			// 	HandleMakeMoveRequest(pMessage as MakeMoveRequest, pSender);
			// }
			//
			// if (pMessage is PlayerJoinRequest)
			// {
			// 	HandlePlayerJoin(pSender);
			// }

			if (pMessage is DoorActive)
			{
				SetTheDoorActive(pMessage as DoorActive);
			}

			if (pMessage is ChooseCamera)
			{
				ChooseCorrectCamera(pMessage as ChooseCamera, pSender);
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
			}
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

		private void HandleMakeMoveRequest(MakeMoveRequest pMessage, TcpMessageChannel pSender)
		{
			//we have two players, so index of sender is 0 or 1, which means playerID becomes 1 or 2
			int playerID = indexOfMember(pSender) + 1;
			//make the requested move (0-8) on the board for the player
			_board.MakeMove(pMessage.move, playerID);

			//and send the result of the boardstate back to all clients
			MakeMoveResult makeMoveResult = new MakeMoveResult();
			makeMoveResult.whoMadeTheMove = playerID;
			makeMoveResult.boardData = _board.GetBoardData();
			sendToAll(makeMoveResult);

			if (makeMoveResult.boardData.WhoHasWon() > 0)
			{
				GameFinished finished = new GameFinished();
				finished.player = _server.GetPlayerInfo(pSender);
				sendToAll(finished);

				ChatMessage winnerMessage = new ChatMessage();
				winnerMessage.message = $"{finished.player.id} has won!";
				sendToAll(winnerMessage);

				IsGameInPlay = false;
			}
		}
		
		// private void HandlePlayerJoin(TcpMessageChannel pSender)
		// {
		// 	PlayerJoinResponse response = new PlayerJoinResponse();
		// 	response.result = PlayerJoinResponse.RequestResult.ACCEPTED;
		// 	PlayerInfo newPlayerInfo = _server.GetPlayerInfo(pSender);
		// 	if (newPlayerInfo != null)
		// 	{
		// 		removeMember(pSender);
		// 		pSender.SendMessage(response);
		// 		_infos.Remove(pSender);
		// 		_server.GetLobbyRoom().AddMember(pSender);
		// 	}
		// }
		
		public void AddRoom(GameRoom room)
		{
			_server._rooms.Add(room);
		}

	}
}
