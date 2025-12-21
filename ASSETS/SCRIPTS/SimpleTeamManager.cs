using UnityEngine;
using System.Collections.Generic;

public class SimpleTeamManager : MonoBehaviour
{
    [System.Serializable]
    public class Team
    {
        public string id;
        public string name;
        public string country;
        public Color colorPrimary;
        public Color colorSecondary;
    }
    
    public static SimpleTeamManager Instance;
    
    public List<Team> allTeams = new List<Team>();
    public Team selectedTeam;
    public Team opponentTeam;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateTeams();
        }
    }
    
    void CreateTeams()
    {
        // Nankatsu
        allTeams.Add(new Team {
            id = "nankatsu",
            name = "Nankatsu SC",
            country = "Japón",
            colorPrimary = Color.red,
            colorSecondary = Color.white
        });
        
        // Toho
        allTeams.Add(new Team {
            id = "toho",
            name = "Toho Academy",
            country = "Japón",
            colorPrimary = Color.blue,
            colorSecondary = Color.white
        });
        
        // Musashi
        allTeams.Add(new Team {
            id = "musashi",
            name = "Musashi FC",
            country = "Japón",
            colorPrimary = Color.green,
            colorSecondary = Color.white
        });
        
        // Furano
        allTeams.Add(new Team {
            id = "furano",
            name = "Furano FC",
            country = "Japón",
            colorPrimary = new Color(1f, 0.65f, 0f), // Naranja
            colorSecondary = Color.black
        });
        
        // Brasil
        allTeams.Add(new Team {
            id = "brazil",
            name = "Brasil",
            country = "Brasil",
            colorPrimary = Color.yellow,
            colorSecondary = Color.green
        });
        
        // Alemania
        allTeams.Add(new Team {
            id = "germany",
            name = "Alemania",
            country = "Alemania",
            colorPrimary = Color.black,
            colorSecondary = Color.red
        });
        
        // Japón (Selección)
        allTeams.Add(new Team {
            id = "japan",
            name = "Japón",
            country = "Japón",
            colorPrimary = Color.blue,
            colorSecondary = Color.white
        });
        
        Debug.Log($"✅ {allTeams.Count} equipos creados");
    }
    
    public Team GetTeam(string teamId)
    {
        return allTeams.Find(t => t.id == teamId);
    }
    
    public void SelectTeam(string teamId)
    {
        selectedTeam = GetTeam(teamId);
        Debug.Log($"⚽ Equipo seleccionado: {selectedTeam.name}");
    }
    
    public void SetOpponent(string teamId)
    {
        opponentTeam = GetTeam(teamId);
        Debug.Log($"🆚 Oponente: {opponentTeam.name}");
    }
    
    public string[] GetTeamNames()
    {
        string[] names = new string[allTeams.Count];
        for (int i = 0; i < allTeams.Count; i++)
        {
            names[i] = allTeams[i].name;
        }
        return names;
    }
}
