namespace MyWebService
{
    /// <summary>
    /// Simple POCO class containing info about a game.
    /// </summary>
    class GameInfo
    {
        public int PlayerCount { get; private set; }

        public GameInfo(int playerCount)
        {
            PlayerCount = playerCount;
        }
    }
}
