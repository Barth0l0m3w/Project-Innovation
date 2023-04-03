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
	class LobbyRoom : SimpleRoom
	{
		//this list keeps tracks of which players are ready to play a game, this is a subset of the people in this room
		private Dictionary<TcpMessageChannel, int> _readyMembers = new Dictionary<TcpMessageChannel, int>();
		private bool _playerOneTaken;
		private bool _playerTwoTaken;

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
			ChatMessage simpleMessage = new ChatMessage();
			simpleMessage.message = "AAAAA";
			sendToAll(simpleMessage);
			Console.WriteLine($"Handling player {_server.GetPlayerInfo(pSender).id}");
			//check if noone else took this character
			if (!_playerOneTaken && pMessage.characterID == 1)
			{
				PlayerInfo playerInfo = new PlayerInfo();
				playerInfo.characterID = pMessage.characterID;
				pSender.SendMessage(playerInfo);
				LobbyInfoUpdate infoUpdate = new LobbyInfoUpdate();
				infoUpdate.playerID = pMessage.characterID;
				sendToAll(pMessage);
				_playerOneTaken = true;
			}
			else if (!_playerTwoTaken && pMessage.characterID == 2)
			{
				
			}
			else
			{
				ChatMessage message = new ChatMessage();
				message.message = "This character is already taken!";
				pSender.SendMessage(message);
			}
			
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
				//if (_server.GetPlayerInfo((p) => p.id == _server.GetPlayerInfo(pSender).id).Count == 1)
				if(_readyMembers.ContainsValue(pReadyNotification.characterID))
				{
					Console.WriteLine("This character already exists!");
				}
				else
				{
					_readyMembers.Add(pSender, pReadyNotification.characterID);
					Console.WriteLine("PLayer ready");
					LobbyInfoUpdate update = new LobbyInfoUpdate();
					update.ready = true;
					pSender.SendMessage(update);
				}
				
			}
			else //if the client is no longer ready, unmark it as ready
			{
				_readyMembers.Remove(pSender);
			}

			//do we have enough people for a game and is there no game running yet?
			if (_readyMembers.Count == 2)
			{
				
				Console.WriteLine("We're going to play the game now");
				TcpMessageChannel player1 = _readyMembers.ElementAt(0).Key;
				TcpMessageChannel player2 = _readyMembers.ElementAt(1).Key;
				removeMember(player1);
				removeMember(player2);
				Console.WriteLine(player1 + " And " + player2);
				LobbyInfoUpdate infoUpdate = new LobbyInfoUpdate();
				infoUpdate.sceneNumber = _server.GetPlayerInfo(pSender).sceneNumber;
				pSender.SendMessage(infoUpdate);
				// GameRoom room = new GameRoom(_server);
				// _server.GetRooms().Add(room);
				// room.StartGame(player1,player2);
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
