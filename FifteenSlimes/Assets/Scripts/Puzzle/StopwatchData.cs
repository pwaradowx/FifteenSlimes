namespace Project.Assets.Puzzle
{
    public class StopwatchData
    {
        public int EightPuzzleBestSec;
        public int EightPuzzleBestMin;
        public int EightPuzzleBestHour;

        public int FifteenPuzzleBestSec;
        public int FifteenPuzzleBestMin;
        public int FifteenPuzzleBestHour;

        public StopwatchData(int eightPuzzleBestSec, int eightPuzzleBestMin, int eightPuzzleBestHour, 
            int fifteenPuzzleBestSec, int fifteenPuzzleBestMin, int fifteenPuzzleBestHour)
        {
            EightPuzzleBestSec = eightPuzzleBestSec;
            EightPuzzleBestMin = eightPuzzleBestMin;
            EightPuzzleBestHour = eightPuzzleBestHour;
            
            FifteenPuzzleBestSec = fifteenPuzzleBestSec;
            FifteenPuzzleBestMin = fifteenPuzzleBestMin;
            FifteenPuzzleBestHour = fifteenPuzzleBestHour;
        }
    }
}
