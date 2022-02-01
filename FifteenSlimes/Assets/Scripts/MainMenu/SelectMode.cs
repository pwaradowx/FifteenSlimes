using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Assets.MainMenu
{
    public class SelectMode : MonoBehaviour
    {
        [SerializeField] private RawImage eightPuzzleButton;
        [SerializeField] private RawImage fifteenPuzzleButton;

        private readonly Color ChekedColor = new Color(255f, 255f, 255f, 1f);
        private readonly Color UnchekedColor = new Color(255f, 255f, 255f, 0.3f);

        private const string PreviousPuzzleModeKey = "PreviouesPuzzleMode";

        public enum Mode
        {
            EightPuzzle = 1,
            FifteenPuzzle = 2
        }
        public Mode CurrentMode;

        public void SelectEightPuzzle()
        {
            OnModeChanged(Mode.EightPuzzle);
        }

        public void SelectFifteenPuzzle()
        {
            OnModeChanged(Mode.FifteenPuzzle);
        }

        private void Start()
        {
            // Set EightPuzzle mode by default.
            OnModeChanged(Mode.EightPuzzle);
            
            OnModeChanged((Mode) PlayerPrefs.GetInt(PreviousPuzzleModeKey));
        }

        private void OnModeChanged(Mode newMode)
        {
            if (newMode == Mode.EightPuzzle)
            {
                eightPuzzleButton.color = ChekedColor;
                CurrentMode = Mode.EightPuzzle;

                fifteenPuzzleButton.color = UnchekedColor;
            }
            else if (newMode == Mode.FifteenPuzzle)
            {
                fifteenPuzzleButton.color = ChekedColor;
                CurrentMode = Mode.FifteenPuzzle;

                eightPuzzleButton.color = UnchekedColor;
            }
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetInt(PreviousPuzzleModeKey, (int) CurrentMode);
        }
    }
}
