namespace GamesBase {
    public class Mastermind : IGame
    {
        private readonly List<string> _goal = new();
        private readonly List<Attempt> _attempts = new();

        private string _player = "";
        private bool _finished = false;
        private bool _won = false;

        public int MaxTries { get; set; } = 10;

        private class Attempt
        {
            public string Guess = "";
            public int Good;
            public int Misplaced;
        }

        public void SetGoal(string goal)
        {
            _goal.Clear();
            _goal.AddRange(goal.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        public IGame EmptyInit()
        {
            return new Mastermind();
        }

        public IGame InitFromState(GameState state)
        {
            var game = new Mastermind();

            int i = 0;
            foreach (var row in state.content)
            {
                string rowstr = string.Join(" ", row);
                if (i == 0)
                {
                    game.SetGoal(rowstr);
                } else
                {
                    game.Play(rowstr);
                }
                i++;
            }

            return game;
        }

        public void SetPlayer(string player)
        {
            _player = player;
        }

        private (int, int) Matches(string[] guess)
        {
            int good = 0;

            var remainingGoal = new Dictionary<string, int>();
            var remainingGuess = new Dictionary<string, int>();

            // First pass: exact matches
            for (int i = 0; i < guess.Length; i++)
            {
                if (guess[i] == _goal[i])
                {
                    good++;
                }
                else
                {
                    if (!remainingGoal.ContainsKey(_goal[i]))
                        remainingGoal[_goal[i]] = 0;
                    remainingGoal[_goal[i]]++;

                    if (!remainingGuess.ContainsKey(guess[i]))
                        remainingGuess[guess[i]] = 0;
                    remainingGuess[guess[i]]++;
                }
            }

            // Second pass: misplaced matches
            int misplaced = 0;

            foreach (var kv in remainingGuess)
            {
                if (remainingGoal.TryGetValue(kv.Key, out int count))
                {
                    misplaced += Math.Min(count, kv.Value);
                }
            }

            return (good, misplaced);
        }

        public void Play(string move)
        {
            if (_finished)
                throw new InvalidOperationException("Game already finished.");

            if (_goal.Count == 0)
                throw new InvalidOperationException("Goal has not been set.");

            var guess = move.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (guess.Length != _goal.Count)
                throw new ArgumentException("Invalid guess length.");

            var (good, misplaced) = Matches(guess);
            _attempts.Add(new Attempt
            {
                Guess = move,
                Good = good,
                Misplaced = misplaced
            });

            if (good == _goal.Count)
            {
                _won = true;
                _finished = true;
            }
            else if (_attempts.Count >= MaxTries)
            {
                _finished = true;
            }
        }

        public bool CompareStateWith(GameState state)
        {
            if (state.headers.Count != 3)
                return false;

            if (state.headers[0] != "Tries")
                return false;

            if (state.headers[1] != "Good")
                return false;

            if (state.headers[2] != "Misplaced")
                return false;

            if (_attempts.Count != state.content.Count)
                return false;

            for (int i = 0; i < _attempts.Count; i++)
            {
                var row = state.content[i];
                var attempt = _attempts[i];

                if (row.Count != 3)
                    return false;

                if (row[0] != attempt.Guess)
                    return false;

                if (row[1] != attempt.Good.ToString())
                    return false;

                if (row[2] != attempt.Misplaced.ToString())
                    return false;
            }

            return true;
        }

        public bool Finished()
        {
            return _finished;
        }

        public bool IsTie()
        {
            return _finished && !_won;
        }

        public string Winner()
        {
            if (!_finished)
                throw new InvalidOperationException("Game has not finished.");

            if (_won)
                return _player;

            return "";
        }

        public GameState GetState()
        {
            var rows = _attempts
                .Select(a => new List<string>
                {
                    a.Guess,
                    a.Good.ToString(),
                    a.Misplaced.ToString()
                })
                .ToList();

            return new GameState(
                new List<string>
                {
                    "Tries",
                    "Good",
                    "Misplaced"
                },
                rows);
        }
    }
}
