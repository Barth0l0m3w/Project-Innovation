namespace shared
{
    public class CameraZoom : ASerializable
    {
        public int Button;
        public int Player;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(Button);
            pPacket.Write(Player);
        }
        public override void Deserialize(Packet pPacket)
        {
            Button = pPacket.ReadInt();
            Player = pPacket.ReadInt();
        }
    }
}