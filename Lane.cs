public class BowlingLane
{
    private List<Coordinate> pins;

    public BowlingLane()
    {
        ResetPins();
    }

    private void ResetPins()
    {
        pins = new List<Coordinate>
        {
            new Coordinate(0, 0), new Coordinate(0, 1), new Coordinate(0, 2), new Coordinate(0, 3),
            new Coordinate(1, 0), new Coordinate(1, 1), new Coordinate(1, 2),
            new Coordinate(2, 0), new Coordinate(2, 1),
            new Coordinate(3, 0)
        };
    }

    public void Print(Score score = null)
    {
        Console.WriteLine("\nBowling Lane:");
        
        // Skriv ut kolumnnummer
        Console.Write("  ");
        for (int j = 0; j < 4; j++)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" {j}");
            Console.ResetColor();
        }
        Console.WriteLine();  // Ny rad efter kolumnnummer

        // Skriv ut spelplanen med radnummer och käglor
        for (int i = 0; i < 4; i++)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{i} ");
            Console.ResetColor();

            for (int j = 0; j < 4; j++)
            {
                if (pins.Contains(new Coordinate(i, j)))
                    Console.Write(" X");
                else
                    Console.Write(" •");
            }

            // Lägg till poänginfo till höger
            if (score != null)
            {
                switch(i)
                {
                    case 0:
                        Console.Write($"     Total points: {score.GetTotalScore()}");
                        break;
                    case 1:
                        Console.Write($"     Best Throw: {score.GetTotalScore()}");
                        break;
                }
            }
            Console.WriteLine();
        }
    }

    public int MakeThrow(int power, int direction)
    {
        int pinsDownCount = 0;
        int startColumn, endColumn;

        // Definiera kolumner baserat på riktning
        if (direction <= 25)       // Vänster
        {
            startColumn = 0;
            endColumn = 1;  // Kolumner 0 och 1 ska träffas för Left
        }
        else if (direction <= 75)  // Mitten
        {
            startColumn = 1;
            endColumn = 2;  // Kolumner 1 och 2 ska träffas för Straight
        }
        else                       // Höger
        {
            startColumn = 2;
            endColumn = 3;  // Kolumner 2 och 3 ska träffas för Right
        }

        // Bestäm rader som påverkas beroende på kaststyrkan
        int[] rowsToAffect;
        if (power <= 40) // WeakPower
        {
            rowsToAffect = new int[] { 2, 3 };  // Svagt kast träffar bara raderna 0 och 1
        }
        else // StrongPower
        {
            rowsToAffect = new int[] { 0, 1, 2, 3 };  // Starkt kast träffar alla rader (0 till 3)
        }

        Console.WriteLine($"Throwing with power: {power} and aiming for columns: {startColumn}-{endColumn}");

        // Påverka de valda kolumnerna och raderna
        foreach (int row in rowsToAffect)
        {
            for (int col = startColumn; col <= endColumn; col++)
            {
                Coordinate pin = new Coordinate(row, col);
                if (pins.Contains(pin))
                {
                    pins.Remove(pin);
                    pinsDownCount++;
                    Console.WriteLine($"Hit pin at column {col}, row {row}");
                }
            }
        }

        Print();
        return pinsDownCount;
    }

    public bool AllPinsDown()
    {
        return pins.Count == 0;
    }

    public struct Coordinate
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
