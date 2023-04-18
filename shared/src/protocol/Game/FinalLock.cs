namespace shared
{
    public class FinalLock : ASerializable
    {
        public bool Solved;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(Solved);
        }
        public override void Deserialize(Packet pPacket)
        {
            Solved = pPacket.ReadBool();
        }
    }
}