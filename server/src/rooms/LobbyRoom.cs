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
		private readonly Dictionary<TcpMessageChannel, int> _readyMembers = new Dictionary<TcpMessageChannel, int>();
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

			if (_server.GetPlayerInfo(pMember).deviceType == 0)
			{
				_readyMembers.Add(pMember,0);
				ChangeReadyStatusRequest changeReadyStatusRequest = new ChangeReadyStatusRequest();
				changeReadyStatusRequest.ready = true;
				pMember.SendMessage(changeReadyStatusRequest);
			}
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
			Console.WriteLine($"Handling player {_server.GetPlayerInfo(pSender).id}");
			//check if noone else took this character
			if (!_playerOneTaken && pMessage.characterID == 1)
			{
				SendPlayerInfo(pMessage, pSender);
				_playerOneTaken = true;
			}
			else if (!_playerTwoTaken && pMessage.characterID == 2)
			{
				SendPlayerInfo(pMessage,pSender);
				_playerTwoTaken = true;
			}
			else
			{
				CharacterNotAccepted notAccepted = new CharacterNotAccepted();
				notAccepted.NotAccepted = false;
				pSender.SendMessage(notAccepted);
			}
			
		}

		private void SendPlayerInfo(ChoosePlayer pMessage, TcpMessageChannel pSender)
		{
			PlayerInfo playerInfo = new PlayerInfo();
			_server.GetPlayerInfo(pSender).characterID = pMessage.characterID;
			pSender.SendMessage(playerInfo);
			ChangeReadyStatusRequest readyStatusRequest = new ChangeReadyStatusRequest();
			readyStatusRequest.characterID = pMessage.characterID;
			readyStatusRequest.ready = true;
			pSender.SendMessage(readyStatusRequest);
			LobbyInfoUpdate infoUpdate = new LobbyInfoUpdate();
			infoUpdate.playerID = _server.GetPlayerInfo(pSender).id;
			infoUpdate.characterID = pMessage.characterID;
			sendToAll(infoUpdate);
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
				else if (_server.GetPlayerInfo(pSender).deviceType == 1)
				{
					_readyMembers.Add(pSender, pReadyNotification.characterID);
					Console.WriteLine("Player ready");
				}
				
			}
			else //if the client is no longer ready, unmark it as ready
			{
				_readyMembers.Remove(pSender);
			}

			//do we have enough people for a game and is there no game running yet?
			if (_readyMembers.Count == 3)
			{
				
				Console.WriteLine("We're going to play the game now");
				GoToNextScene goToNextScene = new GoToNextScene();
				goToNextScene.goToScene = true;
				sendToAll(goToNextScene);
				TcpMessageChannel laptop = _readyMembers.ElementAt(0).Key;
				TcpMessageChannel player1 = _readyMembers.ElementAt(1).Key;
				TcpMessageChannel player2 = _readyMembers.ElementAt(2).Key;
				removeMember(laptop);
				removeMember(player1);
				removeMember(player2);
				Console.WriteLine(_server.GetPlayerInfo(player1).id + " And " + _server.GetPlayerInfo(player2).id);
				
				GameRoom room = new GameRoom(_server);
				room.StartGame(player1,player2, laptop);
				// _server.GetRooms().Add(room);
				// room.StartGame(player1,player2);
			}

			//(un)ready-ing / starting a game changes the lobby/ready count so send out an update
			//to all clients still in the lobby
			//sendLobbyUpdateCount(pSender);
		}

		private void sendLobbyUpdateCount(TcpMessageChannel pSender)
		{
			// RoomJoinedEvent lobbyInfoMessage = new RoomJoinedEvent();
			// lobbyInfoMessage.room = RoomJoinedEvent.Room.GAME_ROOM;
			// sendToAll(lobbyInfoMessage);
		}
		
		

	}
}
