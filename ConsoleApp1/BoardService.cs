using Battleships.Enums;
using System.Text.RegularExpressions;

namespace Battleships
{
    internal class BoardService
    {
        private const string ShootRegex = "^[a-zA-Z]{1}[0-9]{1,2}$";

        private List<Ship> _shipsOnTheBoard;
        private Board _board;

        public BoardService(
            int boardWidth,
            int boardHeight,
            int battleships,
            int destroyers)
        {
            ValidateGameSetUp(battleships, destroyers, boardHeight);

            _board = new Board(boardWidth, boardHeight);

            _shipsOnTheBoard = new List<Ship>();

            PlaceBattleshipsOnTheBoard(battleships);

            PlaceDestroyersOnTheBoard(destroyers);
        }

        public ShotResults Shoot(string shootSquarePosition)
        {
            Match match = Regex.Match(shootSquarePosition, ShootRegex);

            if (!match.Success)
            {
                return ShotResults.IncorrectShotValue;
            }

            string letterStr = shootSquarePosition.Substring(0, 1);

            string numberStr = shootSquarePosition.Substring(1, shootSquarePosition.Length - 1);

            if (!Enum.IsDefined(typeof(Alphabet), letterStr.ToUpper()))
            {
                return ShotResults.IncorrectShotValue;
            }

            Alphabet letter = (Alphabet)Enum.Parse(typeof(Alphabet), letterStr.ToUpper());

            if (int.TryParse(numberStr, out int number))
            {
                bool positionIsValid = _board.IsValidPosition((int)letter, number);

                if (!positionIsValid)
                {
                    return ShotResults.IncorrectShotValue;
                }

                Ship? ship = _board.GetPosition((int)letter, number);

                if (ship is null)
                {
                    return ShotResults.Miss;
                }

                ship.Hit();

                if (ship.IsSunk)
                {
                    if (_shipsOnTheBoard.All(s => s.IsSunk))
                    {
                        return ShotResults.Win;
                    }

                    return ShotResults.Sink;
                }

                return ShotResults.Hit;

            }

            return ShotResults.IncorrectShotValue;
        }

        private void PlaceDestroyersOnTheBoard(int destroyers)
        {
            for (int i = 0; i < destroyers; i++)
            {
                var destroyer = new Destroyer();

                _board.PlaceShipRandomlyOnTheBoard(destroyer);

                _shipsOnTheBoard.Add(destroyer);
            }
        }

        private void PlaceBattleshipsOnTheBoard(int battleships)
        {
            for (int i = 0; i < battleships; i++)
            {
                var battleship = new Battleship();

                _board.PlaceShipRandomlyOnTheBoard(battleship);

                _shipsOnTheBoard.Add(battleship);
            }
        }

        private void ValidateGameSetUp(
            int battleships,
            int destroyers,
            int boardHeight)
        {
            if (battleships < 1 && destroyers < 1)
            {
                throw new Exception("You need to specify at least 1 ship in order to play");
            }

            if (boardHeight > (int)Alphabet.Z)
            {
                throw new Exception($"Board height can't be greater than {(int)Alphabet.Z}.");
            }
        }
    }
}
