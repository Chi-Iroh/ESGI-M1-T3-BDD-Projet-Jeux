Feature: Mastermind

Mastermind game

Scenario: Simple game
    Given an empty game state
    And the goal is black blue yellow blue
    When the player plays black yellow orange orange
    And the player plays black blue blue yellow
    And the player plays black blue yellow blue
    Then the game state should be
        | Tries | Good | Misplaced |
        | black yellow orange orange | 1 | 1 |
        | black blue blue yellow | 2 | 2 |
        | black blue yellow blue | 4 | 0 |
    And the game is finished
