using UnityEngine;

namespace MergeBounce
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Music")]
        public AudioClip menuMusic;
        public AudioClip gameplayMusic;
        public AudioClip extremeFeverMusic;

        [Header("Sound Effects")]
        public AudioClip pegHitSound;
        public AudioClip mergeSound;
        public AudioClip comboSound;
        public AudioClip powerUpSound;
        public AudioClip buttonClickSound;

        private AudioSource musicSource;
        private AudioSource sfxSource;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            musicSource.loop = true;
            musicSource.volume = 0.5f;
            sfxSource.volume = 0.7f;
        }

        public void PlaySound(AudioClip clip, float pitch = 1f)
        {
            if (clip != null)
            {
                sfxSource.pitch = pitch;
                sfxSource.PlayOneShot(clip);
            }
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip != null && musicSource.clip != clip)
            {
                musicSource.clip = clip;
                musicSource.Play();
            }
        }

        public void PlayExtremeFeverMusic()
        {
            PlayMusic(extremeFeverMusic);
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }
    }
}
