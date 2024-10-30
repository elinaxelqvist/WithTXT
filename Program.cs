public class Program
{
    public static void Main(string[] args)
    {
        Game game = new Game();
        game.PlayGame();
    }
        // BowlingLane lane = new BowlingLane();
        // lane.Print();

    //     while (true)
    //     {
    //         Console.WriteLine("\nChoose your direction:");
    //         Console.WriteLine("1. Left");
    //         Console.WriteLine("2. Straight");
    //         Console.WriteLine("3. Right");
    //         Console.Write("Input (1-3): ");
    //         int directionChoice = int.Parse(Console.ReadLine());

    //         IThrow direction;
    //         switch (directionChoice)
    //         {
    //             case 1:
    //                 direction = new LeftDirection();
    //                 break;
    //             case 2:
    //                 direction = new StraightDirection();
    //                 break;
    //             case 3:
    //                 direction = new RightDirection();
    //                 break;
    //             default:
    //                 Console.WriteLine("Invalid choice. Defaulting to Straight.");
    //                 direction = new StraightDirection();
    //                 break;
    //         }

    //         Console.WriteLine("\nChoose your power:");
    //         Console.WriteLine("1. Weak");
    //         Console.WriteLine("2. Strong");
    //         Console.Write("Input (1-2): ");
    //         int powerChoice = int.Parse(Console.ReadLine());

    //         IThrow power = powerChoice == 1 ? new WeakPower() : new StrongPower();

    //         Player player = new Player("Player 1", power, direction);
    //         player.PerformThrow(lane);

    //         Console.WriteLine("\nDo you want to throw again? (y/n)");
    //         if (Console.ReadLine().ToLower() != "y")
    //             break;
    //     }

    //     Console.WriteLine("Game Over!");
    // }

    //    ComputerPlayer computerPlayer = new ComputerPlayer("Computer");
    //    computerPlayer.PerformThrow(lane);
    // }
}