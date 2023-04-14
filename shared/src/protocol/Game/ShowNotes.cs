namespace shared
{
    public class ShowNotes : ASerializable
    {
        public int Player;
        public int PlayerRoom;
        //public int PlayerCamera;
        
        public override void Serialize(Packet pPacket)
        {
 
           pPacket.Write(Player);
            pPacket.Write(PlayerRoom);
            //pPacket.Write(PlayerCamera);
        }
        public override void Deserialize(Packet pPacket)
        {

            Player = pPacket.ReadInt();
            PlayerRoom = pPacket.ReadInt();
            //PlayerCamera = pPacket.ReadInt();
        }
    }
}