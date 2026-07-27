using GamesLibrary;
using Reqnroll;

namespace GamesBase
{
    public interface IGame
    {
        abstract IGame EmptyInit();
        abstract IGame InitFromState(Table table);
        abstract void SetPlayer(string player);
        abstract void Play(string move);
        abstract bool CompareStateWith(Table table);
        abstract bool Finished();
        abstract bool IsTie();
        abstract string Winner();
    }
}
