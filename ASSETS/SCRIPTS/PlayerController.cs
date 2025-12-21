using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float dribbleSpeed = 3f;
    public float sprintSpeed = 8f;
    
    [Header("Player Stats")]
    public float stamina = 100f;
    public float staminaDrain = 10f;
    public float staminaRegen = 5f;
    public float shotPower = 15f;
    
    [Header("Player Info")]
    public string playerName = "Tsubasa Ozora";
    public int playerNumber = 10;
    public bool hasBall = false;
    
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveDirection;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        Debug.Log($"Jugador {playerNumber} - {playerName} listo!");
    }
    
    void Update()
    {
        // Regenerar stamina
        if (stamina < 100f)
            stamina += staminaRegen * Time.deltaTime;
            
        // Actualizar animaciones
        UpdateAnimations();
    }
    
    void FixedUpdate()
    {
        MovePlayer();
    }
    
    void MovePlayer()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            float currentSpeed = hasBall ? dribbleSpeed : moveSpeed;
            
            // Sprint (gasta stamina)
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                currentSpeed = sprintSpeed;
                stamina -= staminaDrain * Time.deltaTime;
            }
            
            rb.velocity = moveDirection * currentSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    
    void UpdateAnimations()
    {
        if (animator == null) return;
        
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetBool("HasBall", hasBall);
    }
    
    // Para controles táctiles móviles
    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
    }
    
    public void Shoot()
    {
        if (!hasBall || stamina < 20f) return;
        
        stamina -= 20f;
        animator.SetTrigger("Shoot");
        
        Debug.Log($"¡{playerName} dispara con potencia {shotPower}!");
        
        // Aquí iría la lógica del disparo
        hasBall = false;
    }
    
    public void Pass()
    {
        if (!hasBall) return;
        
        animator.SetTrigger("Pass");
        Debug.Log($"{playerName} pasa el balón");
        
        // Lógica de pase
        hasBall = false;
    }
    
    public void SpecialMove(string moveName)
    {
        if (stamina < 50f) return;
        
        stamina -= 50f;
        
        switch(moveName)
        {
            case "DriveShot":
                Debug.Log($"¡{playerName} usa DRIVE SHOT!");
                animator.SetTrigger("DriveShot");
                break;
            case "Overhead":
                Debug.Log($"¡{playerName} usa TIRO DE CHILENA!");
                animator.SetTrigger("Overhead");
                break;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            hasBall = true;
            Debug.Log($"{playerName} tiene el balón!");
        }
    }
}
