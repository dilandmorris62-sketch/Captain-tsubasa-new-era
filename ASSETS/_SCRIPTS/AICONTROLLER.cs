using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        ChasingBall,
        Defending,
        Attacking,
        Passing,
        Shooting
    }
    
    [Header("Configuración IA")]
    public AIState currentState = AIState.Idle;
    public float reactionTime = 0.5f;
    public float decisionInterval = 1f;
    
    [Header("Rangos de Distancia")]
    public float chaseDistance = 10f;
    public float shootDistance = 5f;
    public float passDistance = 8f;
    
    [Header("Referencias")]
    public PlayerController player;
    public GameObject ball;
    public Transform homeGoal;
    public Transform awayGoal;
    
    private Vector2 targetPosition;
    private float lastDecisionTime;
    
    void Start()
    {
        player = GetComponent<PlayerController>();
        StartCoroutine(FindBallRoutine());
    }
    
    void Update()
    {
        if (Time.time - lastDecisionTime > decisionInterval)
        {
            MakeDecision();
            lastDecisionTime = Time.time;
        }
        
        ExecuteCurrentState();
    }
    
    IEnumerator FindBallRoutine()
    {
        while (true)
        {
            ball = GameObject.FindGameObjectWithTag("Ball");
            yield return new WaitForSeconds(1f);
        }
    }
    
    void MakeDecision()
    {
        if (ball == null) return;
        
        float distanceToBall = Vector2.Distance(transform.position, ball.transform.position);
        
        // Lógica de decisión simple
        if (player.hasBall)
        {
            // Tengo el balón
            float distanceToGoal = Vector2.Distance(transform.position, awayGoal.position);
            
            if (distanceToGoal <= shootDistance && player.stamina > 30f)
            {
                currentState = AIState.Shooting;
            }
            else
            {
                // Buscar compañero para pasar
                GameObject teammate = FindNearestTeammate();
                if (teammate != null && distanceToBall > passDistance)
                {
                    currentState = AIState.Passing;
                }
                else
                {
                    currentState = AIState.Attacking;
                }
            }
        }
        else
        {
            // No tengo el balón
            if (distanceToBall <= chaseDistance)
            {
                currentState = AIState.ChasingBall;
            }
            else
            {
                // Posicionamiento defensivo
                currentState = AIState.Defending;
            }
        }
    }
    
    void ExecuteCurrentState()
    {
        switch (currentState)
        {
            case AIState.ChasingBall:
                ChaseBall();
                break;
                
            case AIState.Defending:
                DefendPosition();
                break;
                
            case AIState.Attacking:
                AttackGoal();
                break;
                
            case AIState.Passing:
                LookForPass();
                break;
                
            case AIState.Shooting:
                ShootAtGoal();
                break;
        }
    }
    
    void ChaseBall()
    {
        if (ball != null)
        {
            Vector2 direction = (ball.transform.position - transform.position).normalized;
            player.SetMoveDirection(direction);
        }
    }
    
    void DefendPosition()
    {
        // Posicionarse entre el balón y la portería
        if (ball != null && homeGoal != null)
        {
            Vector2 defendPosition = (Vector2)homeGoal.position + 
                                   ((Vector2)ball.transform.position - (Vector2)homeGoal.position) * 0.5f;
            
            Vector2 direction = (defendPosition - (Vector2)transform.position).normalized;
            player.SetMoveDirection(direction);
        }
    }
    
    void AttackGoal()
    {
        if (awayGoal != null)
        {
            Vector2 direction = (awayGoal.position - transform.position).normalized;
            player.SetMoveDirection(direction);
        }
    }
    
    void LookForPass()
    {
        GameObject teammate = FindNearestTeammate();
        if (teammate != null)
        {
            // Orientarse hacia el compañero
            Vector2 direction = (teammate.transform.position - transform.position).normalized;
            transform.up = direction;
            
            // Pasar si está en buen ángulo
            float angle = Vector2.Angle(transform.up, direction);
            if (angle < 30f)
            {
                player.Pass();
            }
        }
    }
    
    void ShootAtGoal()
    {
        if (awayGoal != null && player.hasBall)
        {
            // Apuntar a la portería
            Vector2 direction = (awayGoal.position - transform.position).normalized;
            transform.up = direction;
            
            // Disparar
            player.Shoot();
            
            // Posibilidad de usar técnica especial
            if (Random.Range(0f, 1f) > 0.7f && player.stamina > 40f)
            {
                // Usar técnica especial aleatoria
                string[] shotMoves = { "DriveShot", "TigerShot", "EagleShot" };
                string randomMove = shotMoves[Random.Range(0, shotMoves.Length)];
                
                // Necesitarías una referencia al SpecialMovesSystem
                // specialMoves.ExecuteMove(randomMove);
            }
        }
    }
    
    GameObject FindNearestTeammate()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearest = null;
        float nearestDistance = Mathf.Infinity;
        
        foreach (GameObject p in players)
        {
            if (p == gameObject) continue; // No soy yo mismo
            
            float distance = Vector2.Distance(transform.position, p.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = p;
            }
        }
        
        return nearest;
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualizar rangos en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootDistance);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, passDistance);
    }
}
