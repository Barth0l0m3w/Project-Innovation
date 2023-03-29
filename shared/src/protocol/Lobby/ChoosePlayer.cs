namespace shared
{
    /**
	 * Send from SERVER to all CLIENTS to provide info on how many people are in the lobby
	 * and how many of them are ready.
	 */
    public class ChoosePlayer : ASerializable
    {
        public int sceneNumber;
        public int characterID;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(sceneNumber);
            pPacket.Write(characterID);
        }

        public override void Deserialize(Packet pPacket)
        {
            sceneNumber = pPacket.ReadInt();
            characterID = pPacket.ReadInt();
        }
    }
}