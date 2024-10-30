using System.Text.Json;
public class Game
{
    private List<int> playerPoints = new List<int>();
    private List<int> computerPoints = new List<int>();
    private BowlingStatistics<GameHistory> gameHistory = new();
    private BowlingStatistics<PlayerFrequency> playerFrequency = new();
    private BowlingLane lane;
    private Player player;
    private ComputerPlayer computerPlayer;

    public Game()
    {
        Console.WriteLine("Welcome to Bowling Game!");
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine() ?? "Player";
        
        lane = new BowlingLane();
        IStrategy defaultStrategy = new StraightStrategy();
        IThrow defaultPower = new WeakPower(defaultStrategy);
        player = new Player(playerName, defaultPower, defaultPower);
        computerPlayer = new ComputerPlayer("Computer", defaultPower, defaultPower);
    }

    public void PlayGame()
    {
        for (int round = 1; round <= 2; round++)
        {
            Console.WriteLine($"\nRound {round}");
            
            // Spelarens tur
            lane = new BowlingLane();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n=== {player.Name}'s Turn ===");
            Console.ResetColor();
            lane.Print();

            for (int i = 0; i < 2; i++)
            {
                // Välj strategi
                Console.WriteLine("\nChoose your strategy:");
                Console.WriteLine("1. Left");
                Console.WriteLine("2. Straight");
                Console.WriteLine("3. Right");
                Console.Write("Input (1-3): ");

                int strategyChoice;
                while (!int.TryParse(Console.ReadLine(), out strategyChoice) || strategyChoice < 1 || strategyChoice > 3)
                {
                    Console.WriteLine("Invalid input. Please enter 1, 2 or 3:");
                }

                // Välj styrka
                Console.WriteLine("\nChoose your power:");
                Console.WriteLine("1. Weak");
                Console.WriteLine("2. Strong");
                Console.Write("Input (1-2): ");

                int powerChoice;
                while (!int.TryParse(Console.ReadLine(), out powerChoice) || powerChoice < 1 || powerChoice > 2)
                {
                    Console.WriteLine("Invalid input. Please enter 1 or 2:");
                }

                IStrategy strategy = strategyChoice switch
                {
                    1 => new ForwardSpinStrategy(),
                    2 => new StraightStrategy(),
                    _ => new BackSpinStrategy()
                };

                IThrow power = powerChoice == 1
                    ? new WeakPower(strategy)
                    : new StrongPower(strategy);

                player.UpdateThrowSettings(power, power);
                int playerScore = player.PerformThrow(lane);
                playerPoints.Add(playerScore);

                if (lane.AllPinsDown())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"All pins are down! {player.Name} wins this round!");
                    Console.ResetColor();
                    break;
                }
            }

            // Datorns tur
            lane = new BowlingLane();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\n=== {computerPlayer.Name}'s Turn ===");
            Console.ResetColor();

            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(1000);
                int computerScore = computerPlayer.PerformThrow(lane);
                computerPoints.Add(computerScore);

                if (lane.AllPinsDown())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"All pins are down! {computerPlayer.Name} wins this round!");
                    Console.ResetColor();
                    break;
                }
            }
        }

        Console.WriteLine("\nGame over! 5 rounds completed.");
        var (winner, playerTotal, computerTotal) = AnalyzeScores(playerPoints, computerPoints);

        Console.WriteLine("\nFinal Scores:");
        Console.WriteLine($"{player.Name}: {playerTotal} points");
        Console.WriteLine($"Computer: {computerTotal} points");

        if (winner == "Tie")
        {
            Console.WriteLine("\nIt's a tie!");
        }
        else
        {
            Console.WriteLine($"\n{winner} wins with {(winner == player.Name ? playerTotal : computerTotal)} points!");
            Console.WriteLine($"Winning margin: {Math.Abs(playerTotal - computerTotal)} points");
        }

        // Uppdatera spelhistorik
        gameHistory.AddData(new GameHistory 
        { 
            PlayerName = player.Name,
            Score = playerTotal,
        });

        // Uppdatera spelarfrekvens
        playerFrequency.AddData(new PlayerFrequency 
        { 
            PlayerName = player.Name,
        });

        // Visa statistik
        Console.WriteLine("\nPress Enter to see more statistics...");
        Console.ReadLine();
        Console.Clear();

        // Visa topp 3
        gameHistory.ShowStatistics();

        // Fråga efter specifik spelare
        Console.WriteLine("\nEnter player name to see their total games: ");
        string searchName = Console.ReadLine() ?? "";
        playerFrequency.ShowStatistics(searchName);
    }

    private (string winner, int playerTotal, int computerTotal) AnalyzeScores(List<int> playerPoints, List<int> computerPoints)
    {
        int playerTotal = playerPoints.Sum();
        int computerTotal = computerPoints.Sum();
        
        string winner = playerTotal > computerTotal 
            ? player.Name
            : playerTotal < computerTotal 
                ? "Computer" 
                : "Tie";

        return (winner, playerTotal, computerTotal);
    }
}
