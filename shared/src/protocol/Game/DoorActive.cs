namespace shared
{
    public class DoorActive : ASerializable
    {
        public bool IsActive;
        public int Player;
        
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(IsActive);
            pPacket.Write(Player);
        }
        public override void Deserialize(Packet pPacket)
        {
            IsActive = pPacket.ReadBool();
            Player = pPacket.ReadInt();
        }
    }
}