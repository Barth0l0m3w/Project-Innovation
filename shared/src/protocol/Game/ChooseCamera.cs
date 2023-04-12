namespace shared
{
    public class ChooseCamera : ASerializable
    {
        
        public int Camera;
        public int Player;
        
        public override void Serialize(Packet pPacket)
        {
 
            pPacket.Write(Camera);
            pPacket.Write(Player);
        }
        public override void Deserialize(Packet pPacket)
        {
   
            Camera = pPacket.ReadInt();
            Player = pPacket.ReadInt();
        }
    }
}