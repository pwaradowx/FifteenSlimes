using UnityEngine;

namespace Project.Assets.MainMenu
{
    public class SelectMode : MonoBehaviour
    {
        public ModeButton[] ModesButtons;
        
        public enum Mode
        {
            EightPuzzle = 2,
            FifteenPuzzle = 3
        }
        public Mode CurrentMode;
        
        private const string PreviousPuzzleModeKey = "PreviouesPuzzleMode";

        private void Awake()
        {
            // Set EightPuzzle mode by default.
            CurrentMode = Mode.EightPuzzle;

            CurrentMode = (Mode) PlayerPrefs.GetInt(PreviousPuzzleModeKey);
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetInt(PreviousPuzzleModeKey, (int) CurrentMode);
        }
    }
}
