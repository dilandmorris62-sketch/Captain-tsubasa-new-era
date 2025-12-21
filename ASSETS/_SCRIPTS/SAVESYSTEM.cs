using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class GameData
    {
        public int totalMatchesPlayed = 0;
        public int totalGoalsScored = 0;
        public int totalWins = 0;
        public int totalLosses = 0;
        
        public List<string> unlockedPlayers = new List<string>();
        public List<string> unlockedMoves = new List<string>();
        
        public int coins = 100;
        public int experience = 0;
        public int playerLevel = 1;
        
        public Dictionary<string, int> playerStats = new Dictionary<string, int>();
        
        // Configuración del juego
        public float musicVolume = 0.8f;
        public float sfxVolume = 1.0f;
        public bool vibrationEnabled = true;
        public string language = "es";
        
        // Progreso del torneo
        public int tournamentRound = 0;
        public List<string> defeatedTeams = new List<string>();
    }
    
    public static SaveSystem Instance;
    private GameData currentData;
    private string savePath;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSaveSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeSaveSystem()
    {
        savePath = Path.Combine(Application.persistentDataPath, "captain_tsubasa_save.json");
        LoadGame();
        
        // Si no hay datos guardados, crear nuevos
        if (currentData == null)
        {
            currentData = new GameData();
            
            // Desbloquear jugadores iniciales
            currentData.unlockedPlayers.Add("Tsubasa Ozora");
            currentData.unlockedPlayers.Add("Kojiro Hyuga");
            currentData.unlockedPlayers.Add("Taro Misaki");
            
            // Desbloquear técnicas iniciales
            currentData.unlockedMoves.Add("DriveShot");
            currentData.unlockedMoves.Add("DoubleTouch");
            
            SaveGame();
        }
        
        Debug.Log($"💾 Sistema de guardado inicializado en: {savePath}");
        Debug.Log($"📊 Datos cargados: {currentData.unlockedPlayers.Count} jugadores, {currentData.unlockedMoves.Count} técnicas");
    }
    
    public void SaveGame()
    {
        try
        {
            string jsonData = JsonUtility.ToJson(currentData, true);
            File.WriteAllText(savePath, jsonData);
            
            Debug.Log("✅ Juego guardado exitosamente");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Error guardando el juego: {e.Message}");
        }
    }
    
    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string jsonData = File.ReadAllText(savePath);
                currentData = JsonUtility.FromJson<GameData>(jsonData);
                
                Debug.Log("✅ Juego cargado exitosamente");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Error cargando el juego: {e.Message}");
                currentData = new GameData();
            }
        }
        else
        {
            Debug.Log("📁 No se encontró archivo de guardado, creando nuevo...");
            currentData = new GameData();
        }
    }
    
    // Métodos públicos para acceder a los datos
    public GameData GetGameData()
    {
        return currentData;
    }
    
    public void AddCoins(int amount)
    {
        currentData.coins += amount;
        SaveGame();
        
        Debug.Log($"💰 +{amount} monedas. Total: {currentData.coins}");
    }
    
    public void AddExperience(int xp)
    {
        currentData.experience += xp;
        
        // Subir de nivel cada 1000 XP
        int newLevel = (currentData.experience / 1000) + 1;
        if (newLevel > currentData.playerLevel)
        {
            currentData.playerLevel = newLevel;
            Debug.Log($"🎉 ¡Subiste al nivel {currentData.playerLevel}!");
        }
        
        SaveGame();
    }
    
    public void UnlockPlayer(string playerName)
    {
        if (!currentData.unlockedPlayers.Contains(playerName))
        {
            currentData.unlockedPlayers.Add(playerName);
            SaveGame();
            
            Debug.Log($"👤 ¡Jugador desbloqueado: {playerName}!");
        }
    }
    
    public void UnlockMove(string moveName)
    {
        if (!currentData.unlockedMoves.Contains(moveName))
        {
            currentData.unlockedMoves.Add(moveName);
            SaveGame();
            
            Debug.Log($"✨ ¡Técnica desbloqueada: {moveName}!");
        }
    }
    
    public void RecordMatchResult(bool won, int goalsScored)
    {
        currentData.totalMatchesPlayed++;
        currentData.totalGoalsScored += goalsScored;
        
        if (won)
            currentData.totalWins++;
        else
            currentData.totalLosses++;
        
        // Recompensas
        int coinReward = won ? 50 : 20;
        int xpReward = won ? 100 : 50;
        
        AddCoins(coinReward);
        AddExperience(xpReward);
        
        SaveGame();
    }
    
    public bool IsPlayerUnlocked(string playerName)
    {
        return currentData.unlockedPlayers.Contains(playerName);
    }
    
    public bool IsMoveUnlocked(string moveName)
    {
        return currentData.unlockedMoves.Contains(moveName);
    }
    
    // Configuración
    public void SetMusicVolume(float volume)
    {
        currentData.musicVolume = volume;
        SaveGame();
    }
    
    public void SetSFXVolume(float volume)
    {
        currentData.sfxVolume = volume;
        SaveGame();
    }
    
    public void SetVibration(bool enabled)
    {
        currentData.vibrationEnabled = enabled;
        SaveGame();
    }
    
    // Para depuración
    public void ResetSaveData()
    {
        currentData = new GameData();
        SaveGame();
        Debug.Log("🔄 Datos de guardado reiniciados");
    }
    
    public void PrintDebugInfo()
    {
        Debug.Log($"=== DEBUG SAVE DATA ===");
        Debug.Log($"Monedas: {currentData.coins}");
        Debug.Log($"Nivel: {currentData.playerLevel}");
        Debug.Log($"XP: {currentData.experience}");
        Debug.Log($"Partidos: {currentData.totalMatchesPlayed}");
        Debug.Log($"Victorias: {currentData.totalWins}");
        Debug.Log($"Jugadores: {currentData.unlockedPlayers.Count}");
        Debug.Log($"Técnicas: {currentData.unlockedMoves.Count}");
    }
}
