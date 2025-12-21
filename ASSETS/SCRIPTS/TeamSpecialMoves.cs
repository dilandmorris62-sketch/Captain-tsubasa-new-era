using UnityEngine;

public class TeamSpecialMoves : MonoBehaviour
{
    public static string GetTeamSpecialMove(string teamId)
    {
        switch(teamId)
        {
            case "nankatsu":
                return Random.Range(0, 2) == 0 ? "Drive Shot" : "Tiger Shot";
                
            case "toho":
                return "Tiger Shot";
                
            case "musashi":
                return "Beauty Shot";
                
            case "furano":
                return "Hurricane Shot";
                
            case "brazil":
                return "Miracle Overhead";
                
            case "germany":
                return "Fire Shot";
                
            case "japan":
                string[] japanMoves = {"Drive Shot", "Tiger Shot", "Beauty Shot"};
                return japanMoves[Random.Range(0, japanMoves.Length)];
                
            default:
                return "Normal Shot";
        }
    }
    
    public static Color GetTeamColor(string teamId)
    {
        switch(teamId)
        {
            case "nankatsu": return Color.red;
            case "toho": return Color.blue;
            case "musashi": return Color.green;
            case "furano": return new Color(1f, 0.65f, 0f); // Naranja
            case "brazil": return Color.yellow;
            case "germany": return Color.black;
            case "japan": return Color.blue;
            default: return Color.white;
        }
    }
    
    public static string GetTeamCaptain(string teamId)
    {
        switch(teamId)
        {
            case "nankatsu": return "Tsubasa Ozora";
            case "toho": return "Kojiro Hyuga";
            case "musashi": return "Jun Misugi";
            case "furano": return "Hikaru Matsuyama";
            case "brazil": return "Carlos Santana";
            case "germany": return "Karl Heinz Schneider";
            case "japan": return "Tsubasa Ozora";
            default: return "Capitán";
        }
    }
}
