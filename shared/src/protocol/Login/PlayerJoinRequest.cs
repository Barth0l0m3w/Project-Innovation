namespace shared
{
    /**
     * Send from CLIENT to SERVER to request joining the server.
     */
    public class PlayerJoinRequest : ASerializable
    {
        public string name;
        public int room;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(name);
            pPacket.Write(room);
        }

        public override void Deserialize(Packet pPacket)
        {
            name = pPacket.ReadString();
            room = pPacket.ReadInt();
        }
    }
}
