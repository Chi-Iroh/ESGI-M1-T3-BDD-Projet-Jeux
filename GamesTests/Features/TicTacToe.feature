Feature: TicTacToe

TicTacToe game

Scenario: Simple move
    Given an empty game state
    And x is about to play
    When the player plays at the top left
    Then the game state should be
        | | | |
        | x | | |
        | | | |
        | | | |
    And the game isn't finished yet

Scenario: Initial state
    Given the following game state
        | | | |
        | x | x | o |
        | o | o | x |
        | x | o | |
    And x is about to play
    When the player plays at the bottom right
    Then the game state should be
        | | | |
        | x | x | o |
        | o | o | x |
        | x | o | x |
    And it's a tie

Scenario: Winner
    Given the following game state
        | | | |
        | x | o | o |
        | x | o | |
        | x | | |
    Then the winner should be x

Scenario: Winner fails, game not finished !
    Given the following game state
        | | | |
        | x | | |
        | | | |
        | | | |
    Then the winner cannot be determined yet

Scenario: Invalid state
    Given the following game state
        | | | |
        | r | u | |
        | | | |
        | | 145 | |
    Then the state is invalid

Scenario: Invalid player
    Given an empty game state
    And abc is about to play
    Then the player is invalid

Scenario: Invalid move
    Given an empty game state
    And P1 is about to play
    When the player plays outside the board
    Then the move is invalid
