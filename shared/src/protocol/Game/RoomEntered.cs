namespace shared
{
    public class RoomEntered : ASerializable
    {
        public PlayerInfo player1;
        public PlayerInfo player2;
        public PlayerInfo laptop;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(player1);
            pPacket.Write(player2);
            pPacket.Write(laptop);
        }
        public override void Deserialize(Packet pPacket)
        {
            player1 = pPacket.Read<PlayerInfo>();
            player2 = pPacket.Read<PlayerInfo>();
            laptop = pPacket.Read<PlayerInfo>();
        }
    }
}