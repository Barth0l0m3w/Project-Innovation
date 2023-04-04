namespace shared
{
    /**
	 * BIDIRECTIONAL Chat message for the lobby
	 */
    public class CharacterNotAccepted : ASerializable
    {
        public bool NotAccepted;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(NotAccepted);
        }

        public override void Deserialize(Packet pPacket)
        {
            NotAccepted = pPacket.ReadBool();
        }
    }
}