using System.Collections;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager instance;
    public static BackgroundMusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BackgroundMusicManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("BackgroundMusicManager");
                    instance = go.AddComponent<BackgroundMusicManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource musicSource2; // For crossfading

    [Header("Settings")]
    [SerializeField]
    private float defaultVolume = 0.5f;

    [SerializeField]
    private float fadeDuration = 2f;

    [Header("Music Clips")]
    [SerializeField]
    private AudioClip mainMenuMusic;

    [SerializeField]
    private AudioClip gameplayMusic;

    private bool isFirstSource = true;
    private AudioSource activeSource;
    private AudioSource inactiveSource;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayGameplayMusic();
    }

    private void InitializeAudioSources()
    {
        // Create audio sources if not assigned in inspector
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
        }

        if (musicSource2 == null)
        {
            musicSource2 = gameObject.AddComponent<AudioSource>();
            musicSource2.playOnAwake = false;
            musicSource2.loop = true;
        }

        activeSource = musicSource;
        inactiveSource = musicSource2;
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    public void PlayMusic(AudioClip newClip)
    {
        // Don't restart if the same clip is already playing
        if (activeSource.clip == newClip && activeSource.isPlaying)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(CrossfadeMusic(newClip));
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        // Swap active and inactive sources
        var temp = activeSource;
        activeSource = inactiveSource;
        inactiveSource = temp;

        // Setup new clip
        activeSource.clip = newClip;
        activeSource.volume = 0;
        activeSource.Play();

        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            // Fade out old music
            if (inactiveSource.isPlaying)
                inactiveSource.volume = Mathf.Lerp(defaultVolume, 0, t);

            // Fade in new music
            activeSource.volume = Mathf.Lerp(0, defaultVolume, t);

            yield return null;
        }

        // Ensure clean ending
        inactiveSource.Stop();
        inactiveSource.clip = null;
        activeSource.volume = defaultVolume;

        fadeCoroutine = null;
    }

    public void StopMusic()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float timer = 0;
        float startVolume = activeSource.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            activeSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeDuration);
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = null;
        fadeCoroutine = null;
    }

    public void SetVolume(float volume)
    {
        defaultVolume = Mathf.Clamp01(volume);
        if (!IsFading())
        {
            activeSource.volume = defaultVolume;
        }
    }

    private bool IsFading()
    {
        return fadeCoroutine != null;
    }
}
