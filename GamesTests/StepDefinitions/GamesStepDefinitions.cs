using GamesLibrary;

namespace GamesBase.StepDefinitions
{
    [Binding]
    public sealed class GamesStepDefinitions
    {
        // For additional details on Reqnroll step definitions see https://go.reqnroll.net/doc-stepdef

        private IGame _target = new TicTacToe(null);

        #region Given

        [Given("an empty game state")]
        void GivenAnEmptyState()
        {
            _target = _target.EmptyInit();
        }

        [Given("the following initial state")]
        void GivenTheFollowingInitialState(Table table)
        {
            _target = _target.InitFromState(table);
        }

        [Given("(.*) is about to play")]
        void GivenPlayer(string player)
        {
            _target.SetPlayer(player);
        }

        #endregion

        #region when

        [When("the player plays (.*)")]
        void WhenPlays(string move)
        {
            _target.Play(move);
        }

        #endregion

        #region Then

        [Then("the game state should be")]
        void ThenTheGameStateShouldBe(Table table)
        {
            Assert.IsTrue(_target.CompareStateWith(table));
        }

        [Then("the game isn't finished yet")]
        void ThenTheGameIsNotFinishedYet()
        {
            Assert.IsFalse(_target.Finished());
        }

        [Then("it's a tie")]
        void ThenItIsATie()
        {
            Assert.IsTrue(_target.IsTie());
        }

        [Then("the winner should be (.*)")]
        void ThenTheWinnerShouldBe(string winner)
        {
            Assert.IsTrue(_target.Winner() == winner);
        }

        #endregion
    }
}
