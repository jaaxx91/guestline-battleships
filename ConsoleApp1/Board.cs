namespace Battleships
{
    internal class Board
    {
        public const int MinimumBoardHeight = 1;
        public const int MinimumBoardWidth = 1;

        private int _boardWidth;
        private int _boardHeight;

        private Ship?[,] _board;

        public Board(int boardWidth, int boardHeight)
        {
            ValidateBoardSetUp(boardWidth, boardHeight);

            _boardWidth = boardWidth;
            _boardHeight = boardHeight;

            _board = new Ship[boardWidth, boardHeight];
        }

        public bool IsValidPosition(int width, int height)
            =>
            width <= _boardWidth &&
            width >= MinimumBoardWidth &&
            height <= _boardHeight &&
            height >= MinimumBoardHeight;

        public Ship? GetPosition(int width, int height)
            => _board[width - 1, height - 1];

        public void PlaceShipRandomlyOnTheBoard(Ship ship)
        {
            IEnumerable<(int positionWidth, int positionHeight)> shipPosition = GetNextRandomPositionForShip(ship.Size);

            foreach (var positionSquare in shipPosition)
            {
                _board[positionSquare.positionWidth, positionSquare.positionHeight] = ship;
            }
        }

        private IEnumerable<(int positionWidth, int positionHeight)> GetNextRandomPositionForShip(int shipSize)
        {
            var possiblePositions = new List<List<(int positionWidth, int positionHeight)>>();

            do
            {
                (int positionWidth, int positionHeight) = GetNextRandomPosition();

                possiblePositions = GetPossiblePositions(
                    shipSize,
                    positionWidth,
                    positionHeight);

            } while (!possiblePositions.Any());

            Random rnd = new Random();
            int selectedPosition = rnd.Next(0, possiblePositions.Count() - 1);

            return possiblePositions[selectedPosition];
        }

        private List<List<(int positionWidth, int positionHeight)>> GetPossiblePositions(
            int shipSize,
            int positionWidth,
            int positionHeight)
        {
            var possiblePositions = new List<List<(int positionWidth, int positionHeight)>>();

            int numberOfSquaresNeeded = shipSize - 1;

            List<(int, int)>? positionsAbove = GetPositionsAbove(
                positionWidth,
                positionHeight,
                numberOfSquaresNeeded);

            if (positionsAbove is not null)
            {
                positionsAbove.Add((positionWidth, positionHeight));
                possiblePositions.Add(positionsAbove);
            }

            List<(int, int)>? positionsBelow = GetPositionsBelow(
                positionWidth,
                positionHeight,
                numberOfSquaresNeeded);

            if (positionsBelow is not null)
            {
                positionsBelow.Add((positionWidth, positionHeight));
                possiblePositions.Add(positionsBelow);
            }

            List<(int, int)>? positionsOnTheRightSide = GetPositionsOnTheRightSide(
                positionWidth,
                positionHeight,
                numberOfSquaresNeeded);

            if (positionsOnTheRightSide is not null)
            {
                positionsOnTheRightSide.Add((positionWidth, positionHeight));
                possiblePositions.Add(positionsOnTheRightSide);
            }

            List<(int, int)>? positionsOnTheLeftSide = GetPositionsOnTheLeftSide(
                positionWidth,
                positionHeight,
                numberOfSquaresNeeded);

            if (positionsOnTheLeftSide is not null)
            {
                positionsOnTheLeftSide.Add((positionWidth, positionHeight));
                possiblePositions.Add(positionsOnTheLeftSide);
            }

            return possiblePositions;
        }

        private List<(int positionWidth, int positionHeight)>? GetPositionsAbove(
            int positionWidth,
            int positionHeight,
            int numberOfSquaresNeeded)
        {
            if (positionHeight + numberOfSquaresNeeded <= _boardHeight - 1)
            {
                var positions = new List<(int, int)>();

                for (int i = 1; i < numberOfSquaresNeeded + 1; i++)
                {
                    Ship? shipAlreadyInPlace = _board[positionWidth, positionHeight + i];

                    if (shipAlreadyInPlace is not null)
                    {
                        return null;
                    }

                    positions.Add((positionWidth, positionHeight + i));
                }

                return positions;
            }

            return null;
        }

        private List<(int positionWidth, int positionHeight)>? GetPositionsBelow(
            int positionWidth,
            int positionHeight,
            int numberOfSquaresNeeded)
        {
            if (positionHeight - numberOfSquaresNeeded >= MinimumBoardHeight)
            {
                var positions = new List<(int, int)>();

                for (int i = 1; i < numberOfSquaresNeeded + 1; i++)
                {
                    Ship? shipAlreadyInPlace = _board[positionWidth, positionHeight - i];

                    if (shipAlreadyInPlace is not null)
                    {
                        return null;
                    }

                    positions.Add((positionWidth, positionHeight - i));
                }

                return positions;
            }

            return null;
        }

        private List<(int positionWidth, int positionHeight)>? GetPositionsOnTheRightSide(
            int positionWidth,
            int positionHeight,
            int numberOfSquaresNeeded)
        {
            if (positionWidth + numberOfSquaresNeeded <= _boardWidth - 1)
            {
                var positions = new List<(int, int)>();

                for (int i = 1; i < numberOfSquaresNeeded + 1; i++)
                {
                    Ship? shipAlreadyInPlace = _board[positionWidth + i, positionHeight];

                    if (shipAlreadyInPlace is not null)
                    {
                        return null;
                    }

                    positions.Add((positionWidth + i, positionHeight));
                }

                return positions;
            }

            return null;
        }

        private List<(int positionWidth, int positionHeight)>? GetPositionsOnTheLeftSide(
            int positionWidth,
            int positionHeight,
            int numberOfSquaresNeeded)
        {
            if (positionWidth - numberOfSquaresNeeded >= MinimumBoardWidth)
            {
                var positions = new List<(int, int)>();

                for (int i = 1; i < numberOfSquaresNeeded + 1; i++)
                {
                    Ship? shipAlreadyInPlace = _board[positionWidth - i, positionHeight];

                    if (shipAlreadyInPlace is not null)
                    {
                        return null;
                    }

                    positions.Add((positionWidth - i, positionHeight));
                }

                return positions;
            }

            return null;
        }

        private (int positionWidth, int positionHeight) GetNextRandomPosition()
        {
            Random rnd = new Random();
            int width = rnd.Next(0, _boardWidth - 1);
            int height = rnd.Next(0, _boardHeight - 1);

            var ship = _board[width, height];

            if (ship is not null)
            {
                do
                {
                    width = rnd.Next(0, _boardWidth - 1);
                    height = rnd.Next(0, _boardHeight - 1);

                    ship = _board[width, height];

                } while (ship is not null);
            }

            return (width, height);
        }

        private void ValidateBoardSetUp(int boardWidth, int boardHeight)
        {
            if (boardWidth < 2 || boardHeight < 2)
            {
                throw new Exception("The minimum size of the board is 2x2.");
            }
        }
    }
}
