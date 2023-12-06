using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxHeroSource1; // Q and E
    public AudioSource sfxHeroSource2; // W
    public AudioSource sfxHeroSource3; // R
    public AudioSource sfxMiscSource;
    public AudioSource sfxBossSource1; //Normal Attack
    public AudioSource sfxBossSource2; //Global Skill
    public AudioSource sfxBossSource3; //Roar Skill
    public AudioSource sfxBossSource4; //Tidal Skill
    public AudioSource sfxBossSource5; //Ultimate Skill
    public AudioSource musicSource;

    public float volumeBossSFX = 1f;
    public AudioClip IntroTheme;
    public AudioClip Click;

    private bool canClickTheme = true;
    private float clickCooldown = 0.1f;

    void Awake()
    {
        // Implementing a singleton pattern to ensure only one AudioManager instance exists
        if (instance == null)
        {
            instance = this;
            PlayMusic(IntroTheme);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0) && canClickTheme)
        {
            PlayClickSound(Click, clickCooldown);
        }
    }

    void PlayClickSound(AudioClip music, float cooldown)
    {
        if (canClickTheme)
        {
            PlayMicsSFX(music, 0.5f);

            // Start cooldown
            StartCoroutine(IntroThemeCooldown(cooldown));
        }
    }

    // Coroutine for cooldown
    private System.Collections.IEnumerator IntroThemeCooldown(float cooldown)
    {
        canClickTheme = false;
        yield return new WaitForSeconds(cooldown);
        canClickTheme = true;
    }

    // Play a single sound clip through the sound effects audio source
    public void PlayHeroSFX1(AudioClip clip, float volume = 1f)
    {
        sfxHeroSource1.clip = clip;
        sfxHeroSource1.volume = volume;
        sfxHeroSource1.Play();
    }

    public void PlayHeroSFX2(AudioClip clip, float volume = 1f)
    {
        sfxHeroSource2.clip = clip;
        sfxHeroSource2.volume = volume;
        sfxHeroSource2.Play();
    }

    public void PlayHeroSFX3(AudioClip clip, float volume = 1f)
    {
        sfxHeroSource3.clip = clip;
        sfxHeroSource3.volume = volume;
        sfxHeroSource3.Play();
    }

    public void PlayMicsSFX(AudioClip clip, float volume = 1f)
    {
        sfxMiscSource.clip = clip;
        sfxMiscSource.Play();
    }

    public void PlayBossSFX1(AudioClip clip)
    {
        sfxBossSource1.clip = clip;
        sfxBossSource1.volume = volumeBossSFX - 0.5f; // Set the volume
        sfxBossSource1.Play();
    }

    public void PlayBossSFX2(AudioClip clip)
    {
        sfxBossSource2.clip = clip;
        sfxBossSource2.Play();
    }
    public void PlayBossSFX3(AudioClip clip)
    {
        sfxBossSource3.clip = clip;
        sfxBossSource3.Play();
    }
    public void PlayBossSFX4(AudioClip clip)
    {
        sfxBossSource4.Stop();
        sfxBossSource4.clip = clip;
        sfxBossSource4.Play();
    }
    public void PlayBossSFX5(AudioClip clip)
    {
        sfxBossSource5.clip = clip;
        sfxBossSource5.volume = volumeBossSFX + 0.3f;
        sfxBossSource5.Play();
    }

    // Play music (e.g., for boss battles)
    public void PlayMusic(AudioClip music, float volume = 1f)
    {
        musicSource.clip = music;
        musicSource.volume = volume;
        musicSource.loop = true; // Set loop to true
        musicSource.Play();
    }
}
