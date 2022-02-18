using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Project.Assets.Managers;
using TMPro;

namespace Project.Assets.Puzzle
{
    public class VictoryMenu : MonoBehaviour
    {
        [SerializeField] private Stopwatch stopwatch;
        
        [SerializeField] private TextMeshProUGUI timeHolder;
        [SerializeField] private RawImage backButton;
        [SerializeField] private RawImage slimeCover;

        [SerializeField] private GameObject victoryText;
        [SerializeField] private GameObject currentTimeLabel;
        [SerializeField] private TextMeshProUGUI currentTimeHolder;
        [SerializeField] private GameObject bestTimeLabel;
        [SerializeField] private TextMeshProUGUI bestTimeHolder;
        [SerializeField] private GameObject replayButton;
        [SerializeField] private GameObject menuButton;

        private const float DissolveSpeed = 5f;
        private const float CoverSpeed = 5f;
        private static readonly int CoverRadius = Shader.PropertyToID("_Radius");

        private void Start()
        {
            DisableMenu();
            
            EventManager.Instance.PlayerSolvedPuzzleEvent += Process;
        }

        private void DisableMenu()
        {
            victoryText.SetActive(false);

            currentTimeLabel.SetActive(false);

            currentTimeHolder.gameObject.SetActive(false);

            bestTimeLabel.SetActive(false);

            bestTimeHolder.gameObject.SetActive(false);

            replayButton.SetActive(false);

            menuButton.SetActive(false);
        }

        private async void Process()
        {
            await DissolveExcessStuff();
            await CoverSlimes();
            await ShowMenuElements();
        }

        private async Task DissolveExcessStuff()
        {
            float currentAlpha = 1f;
            float targetAlpha = 0f;

            while (currentAlpha != targetAlpha)
            {
                currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, DissolveSpeed * Time.deltaTime);

                if (Mathf.Abs(currentAlpha - targetAlpha) <= 0.05f)
                {
                    currentAlpha = targetAlpha;

                    if (timeHolder != null)
                    {
                        timeHolder.gameObject.SetActive(false);
                    }
                    backButton.gameObject.SetActive(false);
                }

                timeHolder.alpha = currentAlpha;
                if (backButton != null)
                {
                    backButton.color = new Color(255f, 255f, 255f, currentAlpha);
                }

                await Task.Yield();
            }
        }

        private async Task CoverSlimes()
        {
            slimeCover.color = BackgroundColor.Instance.BackColor;
            
            float currentRad = slimeCover.material.GetFloat(CoverRadius);
            float targetRadius = 0;

            while (currentRad != targetRadius)
            {
                currentRad = Mathf.Lerp(currentRad, targetRadius, Time.deltaTime * CoverSpeed);

                if (Mathf.Abs(currentRad - targetRadius) <= 0.01f)
                {
                    currentRad = targetRadius;
                }

                slimeCover.material.SetFloat(CoverRadius, currentRad);
                
                await Task.Yield();
            }
        }

        private async Task ShowMenuElements()
        {
            victoryText.SetActive(true);
            await Task.Delay(1000);
            
            currentTimeLabel.SetActive(true);
            await Task.Delay(1000);
            
            var currentTime = stopwatch.GetCurrentTime();
            currentTimeHolder.gameObject.SetActive(true);
            currentTimeHolder.text = $"{currentTime.Item1}h:{currentTime.Item2}m:{currentTime.Item3}s";
            await Task.Delay(1000);
            
            bestTimeLabel.SetActive(true);
            await Task.Delay(1000);

            var bestTime = stopwatch.GetBestTime();
            bestTimeHolder.gameObject.SetActive(true);
            bestTimeHolder.text = $"{bestTime.Item1}h:{bestTime.Item2}m:{bestTime.Item3}s";
            await Task.Delay(1000);

            replayButton.SetActive(true);
            await Task.Delay(1000);

            menuButton.SetActive(true);
        }

        private void OnDestroy()
        {
            slimeCover.material.SetFloat(CoverRadius, 1);
        }
    }
}
