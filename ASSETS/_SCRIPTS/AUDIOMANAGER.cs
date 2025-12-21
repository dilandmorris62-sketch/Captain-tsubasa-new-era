using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop = false;
        
        [HideInInspector] public AudioSource source;
    }
    
    [Header("Configuración de Audio")]
    public List<Sound> sounds = new List<Sound>();
    
    [Header("Canales")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;
    
    private float musicVolume = 0.8f;
    private float sfxVolume = 1.0f;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeAudio()
    {
        // Crear AudioSources si no existen
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
        }
        
        // Cargar configuración de volumen
        LoadVolumeSettings();
        
        Debug.Log("🔊 Audio Manager inicializado");
    }
    
    void LoadVolumeSettings()
    {
        // Cargar de PlayerPrefs o SaveSystem
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }
    
    // MÚSICA
    public void PlayMusic(string musicName)
    {
        Sound music = sounds.Find(s => s.name == musicName);
        if (music != null && music.clip != null)
        {
            musicSource.clip = music.clip;
            musicSource.volume = music.volume * musicVolume;
            musicSource.pitch = music.pitch;
            musicSource.loop = music.loop;
            musicSource.Play();
            
            Debug.Log($"🎵 Reproduciendo música: {musicName}");
        }
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
    
    // EFECTOS DE SONIDO
    public void PlaySFX(string sfxName)
    {
        Sound sfx = sounds.Find(s => s.name == sfxName);
        if (sfx != null && sfx.clip != null)
        {
            sfxSource.PlayOneShot(sfx.clip, sfx.volume * sfxVolume);
        }
    }
    
    public void PlaySFXAtPosition(string sfxName, Vector3 position)
    {
        Sound sfx = sounds.Find(s => s.name == sfxName);
        if (sfx != null && sfx.clip != null)
        {
            AudioSource.PlayClipAtPoint(sfx.clip, position, sfx.volume * sfxVolume);
        }
    }
    
    // SONIDOS DEL JUEGO (específicos para Captain Tsubasa)
    public void PlayKickSound()
    {
        string[] kickSounds = { "Kick1", "Kick2", "Kick3" };
        string randomKick = kickSounds[Random.Range(0, kickSounds.Length)];
        PlaySFX(randomKick);
    }
    
    public void PlayWhistleSound()
    {
        PlaySFX("Whistle");
    }
    
    public void PlayCrowdCheer()
    {
        PlaySFX("CrowdCheer");
    }
    
    public void PlayGoalSound()
    {
        PlaySFX("Goal");
        PlayCrowdCheer();
    }
    
    public void PlaySpecialMoveSound(string moveName)
    {
        string soundName = moveName + "Sound";
        PlaySFX(soundName);
    }
    
    // CONTROL DE VOLUMEN
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        
        Debug.Log($"🔈 Volumen de música ajustado a: {musicVolume}");
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        
        Debug.Log($"🔉 Volumen de SFX ajustado a: {sfxVolume}");
    }
    
    public float GetMusicVolume()
    {
        return musicVolume;
    }
    
    public float GetSFXVolume()
    {
        return sfxVolume;
    }
    
    // SONIDOS ESPECÍFICOS DEL JUEGO
    void Start()
    {
        // Inicializar lista de sonidos (esto se haría en el Inspector normalmente)
        InitializeDefaultSounds();
    }
    
    void InitializeDefaultSounds()
    {
        // Esta función simula los sonidos que configurarías en el Inspector de Unity
        Debug.Log("🎶 Inicializando sonidos por defecto del juego");
        
        // En un proyecto real, estos sonidos se añadirían desde el Inspector
        // Aquí solo ponemos nombres de referencia
        string[] defaultSoundNames = {
            "MenuMusic",
            "MatchMusic",
            "Kick1", "Kick2", "Kick3",
            "Whistle",
            "CrowdCheer",
            "Goal",
            "DriveShotSound",
            "TigerShotSound",
            "EagleShotSound",
            "DragonShotSound",
            "Pass",
            "Tackle",
            "Catch", // Portero atrapa
            "HitPost",
            "ButtonClick",
            "ButtonHover",
            "UnlockSound"
        };
        
        Debug.Log($"🎵 {defaultSoundNames.Length} sonidos configurados para el juego");
    }
    
    // PARA ANDROID
    public void HandleAndroidAudioFocus()
    {
        // Pausar audio cuando el juego pierde foco
        // Esto es importante para Android
    }
    
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // Juego en segundo plano
            musicSource.Pause();
        }
        else
        {
            // Juego vuelve al primer plano
            musicSource.UnPause();
        }
    }
}
