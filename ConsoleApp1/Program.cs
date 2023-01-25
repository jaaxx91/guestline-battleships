// See https://aka.ms/new-console-template for more information

using Battleships;
using Battleships.Enums;

Console.WriteLine("Welcome to a board game 10x10 with 1 battleship and 2 destroyers!");

var board = new BoardService(
    GameConfig.BoardWidth,
    GameConfig.BoardHeight,
    GameConfig.BattleShips,
    GameConfig.Destroyers);

var shotResults = ShotResults.IncorrectShotValue;

do
{
    Console.WriteLine("Please enter cooridnates of the form 'A5', where A is the column and 5 is the row.");

    string coordinates = Console.ReadLine();

    shotResults = board.Shoot(coordinates);

    switch (shotResults)
    {
        case ShotResults.IncorrectShotValue:
            Console.WriteLine("These are incorrect coordinates. Please try again.");
            break;
        case ShotResults.Win:
            Console.WriteLine("Congratulations! You have managed to sink all the ships.");
            break;
        case ShotResults.Sink:
            Console.WriteLine("Congratulations! You have managed to sink one of the ships.");
            break;
        case ShotResults.Miss:
            Console.WriteLine("Sorry, that was a missed shot.");
            break;
        case ShotResults.Hit:
            Console.WriteLine("Great! You have managed to hit one of the ships.");
            break;
    }

    Console.WriteLine("Press any key to continue.");
    Console.ReadKey();
    Console.Clear();

} while (shotResults != ShotResults.Win);

Environment.Exit(0);