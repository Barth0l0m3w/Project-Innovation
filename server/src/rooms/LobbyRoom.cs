using System;
using shared;
using System.Collections.Generic;

namespace server
{
	/**
	 * The LobbyRoom is a little bit more extensive than the LoginRoom.
	 * In this room clients change their 'ready status'.
	 * If enough people are ready, they are automatically moved to the GameRoom to play a Game (assuming a game is not already in play).
	 */ 
	class LobbyRoom : SimpleRoom
	{
		//this list keeps tracks of which players are ready to play a game, this is a subset of the people in this room
		private List<TcpMessageChannel> _readyMembers = new List<TcpMessageChannel>();

		public LobbyRoom(TCPGameServer pOwner) : base(pOwner)
		{
		}

		protected override void addMember(TcpMessageChannel pMember)
		{
			base.addMember(pMember);

			//tell the member it has joined the lobby
			RoomJoinedEvent roomJoinedEvent = new RoomJoinedEvent();
			roomJoinedEvent.room = RoomJoinedEvent.Room.LOBBY_ROOM;
			pMember.SendMessage(roomJoinedEvent);

			//print some info in the lobby (can be made more applicable to the current member that joined)
			ChatMessage simpleMessage = new ChatMessage();
			simpleMessage.message = $"Client {_infos[pMember].id} has joined the lobby!";
			//pMember.SendMessage(simpleMessage);
			sendToAll(simpleMessage);

			//send information to all clients that the lobby count has changed
			//sendLobbyUpdateCount(pMember);
		}

		/**
		 * Override removeMember so that our ready count and lobby count is updated (and sent to all clients)
		 * anytime we remove a member.
		 */
		protected override void removeMember(TcpMessageChannel pMember)
		{
			base.removeMember(pMember);
			_readyMembers.Remove(pMember);

			//sendLobbyUpdateCount(pMember);
		}

		protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
		{
			if (pMessage is ChangeReadyStatusRequest) handleReadyNotification(pMessage as ChangeReadyStatusRequest, pSender);
			if (pMessage is ChatMessage) sendMessage(pMessage as ChatMessage, pSender);
			if (pMessage is ChoosePlayer) handlePlayer(pMessage as ChoosePlayer, pSender);
		}

		private void handlePlayer(ChoosePlayer pMessage, TcpMessageChannel pSender)
		{
			LobbyInfoUpdate infoUpdate = new LobbyInfoUpdate();
			infoUpdate.sceneNumber = _server.GetPlayerInfo(pSender).sceneNumber;
			pSender.SendMessage(infoUpdate);
		}

		private void sendMessage(ChatMessage pMessage, TcpMessageChannel pSender)
		{
			pMessage.message = $"{_infos[pSender].id} says: {pMessage.message}";
			sendToAll(pMessage);
		}

		private void handleReadyNotification(ChangeReadyStatusRequest pReadyNotification, TcpMessageChannel pSender)
		{
			//if the given client was not marked as ready yet, mark the client as ready
			if (pReadyNotification.ready)
			{
				if (!_readyMembers.Contains(pSender)) _readyMembers.Add(pSender);
				Console.WriteLine("PLayer ready");
			}
			else //if the client is no longer ready, unmark it as ready
			{
				_readyMembers.Remove(pSender);
			}

			//do we have enough people for a game and is there no game running yet?
			if (_readyMembers.Count >= 2)
			{
				// GameRoom room = new GameRoom(_server);
				// _server.GetRooms().Add(room);
				// TcpMessageChannel player1 = _readyMembers[0];
				// TcpMessageChannel player2 = _readyMembers[1];
				// removeMember(player1);
				// removeMember(player2);
				// room.StartGame(player1,player2);
				Console.WriteLine("We're going to play the game now");
				TcpMessageChannel player1 = _readyMembers[0];
				TcpMessageChannel player2 = _readyMembers[1];
				removeMember(player1);
				removeMember(player2);
				LobbyInfoUpdate infoUpdate = new LobbyInfoUpdate();
				infoUpdate.sceneNumber = _server.GetPlayerInfo(pSender).sceneNumber;
				pSender.SendMessage(infoUpdate);
			}

			//(un)ready-ing / starting a game changes the lobby/ready count so send out an update
			//to all clients still in the lobby
			sendLobbyUpdateCount(pSender);
		}

		private void sendLobbyUpdateCount(TcpMessageChannel pSender)
		{
			// LobbyInfoUpdate lobbyInfoMessage = new LobbyInfoUpdate();
			// lobbyInfoMessage.memberCount = memberCount;
			// lobbyInfoMessage.readyCount = _readyMembers.Count;
			// lobbyInfoMessage.sceneNumber = _server.GetPlayerInfo(pSender).sceneNumber;
			// sendToAll(lobbyInfoMessage);
		}
		
		

	}
}
