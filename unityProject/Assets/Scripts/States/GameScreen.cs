using shared;
using UnityEngine;

namespace States
{
    public class GameScreen : ClientState
    {
        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Welcome to the game...");
        }
        
        private void Update()
        {
            receiveAndProcessNetworkMessages();
        }
        
        protected override void handleNetworkMessage(ASerializable pMessage)
        {
            if (pMessage is GameFinished)
            {
                handleGameFinished(pMessage as GameFinished);
            }
        }
        
        private void handleGameFinished(GameFinished pMessage)
        {
            PlayerJoinRequest join = new PlayerJoinRequest();
            Client.Channel.SendMessage(join);
            Client.SetState<LobbyScreen>();
        }
    }
}