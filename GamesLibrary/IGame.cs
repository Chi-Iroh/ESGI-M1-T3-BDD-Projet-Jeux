using GamesLibrary;

namespace GamesBase
{
    public struct GameState
    {
        public List<string> headers;
        public  List<List<string>> content;

        public GameState(List<string> headers, List<List<string>> content)
        {
            this.headers = headers;
            this.content = content;
        }
    }

    public interface IGame
    {
        abstract IGame EmptyInit();
        abstract IGame InitFromState(GameState table);
        abstract void SetPlayer(string player);
        abstract void Play(string move);
        abstract bool CompareStateWith(GameState table);
        abstract bool Finished();
        abstract bool IsTie();
        abstract string Winner();
    }
}
