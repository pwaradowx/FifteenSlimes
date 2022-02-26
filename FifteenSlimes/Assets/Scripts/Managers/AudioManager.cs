using UnityEngine;

namespace Project.Assets.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip slimeSound;
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip menuAppearSound;

        [SerializeField] private AudioSource audioSource;
        
        public static AudioManager Instance;

        public bool IsAudioOn;
        
        private const string AudioStateKey = "AudioState";
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
            
            Setup();
        }

        private void Setup()
        {
            if (audioSource == null)
            {
                Debug.LogError("One of the audio sources is missing!");
            }
            
            if (PlayerPrefs.GetInt(AudioStateKey) == 0)
            {
                IsAudioOn = true;
            }
            else
            {
                IsAudioOn = PlayerPrefs.GetInt(AudioStateKey) == 1;
            }
        }

        public void PlaySlimeSound()
        {  
            if (IsAudioOn) audioSource.PlayOneShot(slimeSound);
        }

        public void PlayClickSound()
        {
            if (IsAudioOn) audioSource.PlayOneShot(clickSound);
        }

        public void PlayMenuAppearSound()
        {
            if (IsAudioOn) audioSource.PlayOneShot(menuAppearSound);
        }
    }
}
