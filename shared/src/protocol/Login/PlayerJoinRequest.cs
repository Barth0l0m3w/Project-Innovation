namespace shared
{
    /**
     * Send from CLIENT to SERVER to request joining the server.
     */
    public class PlayerJoinRequest : ASerializable
    {
        public int DeviceType;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(DeviceType);
        }

        public override void Deserialize(Packet pPacket)
        {
            DeviceType = pPacket.ReadInt();
        }
    }
}
