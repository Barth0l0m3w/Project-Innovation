using System;
using System.Net.Sockets;
using System.Net;
using shared;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace server
{
    /**
	 * Basic TCPGameServer that runs our game.
	 * 
	 * Server is made up out of different rooms that can hold different members.
	 * Each member is identified by a TcpMessageChannel, which can also be used for communication.
	 * In this setup each client is only member of ONE room, but you could change that of course.
	 * 
	 * Each room is responsible for cleaning up faulty clients (since it might involve gameplay, status changes etc).
	 * 
	 * As you can see this setup is limited/lacking:
	 * - only 1 game can be played at a time
	 */
    class TCPGameServer
    {
        public static void Main(string[] args)
        {
            TCPGameServer tcpGameServer = new TCPGameServer();
            tcpGameServer.run();
        }

        //we have 3 different rooms at the moment (aka simple but limited)

        private LoginRoom _loginRoom; //this is the room every new user joins
        private LobbyRoom _lobbyRoom; //this is the room a user moves to after a successful 'login'
        private GameRoom _gameRoom; //this is the room a user moves to when a game is succesfully started
        public List<GameRoom> _rooms = new List<GameRoom>();
        List<GameRoom> inactiveRooms = new List<GameRoom>();
        private int _id = 0;

        //stores additional info for a player, TODO: here add login name etc
        private Dictionary<TcpMessageChannel, PlayerInfo> _playerInfo = new Dictionary<TcpMessageChannel, PlayerInfo>();

        private TCPGameServer()
        {
            //we have only one instance of each room, this is especially limiting for the game room (since this means you can only have one game at a time).
            _loginRoom = new LoginRoom(this);
            _lobbyRoom = new LobbyRoom(this);
            _gameRoom = new GameRoom(this);
            _playerInfo = new Dictionary<TcpMessageChannel, PlayerInfo>();
        }

        private void run()
        {
            Log.LogInfo("Starting server on port 55558", this, ConsoleColor.Gray);
            //start listening for incoming connections (with max 50 in the queue)
            //we allow for a lot of incoming connections, so we can handle them
            //and tell them whether we will accept them or not instead of bluntly declining them
            TcpListener listener = new TcpListener(IPAddress.Any, 55558);
            listener.Start(50);

            while (true)
            {
                //check for new members	
                if (listener.Pending())
                {
                    //get the waiting client
                    Log.LogInfo("Accepting new client...", this, ConsoleColor.White);
                    TcpClient client = listener.AcceptTcpClient();
                    //and wrap the client in an easier to use communication channel
                    TcpMessageChannel channel = new TcpMessageChannel(client);

                    PlayerInfo info = new PlayerInfo();
                    _playerInfo.Add(channel, info);
                    _id++;
                    Console.WriteLine(_id);
                    _playerInfo[channel].id = _id;
                    Console.WriteLine(_playerInfo[channel].id);

                    //and add it to the login room for further 'processing'
                    _loginRoom.AddMember(channel);
                }

                
                _loginRoom.Update();
                _lobbyRoom.Update();
                foreach (var gameRoom in _rooms)
                {
                    gameRoom.Update();
                }

                RemoveFinishedGameRooms(); //clear the rooms
                Thread.Sleep(100);
            }
        }

        //provide access to the different rooms on the server 
        public LoginRoom GetLoginRoom()
        {
            return _loginRoom;
        }

        public LobbyRoom GetLobbyRoom()
        {
            return _lobbyRoom;
        }

        public List<GameRoom> GetRooms()
        {
            return _rooms; 
            
        }

        /**
		 * Returns a handle to the player info for the given client 
		 * (will create new player info if there was no info for the given client yet)
		 */
        public PlayerInfo GetPlayerInfo(TcpMessageChannel pClient)
        {
            if (!_playerInfo.ContainsKey(pClient))
            {
                Console.WriteLine("this shouldnt call");
                _playerInfo[pClient] = new PlayerInfo();
            }

            return _playerInfo[pClient];
        }

        /**
		 * Returns a list of all players that match the predicate, e.g. to get a list of 
		 * all players named bob, you would do:
		 *	GetPlayerInfo((playerInfo) => playerInfo.name == "bob");
		 */
        public List<PlayerInfo> GetPlayerInfo(Predicate<PlayerInfo> pPredicate)
        {
            return _playerInfo.Values.ToList<PlayerInfo>().FindAll(pPredicate);
        }

        /**
		 * Should be called by a room when a member is closed and removed.
		 */
        public void RemovePlayerInfo(TcpMessageChannel pClient)
        {
            _playerInfo.Remove(pClient);
        }

        public void AddPlayerInfo(TcpMessageChannel pClient, PlayerInfo playerInfo)
        {
            try
            {
                Console.WriteLine("trying to add client :" + playerInfo.id);
                _playerInfo[pClient] = playerInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void RemoveFinishedGameRooms()
        {
            foreach (var room in _rooms)
            {
                if (!room.IsGameInPlay && room._members.Count == 0)
                {
                    Console.WriteLine("Room added as inactive");
                    inactiveRooms.Add(room);
                }
            }

            foreach (var inactiveRom in inactiveRooms)
            {
                Console.WriteLine("inactive room removed");
                _rooms.Remove(inactiveRom);
            }
        }
    }
}