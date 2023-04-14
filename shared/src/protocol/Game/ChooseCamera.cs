namespace shared
{
    public class ChooseCamera : ASerializable
    {
        
        public int Camera;
        public int Player;
        public int PlayerRoom;
        public int PlayerCamera;
        
        public override void Serialize(Packet pPacket)
        {
 
            pPacket.Write(Camera);
            pPacket.Write(Player);
            pPacket.Write(PlayerRoom);
            pPacket.Write(PlayerCamera);
        }
        public override void Deserialize(Packet pPacket)
        {
   
            Camera = pPacket.ReadInt();
            Player = pPacket.ReadInt();
            PlayerRoom = pPacket.ReadInt();
            PlayerCamera = pPacket.ReadInt();
        }
    }
}