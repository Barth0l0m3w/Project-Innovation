namespace shared
{
    public class GoToNextScene : ASerializable
    {
        public bool goToScene = false;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(goToScene);
        }

        public override void Deserialize(Packet pPacket)
        {
            goToScene = pPacket.ReadBool();
        }
    }
}