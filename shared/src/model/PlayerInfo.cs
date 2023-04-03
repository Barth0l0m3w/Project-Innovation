namespace shared
{
    /**
     * Empty placeholder class for the PlayerInfo object which is being tracked for each client by the server.
     * Add any data you want to store for the player here and make it extend ASerializable.
     */
    public class PlayerInfo : ASerializable
    {
        public int id;
        public int sceneNumber;
        public int characterID;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(id);
            pPacket.Write(sceneNumber);
            pPacket.Write(characterID);
        }

        public override void Deserialize(Packet pPacket)
        {
            id = pPacket.ReadInt();
            sceneNumber = pPacket.ReadInt();
            characterID = pPacket.ReadInt();
        }
    }
}
