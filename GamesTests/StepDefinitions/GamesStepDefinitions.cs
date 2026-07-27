using System.Collections.Immutable;
using GamesLibrary;

namespace GamesBase.StepDefinitions
{
    [Binding]
    public sealed class GamesStepDefinitions
    {
        // For additional details on Reqnroll step definitions see https://go.reqnroll.net/doc-stepdef

        private IGame _target;

        public GamesStepDefinitions(FeatureContext featureContext)
        {
            this._target = GameFactory.Create(featureContext.FeatureInfo.Title);
        }

        private static GameState tableToGameState(Table table)
        {
            List<string> headers = table.Header.ToList();
            List<List<string>> content = table.Rows.Select(row => row.Values.ToList()).ToList();
            return new GameState(headers, content);
        }

        #region Given

        [Given("an empty game state")]
        void GivenAnEmptyState()
        {
            _target = _target.EmptyInit();
        }

        [Given("the following game state")]
        void GivenTheFollowingInitialState(Table table)
        {
            _target = _target.InitFromState(tableToGameState(table));
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
            Assert.IsTrue(_target.CompareStateWith(tableToGameState(table)));
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

        [Then("the winner cannot be determined yet")]
        void ThenTheWinnerCannotBeDeterminedYet()
        {
            try
            {
                Assert.IsFalse(_target.Finished());
                _target.Winner();
                Assert.Fail("Winner detection should fail !");
            } catch (GameNotFinishedException) {}
        }

        #endregion
    }
}
