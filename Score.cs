using System.Collections.Generic;
using System.Linq;

public class BowlingScore
{
    public string PlayerType { get; set; }  // "Player" eller "Computer"
    public int Score { get; set; }
}

public class Score : IEnumerable<int>
{
    private List<int> throwScores = new List<int>();

    // Förenklad iterator som använder den underliggande kollektionen direkt
    public IEnumerator<int> GetEnumerator()
    {
        return throwScores.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void AddPoints(int points)
    {
        throwScores.Add(points);
    }

    public int GetTotalScore() //LINQ
    {
        int total = 0;
        foreach(int score in throwScores)
        {
            total += score;
        }
        return total;
    }

    public (string winner, int playerTotal, int computerTotal) AnalyzeScores(List<int> playerPoints, List<int> computerPoints)
    {
        int playerTotal = playerPoints.Sum();
        int computerTotal = computerPoints.Sum();
        
        string winner;
        if (playerTotal > computerTotal)
            winner = "Player";
        else if (computerTotal > playerTotal)
            winner = "Computer";
        else
            winner = "Tie";

        return (winner, playerTotal, computerTotal);
    }
}
