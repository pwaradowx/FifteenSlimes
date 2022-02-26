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
        [SerializeField] private GameObject retryButton;
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

            retryButton.SetActive(false);

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

                    if (timeHolder != null) timeHolder.gameObject.SetActive(false);
                    if (backButton != null) backButton.gameObject.SetActive(false);
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
            if (slimeCover != null) slimeCover.color = BackgroundColor.Instance.BackColor;
            
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
            AudioManager.Instance.PlayMenuAppearSound();
            if (victoryText != null) victoryText.SetActive(true);
            await Task.Delay(1000);
            
            AudioManager.Instance.PlayMenuAppearSound();
            if (currentTimeLabel != null) currentTimeLabel.SetActive(true);
            await Task.Delay(1000);
            
            var currentTime = stopwatch.GetCurrentTime();
            AudioManager.Instance.PlayMenuAppearSound();
            if (currentTimeHolder != null) currentTimeHolder.gameObject.SetActive(true);
            currentTimeHolder.text = $"{currentTime.Item1}h:{currentTime.Item2}m:{currentTime.Item3}s";
            await Task.Delay(1000);
            
            AudioManager.Instance.PlayMenuAppearSound();
            if (bestTimeLabel != null) bestTimeLabel.SetActive(true);
            await Task.Delay(1000);

            var bestTime = stopwatch.GetBestTime();
            AudioManager.Instance.PlayMenuAppearSound();
            if (bestTimeHolder != null) bestTimeHolder.gameObject.SetActive(true);
            bestTimeHolder.text = $"{bestTime.Item1}h:{bestTime.Item2}m:{bestTime.Item3}s";
            await Task.Delay(1000);

            AudioManager.Instance.PlayMenuAppearSound();
            if (retryButton != null) retryButton.SetActive(true);
            if (menuButton != null) menuButton.SetActive(true);
        }

        private void OnDestroy()
        {
            slimeCover.material.SetFloat(CoverRadius, 1);
        }
    }
}
