using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance;
    
    [System.Serializable]
    public class Achievement
    {
        public string id;
        public string title;
        public string description;
        public string iconName;
        public int rewardCoins;
        public bool isHidden;
        
        // Requisitos
        public AchievementType type;
        public int requiredValue;
        
        // Estado
        public bool isUnlocked;
        public int currentProgress;
        
        public enum AchievementType
        {
            ScoreGoals,
            WinMatches,
            UseSpecialMoves,
            CompleteTournament,
            UnlockPlayers,
            ScoreHatTrick,
            LongRangeGoal,
            PerfectMatch,
            ComebackWin,
            DailyPlayer
        }
    }
    
    public List<Achievement> allAchievements = new List<Achievement>();
    private Dictionary<string, Achievement> achievementsDict;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeAchievements()
    {
        achievementsDict = new Dictionary<string, Achievement>();
        
        // LOGROS DE INICIO
        allAchievements.Add(new Achievement {
            id = "FIRST_GOAL",
            title = "Primer Gol",
            description = "Marca tu primer gol",
            iconName = "achievement_first_goal",
            rewardCoins = 50,
            isHidden = false,
            type = Achievement.AchievementType.ScoreGoals,
            requiredValue = 1
        });
        
        allAchievements.Add(new Achievement {
            id = "FIRST_WIN",
            title = "Primera Victoria",
            description = "Gana tu primer partido",
            iconName = "achievement_first_win",
            rewardCoins = 100,
            isHidden = false,
            type = Achievement.AchievementType.WinMatches,
            requiredValue = 1
        });
        
        allAchievements.Add(new Achievement {
            id = "DRIVE_MASTER",
            title = "Maestro del Drive Shot",
            description = "Usa el Drive Shot 10 veces",
            iconName = "achievement_drive_master",
            rewardCoins = 150,
            isHidden = false,
            type = Achievement.AchievementType.UseSpecialMoves,
            requiredValue = 10
        });
        
        // LOGROS DE HABILIDAD
        allAchievements.Add(new Achievement {
            id = "HAT_TRICK",
            title = "Hat-Trick",
            description = "Marca 3 goles en un partido",
            iconName = "achievement_hat_trick",
            rewardCoins = 200,
            isHidden = false,
            type = Achievement.AchievementType.ScoreHatTrick,
            requiredValue = 1
        });
        
        allAchievements.Add(new Achievement {
            id = "LONG_RANGE",
            title = "Tirador Lejano",
            description = "Marca un gol desde más de 30 metros",
            iconName = "achievement_long_range",
            rewardCoins = 250,
            isHidden = false,
            type = Achievement.AchievementType.LongRangeGoal,
            requiredValue = 1
        });
        
        allAchievements.Add(new Achievement {
            id = "PERFECT_MATCH",
            title = "Partido Perfecto",
            description = "Gana un partido sin recibir goles",
            iconName = "achievement_perfect",
            rewardCoins = 300,
            isHidden = false,
            type = Achievement.AchievementType.PerfectMatch,
            requiredValue = 1
        });
        
        // LOGROS SECRETOS
        allAchievements.Add(new Achievement {
            id = "COMEBACK_KING",
            title = "Rey de la Remontada",
            description = "Gana un partido estando 2 goles abajo",
            iconName = "achievement_comeback",
            rewardCoins = 500,
            isHidden = true,
            type = Achievement.AchievementType.ComebackWin,
            requiredValue = 1
        });
        
        allAchievements.Add(new Achievement {
            id = "LEGENDARY_CAPTAIN",
            title = "Capitán Legendario",
            description = "Completa el torneo mundial",
            iconName = "achievement_legendary",
            rewardCoins = 1000,
            isHidden = false,
            type = Achievement.AchievementType.CompleteTournament,
            requiredValue = 1
        });
        
        // Crear diccionario
        foreach (var achievement in allAchievements)
        {
            achievementsDict[achievement.id] = achievement;
        }
        
        // Cargar progreso guardado
        LoadAchievementProgress();
        
        Debug.Log($"🏆 Sistema de logros cargado: {allAchievements.Count} logros disponibles");
    }
    
    void LoadAchievementProgress()
    {
        // Cargar de PlayerPrefs o SaveSystem
        foreach (var achievement in allAchievements)
        {
            string key = $"ACHIEVEMENT_{achievement.id}";
            achievement.isUnlocked = PlayerPrefs.GetInt(key + "_UNLOCKED", 0) == 1;
            achievement.currentProgress = PlayerPrefs.GetInt(key + "_PROGRESS", 0);
        }
    }
    
    void SaveAchievementProgress(Achievement achievement)
    {
        string key = $"ACHIEVEMENT_{achievement.id}";
        PlayerPrefs.SetInt(key + "_UNLOCKED", achievement.isUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(key + "_PROGRESS", achievement.currentProgress);
        PlayerPrefs.Save();
    }
    
    // Métodos para actualizar progreso
    public void RecordGoalScored()
    {
        UpdateAchievementProgress("FIRST_GOAL", 1);
        UpdateAchievementProgress("GOAL_SCORER", 1);
    }
    
    public void RecordMatchWon()
    {
        UpdateAchievementProgress("FIRST_WIN", 1);
        UpdateAchievementProgress("VETERAN_PLAYER", 1);
    }
    
    public void RecordSpecialMoveUsed(string moveName)
    {
        if (moveName == "DriveShot")
        {
            UpdateAchievementProgress("DRIVE_MASTER", 1);
        }
    }
    
    public void RecordHatTrick()
    {
        UpdateAchievementProgress("HAT_TRICK", 1);
    }
    
    public void RecordLongRangeGoal()
    {
        UpdateAchievementProgress("LONG_RANGE", 1);
    }
    
    public void RecordPerfectMatch()
    {
        UpdateAchievementProgress("PERFECT_MATCH", 1);
    }
    
    public void RecordComebackWin()
    {
        UpdateAchievementProgress("COMEBACK_KING", 1);
    }
    
    public void RecordTournamentComplete()
    {
        UpdateAchievementProgress("LEGENDARY_CAPTAIN", 1);
    }
    
    void UpdateAchievementProgress(string achievementId, int progressToAdd)
    {
        if (!achievementsDict.ContainsKey(achievementId))
        {
            Debug.LogWarning($"Logro no encontrado: {achievementId}");
            return;
        }
        
        Achievement achievement = achievementsDict[achievementId];
        
        // Si ya está desbloqueado, no hacer nada
        if (achievement.isUnlocked)
            return;
        
        // Actualizar progreso
        achievement.currentProgress += progressToAdd;
        
        // Verificar si se desbloquea
        if (achievement.currentProgress >= achievement.requiredValue)
        {
            UnlockAchievement(achievement);
        }
        else
        {
            SaveAchievementProgress(achievement);
        }
    }
    
    void UnlockAchievement(Achievement achievement)
    {
        achievement.isUnlocked = true;
        SaveAchievementProgress(achievement);
        
        // Otorgar recompensa
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.AddCoins(achievement.rewardCoins);
        }
        
        // Mostrar notificación
        ShowAchievementPopup(achievement);
        
        Debug.Log($"🏆 ¡Logro desbloqueado: {achievement.title}! +{achievement.rewardCoins} monedas");
    }
    
    void ShowAchievementPopup(Achievement achievement)
    {
        // Aquí iría la lógica para mostrar un popup en la UI
        // Por ahora solo lo logueamos
        Debug.Log($"🎉 ¡LOGRO DESBLOQUEADO! 🎉");
        Debug.Log($"🏅 {achievement.title}");
        Debug.Log($"📝 {achievement.description}");
        Debug.Log($"💰 Recompensa: {achievement.rewardCoins} monedas");
        
        // Notificar UIManager si existe
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            // uiManager.ShowAchievementPopup(achievement);
        }
    }
    
    // Métodos para UI
    public List<Achievement> GetUnlockedAchievements()
    {
        List<Achievement> unlocked = new List<Achievement>();
        foreach (var achievement in allAchievements)
        {
            if (achievement.isUnlocked)
                unlocked.Add(achievement);
        }
        return unlocked;
    }
    
    public List<Achievement> GetLockedAchievements()
    {
        List<Achievement> locked = new List<Achievement>();
        foreach (var achievement in allAchievements)
        {
            if (!achievement.isUnlocked && !achievement.isHidden)
                locked.Add(achievement);
        }
        return locked;
    }
    
    public Achievement GetAchievement(string id)
    {
        if (achievementsDict.ContainsKey(id))
            return achievementsDict[id];
        return null;
    }
    
    public int GetTotalAchievements()
    {
        return allAchievements.Count;
    }
    
    public int GetUnlockedCount()
    {
        int count = 0;
        foreach (var achievement in allAchievements)
        {
            if (achievement.isUnlocked)
                count++;
        }
        return count;
    }
    
    public float GetCompletionPercentage()
    {
        if (allAchievements.Count == 0) return 0f;
        return (float)GetUnlockedCount() / allAchievements.Count * 100f;
    }
    
    // Para debugging
    public void PrintAllAchievements()
    {
        Debug.Log("=== LISTA DE LOGROS ===");
        foreach (var achievement in allAchievements)
        {
            string status = achievement.isUnlocked ? "✅" : "❌";
            string hidden = achievement.isHidden ? "(SECRETO)" : "";
            Debug.Log($"{status} {achievement.title} {hidden}");
            Debug.Log($"   Progreso: {achievement.currentProgress}/{achievement.requiredValue}");
            Debug.Log($"   Recompensa: {achievement.rewardCoins} monedas");
        }
    }
    
    public void ResetAllAchievements()
    {
        foreach (var achievement in allAchievements)
        {
            achievement.isUnlocked = false;
            achievement.currentProgress = 0;
            SaveAchievementProgress(achievement);
        }
        Debug.Log("🔄 Todos los logros han sido reiniciados");
    }
}
