using GamesBase;

namespace GamesLibrary
{
    public class GameFactory
    {
        public static IGame Create(string type)
        {
            switch (type)
            {
                case "TicTacToe":
                    return new TicTacToe(null);

                case "Darts":
                    return new Darts();

                default:
                    throw new Exception($"{type} game not implemented !");
            }
        }
    }
}
