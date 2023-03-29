namespace shared
{
    public class GameFinished : ASerializable
    {
        public PlayerInfo player;
        
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(player);
        }
        public override void Deserialize(Packet pPacket)
        {
            player = pPacket.Read<PlayerInfo>();
        }
    }
}