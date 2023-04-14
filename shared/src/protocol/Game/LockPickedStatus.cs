namespace shared
{
    public class LockPickedStatus : ASerializable
    {

        public bool IsLockPicked;
        
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(IsLockPicked);
        }
        public override void Deserialize(Packet pPacket)
        {
            IsLockPicked = pPacket.ReadBool();
        }
    }
}