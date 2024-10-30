using System;
using System.Drawing;
using System.Numerics;

public interface IThrow
{
    IStrategy Strategy { get; }
    public string Name { get; }
    public string Description { get; }
    public int Number { get; }
}

public interface IStrategy
{
    public string Name { get; }
    public int Number { get; }
    (bool hit, string result) Spin();
}

class WeakPower : IThrow
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Number { get; private set; }
    public IStrategy Strategy { get; private set; }

    public WeakPower(IStrategy strategy)
    {
        Name = "Weak";
        Description = "For Pins in the front row";
        Number = 10;
        Strategy = strategy;
    }
}

class StrongPower : IThrow
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Number { get; private set; }
    public IStrategy Strategy { get; private set; }

    public StrongPower(IStrategy strategy)
    {
        Name = "Strong";
        Description = "For Pins in the back row";
        Number = 100;
        Strategy = strategy;
    }
}

class ForwardSpinStrategy : IStrategy
{
    public string Name { get; private set; }
    public int Number { get; private set; }

    public ForwardSpinStrategy()
    {
        Name = "Left Spin";
        Number = 0;
    }

    public (bool hit, string result) Spin()
    {
        Random random = new Random();
        int spinChance = random.Next(1, 101);
        bool willHit = spinChance <= 50;
        return (willHit, willHit ? "left spin" : "\u001b[91mOps! You slipped!\u001b[0m");
    }
}

class StraightStrategy : IStrategy
{
    public string Name { get; private set; }
    public int Number { get; private set; }

    public StraightStrategy()
    {
        Name = "Straight";
        Number = 50;
    }

    public (bool hit, string result) Spin()
    {
        Random random = new Random();
        int spinChance = random.Next(1, 101);
        bool willHit = spinChance <= 70;
        return (willHit, willHit ? "straight spin" : "\u001b[91mOh no... The ball went out of the lane..\u001b[0m");
    }
}

class BackSpinStrategy : IStrategy
{
    public string Name { get; private set; }
    public int Number { get; private set; }

    public BackSpinStrategy()
    {
        Name = "Right Spin";
        Number = 100;
    }

    public (bool hit, string result) Spin()
    {
        Random random = new Random();
        int spinChance = random.Next(1, 101);
        bool willHit = spinChance <= 60;
        return (willHit, willHit ? "right spin" : "\u001b[91mWops, too much power! The ball got in another persons lane..\u001b[0m");
    }
}

public class Player
{
    private readonly ThrowHandler throwHandler = new ThrowHandler();
    public string Name { get; private set; }
    public IThrow PowerType { get; private set; }
    public IThrow StrategyType { get; private set; }

    public Player(string playerName, IThrow powerType, IThrow strategyType)
    {
        Name = playerName;
        PowerType = powerType;
        StrategyType = strategyType;
    }

    public void UpdateThrowSettings(IThrow powerType, IThrow strategyType)
    {
        PowerType = powerType;
        StrategyType = strategyType;
    }

    public int PerformThrow(BowlingLane lane)
    {
        return throwHandler.PerformThrow(Name, PowerType, lane);
    }
}

public class ComputerPlayer
{
    private readonly ThrowHandler throwHandler = new ThrowHandler();
    private readonly Random random = new Random();
    public string Name { get; private set; }
    public IThrow PowerType { get; private set; }
    public IThrow StrategyType { get; private set; }

    public ComputerPlayer(string name, IThrow powerType, IThrow strategyType)
    {
        Name = name;
        PowerType = powerType;
        StrategyType = strategyType;
    }

    public void UpdateThrowSettings(IThrow powerType, IThrow strategyType)
    {
        PowerType = powerType;
        StrategyType = strategyType;
    }

    public int PerformThrow(BowlingLane lane)
    {
        IStrategy strategy = random.Next(1, 4) switch
        {
            1 => new ForwardSpinStrategy(),
            2 => new StraightStrategy(),
            _ => new BackSpinStrategy()
        };

        var pins = lane.GetPins();
        if (pins.Any(p => p.Y >= 2))
        {
            PowerType = new WeakPower(strategy);
        }
        else
        {
            PowerType = new StrongPower(strategy);
        }

        return throwHandler.PerformThrow(Name, PowerType, lane);
    }
}

public class ThrowHandler
{
    public int PerformThrow(string name, IThrow power, BowlingLane lane)
    {
        Console.WriteLine($"{name} is throwing with {power.Name} power and {power.Strategy.Name} spin.");
        
        var (hit, result) = power.Strategy.Spin();
        if (hit)
        {
            Console.WriteLine($"The ball had good {result}!");
            int pinsDown = lane.MakeThrow(power.Number, power.Strategy.Number);
            Console.WriteLine($"Pins down this throw: {pinsDown}");
            return pinsDown;
        }
        else
        {
            Console.WriteLine($"The throw missed due to {result}!");
            lane.Print();
            return 0;
        }
    }
}