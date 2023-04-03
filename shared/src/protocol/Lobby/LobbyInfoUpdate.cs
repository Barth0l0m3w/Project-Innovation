namespace shared
{
	/**
	 * Send from SERVER to all CLIENTS to provide info on how many people are in the lobby
	 * and how many of them are ready.
	 */
	public class LobbyInfoUpdate : ASerializable
	{
		public int sceneNumber;
		public int playerID;
		public int characterID;
		public bool ready;

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(sceneNumber);
			pPacket.Write(playerID);
			pPacket.Write(characterID);
			pPacket.Write(ready);
		}

		public override void Deserialize(Packet pPacket)
		{
			sceneNumber = pPacket.ReadInt();
			playerID = pPacket.ReadInt();
			characterID = pPacket.ReadInt();
			ready = pPacket.ReadBool();
		}
	}
}
