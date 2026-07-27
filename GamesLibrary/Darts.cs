using GamesBase;

namespace GamesLibrary
{
    public class Darts : IGame
    {
        private Dictionary<string, List<int?>> scores = new()
        {
            { "P1", [] },
            { "P2", []}
        };
        private string player = "P1";

        private const int TURNS = 5;

        public IGame EmptyInit()
        {
            return new Darts();
        }

        public IGame InitFromState(GameState state)
        {
            string player = state.headers[0];

            if (state.content.Count > TURNS)
            {
                throw new Exception($"Only {TURNS} allowed !");
            }

            var _scores = new Dictionary<string, List<int?>>
            {
                { state.headers[0], [] },
                { state.headers[1], [] }
            };

            for (int i = 0; i < state.content.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    string move = state.content[i][j];
                    if (move == "-")
                    {
                        _scores[state.headers[j]].Add(null);
                        player = state.headers[j];
                    } else
                    {
                        _scores[state.headers[j]].Add(int.Parse(move));
                    }
                }
            }

            var darts = new Darts();
            darts.scores = _scores;
            darts.player = player;
            return darts;
        }

        public void SetPlayer(string player)
        {
            if (scores.ContainsKey(player))
            {
                this.player = player;
            } else
            {
                throw new Exception($"Invalid player {player} !");
            }
        }

        private string OtherPlayer(string player)
        {
            var players = this.scores.Keys.ToArray();
            return (player == players[0]) ? players[1] : players[0];
        }

        public void Play(string move)
        {
            if (this.Finished())
            {
                throw new Exception("Game already finished !");
            }

            int m = int.Parse(move);
            var moves = this.scores[this.player];
            if (moves.Count != 0 && moves[moves.Count - 1] is null)
            {
                this.scores[this.player][moves.Count - 1] = m;
            } else
            {
                this.scores[this.player].Add(m);
            }
            this.player = OtherPlayer(this.player);
        }

        public bool CompareStateWith(GameState state)
        {
            for (int i = 0; i < state.content.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    string player = state.headers[j];
                    if (!this.scores.ContainsKey(player))
                    {
                        return false;
                    }

                    string score = state.content[i][j];
                    if (score == "-")
                    {
                        if (this.scores[player].Count > i && this.scores[player][i] is not null)
                        {
                            return false;
                        }
                    } else
                    {
                        if (this.scores[player][i].ToString() != score)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool Finished()
        {
            return this.scores.All(scores => scores.Value.Count == TURNS && scores.Value[TURNS - 1] is not null);
        }

        private Dictionary<string, int> GetScore()
        {
            Dictionary<string, int> score = new();

            foreach (var pair in this.scores)
            {
                int _score = 0;
                foreach (int? i in pair.Value)
                {
                    if (i is null)
                    {
                        throw new Exception("Game isn't finished !");
                    }
                    _score += (int)i;
                }
                score.Add(pair.Key, _score);
            }
            return score;
        }

        private string? _Winner()
        {
            if (!Finished())
            {
                throw new GameNotFinishedException();
            }

            var score = GetScore();
            var max = score.MaxBy(k => k.Value);
            var min = score.MinBy(k => k.Value);

            if (max.Value == min.Value)
            {
                return null;
            }
            return max.Key;
        }

        public bool IsTie()
        {
            return _Winner() is null;
        }

        public string Winner()
        {
            return _Winner()!;
        }
    }
}
