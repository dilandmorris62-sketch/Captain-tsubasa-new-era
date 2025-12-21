using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Settings")]
    public int matchTime = 90; // Tiempo en segundos
    public int maxPlayers = 11;
    
    [Header("UI References")]
    public Text scoreText;
    public Text timeText;
    public Text staminaText;
    
    [Header("Game State")]
    public int homeScore = 0;
    public int awayScore = 0;
    public float currentTime = 0;
    public bool isMatchActive = false;
    
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
        }
    }
    
    void Start()
    {
        StartMatch();
    }
    
    void Update()
    {
        if (isMatchActive)
        {
            UpdateTimer();
            UpdateUI();
        }
    }
    
    public void StartMatch()
    {
        homeScore = 0;
        awayScore = 0;
        currentTime = matchTime;
        isMatchActive = true;
        
        Debug.Log("⚽ Partido de Captain Tsubasa iniciado!");
    }
    
    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        
        if (currentTime <= 0)
        {
            EndMatch();
        }
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"{homeScore} - {awayScore}";
            
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    
    public void ScoreGoal(bool isHomeTeam)
    {
        if (isHomeTeam)
            homeScore++;
        else
            awayScore++;
            
        Debug.Log($"¡GOL! Marcador: {homeScore} - {awayScore}");
    }
    
    public void EndMatch()
    {
        isMatchActive = false;
        Debug.Log($"⏰ Partido terminado! Resultado: {homeScore} - {awayScore}");
    }
    
    // Para botones táctiles
    public void OnPauseButton()
    {
        Time.timeScale = 0;
        Debug.Log("Juego pausado");
    }
    
    public void OnResumeButton()
    {
        Time.timeScale = 1;
        Debug.Log("Juego reanudado");
    }
}
