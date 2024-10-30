using System;
using System.Collections.Generic;
using System.IO;

public class BowlingStatistics
{
    private List<PlayerData> playerHistory;
    public delegate void ExportToFile<in T>(T data, string filename);

    public BowlingStatistics()
    {
        playerHistory = new List<PlayerData>();
    }

    public void UpdateGameResult(string playerName, int playerScore, string computerName, int computerScore, string winner)
    {
        playerHistory.Add(new PlayerData 
        { 
            Name = playerName, 
            Score = playerScore,
            IsWinner = winner == "Player"
        });
        
        playerHistory.Add(new PlayerData 
        { 
            Name = computerName, 
            Score = computerScore,
            IsWinner = winner == "Computer"
        });

        ExportResults();
    }

    private void ExportResults()
    {
        ExportToFile<List<PlayerData>> exportResults = (List<PlayerData> data, string filename) =>
        {
            string resultText = "Game History:\n";
            foreach (var player in data)
            {
                resultText += $"{player.Name}: {player.Score} points - {(player.IsWinner ? "Winner!" : "")}\n";
            }
            resultText += $"Date: {DateTime.Now}\n";
            
            File.WriteAllText(filename, resultText);
        };
        
        exportResults(playerHistory, "BowlingHistory.txt");
    }

    public void ShowGameStats()
    {
        Console.WriteLine($"\nTotal games played: {GetTotalGames()}");
        Console.WriteLine($"Player wins: {GetPlayerWins()}");
        Console.WriteLine($"Computer wins: {GetComputerWins()}");
    }

    private int GetTotalGames() => playerHistory.Count / 2;
    private int GetPlayerWins() => playerHistory.Count(p => p.Name == "Player" && p.IsWinner);
    private int GetComputerWins() => playerHistory.Count(p => p.Name == "Computer" && p.IsWinner);
}

public class PlayerData
{
    public string Name { get; set; }
    public int Score { get; set; }
    public bool IsWinner { get; set; }
} 