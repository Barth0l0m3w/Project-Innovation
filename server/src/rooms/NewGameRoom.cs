using System;
using shared;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace server
{
	/**
	 * The LobbyRoom is a little bit more extensive than the LoginRoom.
	 * In this room clients change their 'ready status'.
	 * If enough people are ready, they are automatically moved to the GameRoom to play a Game (assuming a game is not already in play).
	 */ 
	class NewGameRoom : SimpleRoom
	{
		//this list keeps tracks of which players are ready to play a game, this is a subset of the people in this room
		private readonly Dictionary<TcpMessageChannel, int> _readyMembers = new Dictionary<TcpMessageChannel, int>();
		private bool _playerOneTaken;
		private bool _playerTwoTaken;
		public bool IsGameInPlay { get; private set; }
		private TcpMessageChannel _player1;
		private TcpMessageChannel _player2;
		private TcpMessageChannel _laptop;

		public NewGameRoom(TCPGameServer pOwner) : base(pOwner)
		{
		}


		protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
		{
			if(pMessage is DoorActive) Console.WriteLine("Door active received");
		}
		
		public void StartGame (TcpMessageChannel pPlayer1, TcpMessageChannel pPlayer2, TcpMessageChannel laptop)
		{
			if (IsGameInPlay) throw new Exception("Programmer error duuuude.");

			IsGameInPlay = true;
			addMember(pPlayer1);
			addMember(pPlayer2);
			addMember(laptop);
			_player1 = pPlayer1;
			_player2 = pPlayer2;
			_laptop = laptop;
			RoomEntered roomEntered = new RoomEntered();
			roomEntered.player1 = _server.GetPlayerInfo(pPlayer1);
			roomEntered.player2 = _server.GetPlayerInfo(pPlayer2);
			roomEntered.laptop = _server.GetPlayerInfo(laptop);
			sendToAll(roomEntered);
		}
	}
}
