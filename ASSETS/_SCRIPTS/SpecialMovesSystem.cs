using System.Collections.Generic;
using UnityEngine;

public class SpecialMovesSystem : MonoBehaviour
{
    [System.Serializable]
    public class SpecialMove
    {
        public string moveName;
        public string description;
        public float staminaCost;
        public float powerMultiplier;
        public string animationTrigger;
        public KeyCode keyboardShortcut;
        public GameObject visualEffect;
        
        // Solo lectura
        public bool isUnlocked = true;
        public float cooldown = 0f;
    }
    
    [Header("Técnicas Disponibles")]
    public List<SpecialMove> allMoves = new List<SpecialMove>();
    
    [Header("Referencias")]
    public PlayerController player;
    public UIManager uiManager;
    
    private Dictionary<string, SpecialMove> movesDictionary;
    private float globalCooldown = 0f;
    
    void Awake()
    {
        InitializeMoves();
    }
    
    void InitializeMoves()
    {
        movesDictionary = new Dictionary<string, SpecialMove>();
        
        // TÉCNICAS DE TIRO
        allMoves.Add(new SpecialMove {
            moveName = "DriveShot",
            description = "Tiro impulsado de Tsubasa",
            staminaCost = 40f,
            powerMultiplier = 1.8f,
            animationTrigger = "DriveShot",
            keyboardShortcut = KeyCode.Q
        });
        
        allMoves.Add(new SpecialMove {
            moveName = "TigerShot",
            description = "Tiro del tigre de Hyuga",
            staminaCost = 50f,
            powerMultiplier = 2.2f,
            animationTrigger = "TigerShot",
            keyboardShortcut = KeyCode.W
        });
        
        allMoves.Add(new SpecialMove {
            moveName = "EagleShot",
            description = "Tiro del águila de Misaki",
            staminaCost = 45f,
            powerMultiplier = 2.0f,
            animationTrigger = "EagleShot",
            keyboardShortcut = KeyCode.E
        });
        
        allMoves.Add(new SpecialMove {
            moveName = "DragonShot",
            description = "Tiro del dragón de Schneider",
            staminaCost = 55f,
            powerMultiplier = 2.5f,
            animationTrigger = "DragonShot",
            keyboardShortcut = KeyCode.R
        });
        
        // TÉCNICAS DE REGATE
        allMoves.Add(new SpecialMove {
            moveName = "DoubleTouch",
            description = "Regate de doble toque",
            staminaCost = 25f,
            powerMultiplier = 1.0f,
            animationTrigger = "DoubleTouch",
            keyboardShortcut = KeyCode.A
        });
        
        allMoves.Add(new SpecialMove {
            moveName = "HeelLift",
            description = "Elevación de talón",
            staminaCost = 20f,
            powerMultiplier = 1.0f,
            animationTrigger = "HeelLift",
            keyboardShortcut = KeyCode.S
        });
        
        allMoves.Add(new SpecialMove {
            moveName = "SlidingDribble",
            description = "Regate deslizante",
            staminaCost = 30f,
            powerMultiplier = 1.0f,
            animationTrigger = "SlidingDribble",
            keyboardShortcut = KeyCode.D
        });
        
        // Crear diccionario para acceso rápido
        foreach (var move in allMoves)
        {
            movesDictionary[move.moveName] = move;
        }
        
        Debug.Log($"✅ Sistema de técnicas cargado: {allMoves.Count} técnicas disponibles");
    }
    
    void Update()
    {
        UpdateCooldowns();
        HandleKeyboardInput();
    }
    
    void UpdateCooldowns()
    {
        globalCooldown -= Time.deltaTime;
        if (globalCooldown < 0) globalCooldown = 0;
        
        foreach (var move in allMoves)
        {
            move.cooldown -= Time.deltaTime;
            if (move.cooldown < 0) move.cooldown = 0;
        }
    }
    
    void HandleKeyboardInput()
    {
        // Para pruebas en editor
        if (Input.GetKeyDown(KeyCode.Q) && globalCooldown <= 0)
        {
            ExecuteMove("DriveShot");
        }
        if (Input.GetKeyDown(KeyCode.W) && globalCooldown <= 0)
        {
            ExecuteMove("TigerShot");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ExecuteMove("DoubleTouch");
        }
    }
    
