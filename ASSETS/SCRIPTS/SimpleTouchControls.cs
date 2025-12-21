using UnityEngine;
using UnityEngine.UI;

public class SimpleTouchControls : MonoBehaviour
{
    public Button moveUpButton;
    public Button moveDownButton;
    public Button moveLeftButton;
    public Button moveRightButton;
    public Button shootButton;
    public Button passButton;
    public Button specialButton;
    
    private Vector2 moveDirection = Vector2.zero;
    
    void Start()
    {
        SetupButtons();
    }
    
    void SetupButtons()
    {
        // Movimiento
        moveUpButton.onClick.AddListener(() => Move(Vector2.up));
        moveDownButton.onClick.AddListener(() => Move(Vector2.down));
        moveLeftButton.onClick.AddListener(() => Move(Vector2.left));
        moveRightButton.onClick.AddListener(() => Move(Vector2.right));
        
        // Acciones
        shootButton.onClick.AddListener(Shoot);
        passButton.onClick.AddListener(Pass);
        specialButton.onClick.AddListener(UseSpecial);
        
        // Colores por equipo
        Color teamColor = SimpleTeamManager.Instance.selectedTeam.colorPrimary;
        shootButton.image.color = teamColor;
        specialButton.image.color = teamColor;
    }
    
    void Move(Vector2 direction)
    {
        moveDirection = direction;
        Debug.Log($"Movimiento: {direction}");
        // Aquí moverías al jugador
    }
    
    void Shoot()
    {
        Debug.Log($"💥 Disparo normal");
    }
    
    void Pass()
    {
        Debug.Log($"🔄 Pase");
    }
    
    void UseSpecial()
    {
        string specialMove = TeamSpecialMoves.GetTeamSpecialMove(
            SimpleTeamManager.Instance.selectedTeam.id
        );
        Debug.Log($"✨ Técnica especial: {specialMove}");
    }
    
    // Para liberar movimiento
    public void StopMove()
    {
        moveDirection = Vector2.zero;
    }
}
