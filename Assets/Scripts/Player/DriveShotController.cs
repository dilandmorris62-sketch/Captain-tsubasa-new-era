// DriveShotController.cs (específico para tu set)
using UnityEngine;

public class DriveShotController : MonoBehaviour
{
    [Header("Drive Shot Sprites (Tus 6 PNGs)")]
    public Sprite[] driveShotFrames = new Sprite[6]; // 0-5
    
    [Header("Efectos Especiales")]
    public GameObject blueBallEffect;    // Tu frame 5
    public GameObject eagleEffect;       // Tu frame 6
    
    [Header("Configuración")]
    public float frameTime = 0.1f;       // 10 FPS
    public int ballCreationFrame = 3;    // Frame 4 (índice 3)
    public int eagleSpawnFrame = 4;      // Frame 5 (índice 4)
    
    private SpriteRenderer playerSprite;
    private bool isDoingDriveShot = false;
    private int currentDriveFrame = 0;
    private float driveTimer = 0f;
    private GameObject currentBlueBall;
    private GameObject currentEagle;
    
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        Debug.Log("✅ Controlador Drive Shot listo (6 frames detectados)");
    }
    
    public void StartDriveShot()
    {
        if (isDoingDriveShot) return;
        
        isDoingDriveShot = true;
        currentDriveFrame = 0;
        driveTimer = 0f;
        
        // Empezar con primer frame
        playerSprite.sprite = driveShotFrames[0];
        
        Debug.Log("🚀 INICIANDO DRIVE SHOT...");
    }
    
    void Update()
    {
        if (!isDoingDriveShot) return;
        
        driveTimer += Time.deltaTime;
        
        if (driveTimer >= frameTime)
        {
            driveTimer = 0f;
            currentDriveFrame++;
            
            // Si terminamos los 4 frames del cuerpo
            if (currentDriveFrame >= 4)
            {
                // Después del frame 4, volver a idle
                isDoingDriveShot = false;
                GetComponent<PlayerController>().SetToIdle();
                Debug.Log("🎯 Drive Shot completado");
                return;
            }
            
            // Cambiar sprite del jugador (solo frames 1-4)
            playerSprite.sprite = driveShotFrames[currentDriveFrame];
            
            // EVENTOS ESPECIALES EN FRAMES CLAVE
            if (currentDriveFrame == ballCreationFrame) // Frame 3 (impacto)
            {
                CreateBlueBallEffect();
                Debug.Log("💥 BALÓN ESPECIAL CREADO");
            }
            
            if (currentDriveFrame == eagleSpawnFrame) // Frame 4
            {
                CreateEagleEffect();
                Debug.Log("🦅 ÁGULLA CREADA");
            }
        }
    }
    
    void CreateBlueBallEffect()
    {
        if (blueBallEffect != null)
        {
            // Crear el balón azul como objeto separado
            currentBlueBall = Instantiate(blueBallEffect, 
                transform.position + Vector3.right * 1.5f, 
                Quaternion.identity);
            
            // Añadir física para que se mueva
            Rigidbody2D rb = currentBlueBall.AddComponent<Rigidbody2D>();
            rb.velocity = Vector2.right * 15f; // Velocidad hacia la derecha
            rb.gravityScale = 0f; // Sin gravedad
            
            // Destruir después de 2 segundos
            Destroy(currentBlueBall, 2f);
        }
    }
    
    void CreateEagleEffect()
    {
        if (eagleEffect != null)
        {
            // El águila sigue al balón azul
            currentEagle = Instantiate(eagleEffect,
                transform.position + Vector3.right * 1f,
                Quaternion.identity);
            
            // Hacer que el águila siga al balón
            if (currentBlueBall != null)
            {
                currentEagle.GetComponent<EagleFollower>()?.SetTarget(currentBlueBall);
            }
            
            Destroy(currentEagle, 1.5f);
        }
    }
    
    // Para cancelar si es necesario
    public void CancelDriveShot()
    {
        isDoingDriveShot = false;
        if (currentBlueBall != null) Destroy(currentBlueBall);
        if (currentEagle != null) Destroy(currentEagle);
    }
}
