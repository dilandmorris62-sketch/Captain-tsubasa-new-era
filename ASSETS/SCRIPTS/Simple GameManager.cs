using UnityEngine;
using UnityEngine.UI;

public class SimpleGameManager : MonoBehaviour
{
    public Text scoreText;
    public Text timeText;
    public Text team1Text;
    public Text team2Text;
    
    private int team1Score = 0;
    private int team2Score = 0;
    private float matchTime = 300f; // 5 minutos
    
    void Start()
    {
        if (SimpleTeamManager.Instance.selectedTeam != null)
        {
            team1Text.text = SimpleTeamManager.Instance.selectedTeam.name;
            team1Text.color = SimpleTeamManager.Instance.selectedTeam.colorPrimary;
        }
        
        if (SimpleTeamManager.Instance.opponentTeam != null)
        {
            team2Text.text = SimpleTeamManager.Instance.opponentTeam.name;
            team2Text.color = SimpleTeamManager.Instance.opponentTeam.colorPrimary;
        }
    }
    
    void Update()
    {
        if (matchTime > 0)
        {
            matchTime -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            EndMatch();
        }
        
        // Simular goles aleatorios (para pruebas)
        if (Input.GetKeyDown(KeyCode.G))
        {
            bool isTeam1 = Random.Range(0, 2) == 0;
            ScoreGoal(isTeam1);
        }
    }
    
    void UpdateUI()
    {
        scoreText.text = $"{team1Score} - {team2Score}";
        
        int minutes = Mathf.FloorToInt(matchTime / 60);
        int seconds = Mathf.FloorToInt(matchTime % 60);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }
    
    public void ScoreGoal(bool isTeam1)
    {
        if (isTeam1)
            team1Score++;
        else
            team2Score++;
            
        string scoringTeam = isTeam1 ? 
            SimpleTeamManager.Instance.selectedTeam.name : 
            SimpleTeamManager.Instance.opponentTeam.name;
            
        string specialMove = TeamSpecialMoves.GetTeamSpecialMove(
            isTeam1 ? SimpleTeamManager.Instance.selectedTeam.id : 
                     SimpleTeamManager.Instance.opponentTeam.id
        );
            
        Debug.Log($"⚽ ¡GOOOL de {scoringTeam}! ({specialMove})");
        UpdateUI();
    }
    
    void EndMatch()
    {
        Debug.Log($"🏁 Partido terminado: {team1Score} - {team2Score}");
        if (team1Score > team2Score)
            Debug.Log($"🏆 Ganador: {SimpleTeamManager.Instance.selectedTeam.name}");
        else if (team2Score > team1Score)
            Debug.Log($"🏆 Ganador: {SimpleTeamManager.Instance.opponentTeam.name}");
        else
            Debug.Log("🤝 Empate");
    }
    
    // Para botones táctiles
    public void Team1ScoreButton()
    {
        ScoreGoal(true);
    }
    
    public void Team2ScoreButton()
    {
        ScoreGoal(false);
    }
}