    public void ExecuteMove(string moveName)
    {
        if (!movesDictionary.ContainsKey(moveName))
        {
            Debug.LogError($"❌ Técnica no encontrada: {moveName}");
            return;
        }
        
        SpecialMove move = movesDictionary[moveName];
        
        // Verificar condiciones
        if (!move.isUnlocked)
        {
            Debug.Log($"🔒 Técnica {moveName} no desbloqueada");
            return;
        }
        
        if (player.stamina < move.staminaCost)
        {
            Debug.Log($"💨 Stamina insuficiente para {moveName}");
            return;
        }
        
        if (move.cooldown > 0)
        {
            Debug.Log($"⏳ {moveName} en enfriamiento: {move.cooldown:F1}s");
            return;
        }
        
        if (globalCooldown > 0)
        {
            Debug.Log($"🔄 Enfriamiento global: {globalCooldown:F1}s");
            return;
        }
        
        // Ejecutar técnica
        player.stamina -= move.staminaCost;
        move.cooldown = 5f; // 5 segundos de cooldown
        globalCooldown = 1f; // 1 segundo cooldown global
        
        // Animación
        if (player.animator != null)
        {
            player.animator.SetTrigger(move.animationTrigger);
        }
        
        // Efecto visual
        if (move.visualEffect != null)
        {
            Instantiate(move.visualEffect, player.transform.position, Quaternion.identity);
        }
        
        // Aplicar efecto según tipo de técnica
        switch(moveName)
        {
            case "DriveShot":
            case "TigerShot":
            case "EagleShot":
            case "DragonShot":
                ExecuteShotMove(move);
                break;
                
            case "DoubleTouch":
            case "HeelLift":
            case "SlidingDribble":
                ExecuteDribbleMove(move);
                break;
        }
        
        Debug.Log($"✨ {player.playerName} usa {moveName}!");
        
        // Notificar UI
        if (uiManager != null)
        {
            uiManager.ShowSpecialMoveText(moveName);
        }
    }
    
    void ExecuteShotMove(SpecialMove move)
    {
        // Lógica de tiro especial
        if (player.hasBall)
        {
            // Encontrar el balón
            Ball ball = FindObjectOfType<Ball>();
            if (ball != null)
            {
                // Dirección según posición del jugador
                Vector2 shotDirection = player.transform.right;
                
                // Aplicar fuerza con multiplicador
                float shotForce = player.shotPower * move.powerMultiplier;
                ball.ApplyForce(shotDirection * shotForce);
                
                // Efectos adicionales
                Camera.main.GetComponent<CameraShake>().Shake(0.3f, 0.2f);
            }
            
            player.hasBall = false;
        }
    }
    
    void ExecuteDribbleMove(SpecialMove move)
    {
        // Lógica de regate especial
        player.moveSpeed *= 1.5f; // Aumento temporal de velocidad
        Invoke("ResetMoveSpeed", 2f); // Reset después de 2 segundos
        
        // Efecto de invencibilidad temporal
        player.gameObject.layer = LayerMask.NameToLayer("Invincible");
        Invoke("ResetPlayerLayer", 1f);
    }
    
    void ResetMoveSpeed()
    {
        if (player != null)
            player.moveSpeed = 5f;
    }
    
    void ResetPlayerLayer()
    {
        if (player != null)
            player.gameObject.layer = LayerMask.NameToLayer("Players");
    }
    
    // Para controles táctiles
    public void OnDriveShotButton()
    {
        ExecuteMove("DriveShot");
    }
    
    public void OnTigerShotButton()
    {
        ExecuteMove("TigerShot");
    }
    
    public void OnDoubleTouchButton()
    {
        ExecuteMove("DoubleTouch");
    }
    
    public List<string> GetAvailableMoves()
    {
        List<string> available = new List<string>();
        foreach (var move in allMoves)
        {
            if (move.isUnlocked)
                available.Add(move.moveName);
        }
        return available;
    }
    
    public SpecialMove GetMoveInfo(string moveName)
    {
        if (movesDictionary.ContainsKey(moveName))
            return movesDictionary[moveName];
        return null;
    }
}
