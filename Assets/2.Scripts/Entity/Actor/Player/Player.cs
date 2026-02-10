public class Player : ActorEntity
{
    public PlayerSo PlayerData { get; private set; }
    public string PlayerName { get; private set; }

    public void SetUp(PlayerData playerData)
    {
        PlayerName = playerData.playerName;
    }
}