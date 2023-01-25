namespace Battleships
{
    internal abstract class Ship
    {
        private int _numberOfHits { get; set; }

        public bool IsSunk => _numberOfHits >= Size;

        public abstract int Size { get; }

        public void Hit() => _numberOfHits = _numberOfHits + 1;
    }
}
