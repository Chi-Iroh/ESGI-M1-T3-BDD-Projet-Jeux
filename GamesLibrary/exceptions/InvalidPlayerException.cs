namespace GamesLibrary
{
    public class InvalidPlayerException : Exception
    {
        public InvalidPlayerException(string message) : base(message) {}
    }
}