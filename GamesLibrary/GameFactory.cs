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

                case "Mastermind":
                    return new Mastermind();

                default:
                    throw new Exception($"{type} game not implemented !");
            }
        }
    }
}
