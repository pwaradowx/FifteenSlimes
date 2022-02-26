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
        }

        public void PlaySlimeSound()
        {
            audioSource.PlayOneShot(slimeSound);
        }

        public void PlayClickSound()
        {
            audioSource.PlayOneShot(clickSound);
        }

        public void PlayMenuAppearSound()
        {
            audioSource.PlayOneShot(menuAppearSound);
        }
    }
}
