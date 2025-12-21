using UnityEngine;
using UnityEngine.UI;

public class TeamSelectionUI : MonoBehaviour
{
    public Button[] teamButtons;
    public Text selectedTeamText;
    public Button startMatchButton;
    
    void Start()
    {
        SetupTeamButtons();
        startMatchButton.interactable = false;
    }
    
    void SetupTeamButtons()
    {
        // Nankatsu
        teamButtons[0].onClick.AddListener(() => SelectTeam("nankatsu"));
        teamButtons[0].GetComponentInChildren<Text>().text = "Nankatsu";
        teamButtons[0].image.color = Color.red;
        
        // Toho
        teamButtons[1].onClick.AddListener(() => SelectTeam("toho"));
        teamButtons[1].GetComponentInChildren<Text>().text = "Toho";
        teamButtons[1].image.color = Color.blue;
        
        // Musashi
        teamButtons[2].onClick.AddListener(() => SelectTeam("musashi"));
        teamButtons[2].GetComponentInChildren<Text>().text = "Musashi";
        teamButtons[2].image.color = Color.green;
        
        // Furano
        teamButtons[3].onClick.AddListener(() => SelectTeam("furano"));
        teamButtons[3].GetComponentInChildren<Text>().text = "Furano";
        teamButtons[3].image.color = new Color(1f, 0.65f, 0f);
        
        // Brasil
        teamButtons[4].onClick.AddListener(() => SelectTeam("brazil"));
        teamButtons[4].GetComponentInChildren<Text>().text = "Brasil";
        teamButtons[4].image.color = Color.yellow;
        
        // Alemania
        teamButtons[5].onClick.AddListener(() => SelectTeam("germany"));
        teamButtons[5].GetComponentInChildren<Text>().text = "Alemania";
        teamButtons[5].image.color = Color.black;
        
        // Japón
        teamButtons[6].onClick.AddListener(() => SelectTeam("japan"));
        teamButtons[6].GetComponentInChildren<Text>().text = "Japón";
        teamButtons[6].image.color = Color.blue;
    }
    
    void SelectTeam(string teamId)
    {
        SimpleTeamManager.Instance.SelectTeam(teamId);
        selectedTeamText.text = $"Equipo: {SimpleTeamManager.Instance.selectedTeam.name}";
        startMatchButton.interactable = true;
        
        // Elegir oponente aleatorio (no el mismo equipo)
        string[] availableTeams = {
            "nankatsu", "toho", "musashi", "furano", "brazil", "germany", "japan"
        };
        
        string randomOpponent;
        do {
            randomOpponent = availableTeams[Random.Range(0, availableTeams.Length)];
        } while (randomOpponent == teamId);
        
        SimpleTeamManager.Instance.SetOpponent(randomOpponent);
        
        Debug.Log($"🎮 Preparando: {SimpleTeamManager.Instance.selectedTeam.name} vs {SimpleTeamManager.Instance.opponentTeam.name}");
    }
    
    public void StartMatch()
    {
        Debug.Log($"⚽ ¡Partido comenzando! {SimpleTeamManager.Instance.selectedTeam.name} vs {SimpleTeamManager.Instance.opponentTeam.name}");
        // Aquí cargarías la escena del partido
    }
}
