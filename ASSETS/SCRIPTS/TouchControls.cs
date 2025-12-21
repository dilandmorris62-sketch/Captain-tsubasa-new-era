using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour
{
    [Header("Joystick References")]
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public float joystickRange = 100f;
    
    [Header("Button References")]
    public Button shootButton;
    public Button passButton;
    public Button specialButton;
    public Button sprintButton;
    
    [Header("Player Reference")]
    public PlayerController player;
    
    private Vector2 joystickInput = Vector2.zero;
    private bool isDragging = false;
    
    void Start()
    {
        SetupButtons();
    }
    
    void SetupButtons()
    {
        if (shootButton != null)
            shootButton.onClick.AddListener(OnShootPressed);
            
        if (passButton != null)
            passButton.onClick.AddListener(OnPassPressed);
            
        if (specialButton != null)
            specialButton.onClick.AddListener(OnSpecialPressed);
            
        if (sprintButton != null)
        {
            sprintButton.onClick.AddListener(OnSprintPressed);
            // Mantener presionado para sprint
            EventTrigger trigger = sprintButton.gameObject.AddComponent<EventTrigger>();
            
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => { player.moveSpeed = player.sprintSpeed; });
            
            var pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => { player.moveSpeed = 5f; });
            
            trigger.triggers.Add(pointerDown);
            trigger.triggers.Add(pointerUp);
        }
    }
    
    void Update()
    {
        if (player != null && isDragging)
        {
            player.SetMoveDirection(joystickInput);
        }
        
        // Para pruebas en editor
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnShootPressed();
        }
    }
    
    // Joystick táctil
    public void OnJoystickDrag(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;
        
        Vector2 direction = pointerData.position - (Vector2)joystickBackground.position;
        direction = Vector2.ClampMagnitude(direction, joystickRange);
        
        joystickHandle.anchoredPosition = direction;
        joystickInput = direction / joystickRange;
        
        isDragging = true;
    }
    
    public void OnJoystickRelease()
    {
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickInput = Vector2.zero;
        isDragging = false;
        
        if (player != null)
            player.SetMoveDirection(Vector2.zero);
    }
    
    // Botones táctiles
    void OnShootPressed()
    {
        if (player != null)
            player.Shoot();
    }
    
    void OnPassPressed()
    {
        if (player != null)
            player.Pass();
    }
    
    void OnSpecialPressed()
    {
        if (player != null)
            player.SpecialMove("DriveShot");
    }
    
    void OnSprintPressed()
    {
        // Ya manejado por EventTrigger
    }
    
    // Gestos táctiles
    public void OnSwipeUp()
    {
        Debug.Log("Swipe Up - Tiro elevado");
        // Implementar tiro elevado
    }
    
    public void OnSwipeDown()
    {
        Debug.Log("Swipe Down - Slide tackle");
        // Implementar entrada deslizante
    }
}
