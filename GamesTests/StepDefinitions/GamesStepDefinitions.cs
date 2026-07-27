using System.Collections.Immutable;
using GamesLibrary;

namespace GamesBase.StepDefinitions
{
    [Binding]
    public sealed class GamesStepDefinitions
    {
        // For additional details on Reqnroll step definitions see https://go.reqnroll.net/doc-stepdef

        private IGame _target;
        private bool _invalidState = false;
        private bool _invalidPlayer = false;
        private bool _invalidMove = false;

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
            _invalidState = false;
            try {
                _target = _target.InitFromState(tableToGameState(table));
            } catch (InvalidStateException)
            {
                _invalidState = true;
            }
        }

        [Given("(.*) is about to play")]
        void GivenPlayer(string player)
        {
            _invalidPlayer = false;
            try {
                _target.SetPlayer(player);
            } catch (InvalidPlayerException)
            {
                _invalidPlayer = true;
            }
        }

        #endregion

        #region when

        [When("the player plays (.*)")]
        void WhenPlays(string move)
        {
            _invalidMove = false;
            try {
                _target.Play(move);
            } catch (InvalidMoveException)
            {
                _invalidMove = true;
            }
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

        [Then("the state is invalid")]
        void ThenTheStateIsInvalid()
        {
            Assert.IsTrue(_invalidState);
        }

        [Then("the player is invalid")]
        void ThenThePlayerIsInvalid()
        {
            Assert.IsTrue(_invalidPlayer);
        }

        [Then("the move is invalid")]
        void ThenTheMoveIsInvalid()
        {
            Assert.IsTrue(_invalidMove);
        }

        #endregion
    }
}
