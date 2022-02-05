using Project.Assets.MainMenu;

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

        public StopwatchData(int bestSec, int bestMin, int bestHour, SelectMode.Mode mode)
        {
            if (mode == SelectMode.Mode.EightPuzzle)
            {
                EightPuzzleBestSec = bestSec;
                EightPuzzleBestMin = bestMin;
                EightPuzzleBestHour = bestHour;
            }
            else if (mode == SelectMode.Mode.FifteenPuzzle)
            {
                FifteenPuzzleBestSec = bestSec;
                FifteenPuzzleBestMin = bestMin;
                FifteenPuzzleBestHour = bestHour;
            }
        }
    }
}
