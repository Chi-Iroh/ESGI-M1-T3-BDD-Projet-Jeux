Feature: Darts

Darts game

Scenario: Simple move
    Given an empty game state
    And P1 is about to play
    When the player plays 20
    Then the game state should be
        | P1 | P2 |
        | 20 | - |
    And the game isn't finished yet

Scenario: Initial state
    Given the following game state
        | P1 | P2 |
        | 0 | 0 |
        | 50 | 17 |
        | 60 | 57 |
        | 4 | 8 |
        | 0 | - |
    And P2 is about to play
    When the player plays 40
    Then the game state should be
        | P1 | P2 |
        | 0 | 0 |
        | 50 | 17 |
        | 60 | 57 |
        | 4 | 8 |
        | 0 | 40 |
    And the winner should be P2

Scenario: Winner
    Given the following game state
        | Alice | Bob |
        | 10 | 15 |
        | 50 | 4 |
        | 9 | 19 |
        | 1 | 30 |
        | 40 | 1 |
    Then the winner should be Alice

Scenario: Winner fails, game not finished !
    Given the following game state
        | Alice | Bob |
        | 40 | 1 |
    Then the winner cannot be determined yet
