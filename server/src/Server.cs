// using System;
// using System.Collections.Generic;
// using System.Net;
// using System.Net.Sockets;
// using System.Threading;
// using shared;
//
// namespace server
// {
//     public class Server
//     {
//         // public static void Main(string[] args)
//         // {
//         //     Server tcpGameServer = new Server();
//         //     tcpGameServer.run();
//         // }
//         
//         private LoginScreen _loginRoom;
//         private Dictionary<TcpMessageChannel, PlayerInfo> _playerInfo = new Dictionary<TcpMessageChannel, PlayerInfo>();
//         
//         private Server()
//         {
//             //we have only one instance of each room, this is especially limiting for the game room (since this means you can only have one game at a time).
//             //_loginRoom = new LoginScreen(this);
//             _playerInfo = new Dictionary<TcpMessageChannel, PlayerInfo>();
//         }
//         
//         private void run()
//         {
//             Console.WriteLine("Starting server on port 55558");
//             TcpListener listener = new TcpListener(IPAddress.Any, 55558);
//             Console.WriteLine(listener);
//             listener.Start(50);
//
//             while (true)
//             {
//                 //check for new members	
//                 if (listener.Pending())
//                 {
//                     //get the waiting client
//                     Log.LogInfo("Accepting new client...", this, ConsoleColor.White);
//                     TcpClient client = listener.AcceptTcpClient();
//                     //and wrap the client in an easier to use communication channel
//                     TcpMessageChannel channel = new TcpMessageChannel(client);
//
//                     PlayerInfo info = new PlayerInfo();
//                     //_playerInfo.Add(channel, info);
//
//                     //and add it to the login room for further 'processing'
//                     _loginRoom.AddMember(channel);
//                 }
//
//                
//                 _loginRoom.Update();
//                 Thread.Sleep(100);
//             }
//         }
//     }
// }