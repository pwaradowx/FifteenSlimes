namespace Project.Assets.Puzzle
{
    public readonly struct GridTile
    {
        /// <summary>
        /// Represents the actual position of the tile inside of puzzle space.
        /// Numbers from 1 up 15 tells the position for slimes and 16 means this tile should be empty.
        /// </summary>
        public int MyNumber { get; }

        public GridTile(int number)
        {
            MyNumber = number;
        }
    }
}

