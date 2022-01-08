namespace Project.Assets.FifteenPuzzle
{
    public class GridTile
    {
        public enum TileNumber
        {
            One = 1,
            Two = 2, 
            Three = 3, 
            Four = 4, 
            Five = 5, 
            Six = 6, 
            Seven = 7,
            Eight = 8, 
            Nine = 9, 
            Ten = 10, 
            Eleven = 11, 
            Twelve = 12, 
            Thirteen = 13, 
            Fourteen = 14,
            Fifteen = 15,
            Empty = 16
        }
        
        public TileNumber MyNumber { get; }

        public GridTile(TileNumber number)
        {
            MyNumber = number;
        }
    }
}

