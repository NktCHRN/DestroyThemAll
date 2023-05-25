using Common;

namespace ProblemGenerators;

public sealed class ProblemGenerator
{
    private readonly Random _random = new();

    public double MaxSoldiersCountCoefficient { get; set; } = 0.75;

    public int MinSoldiersPerObjectCount { get; set; } = 5;
    public int MaxSoldiersPerObjectCount { get; set; } = 10;

    public double MinTimePerObject { get; set; } = 6;
    public double MaxTimePerObject { get; set; } = 12;

    public Problem Generate(int objectsCount = 10)
    {
        var objects = new MilitaryObject[objectsCount];
        var totalSoldiersCount = 0;
        for (var i = 0; i < objectsCount; i++)
        {
            objects[i] = GenerateObject(i + 1);
            totalSoldiersCount += objects[i].SoldiersCount;
        }
        var maxSoldiersCount = (int)Math.Round(totalSoldiersCount * MaxSoldiersCountCoefficient, MidpointRounding.AwayFromZero);
        return new Problem(objects, maxSoldiersCount);
    }

    private MilitaryObject GenerateObject(int number)
    {
        return new MilitaryObject
        (
            Name: $"O{number}",
            SoldiersCount: _random.Next(MinSoldiersPerObjectCount, MaxSoldiersPerObjectCount),
            Time: RoundByMinutes(_random.NextDouble() * (MaxTimePerObject - MinTimePerObject) + MinTimePerObject));
    }

    private static double RoundByMinutes(double value) => Math.Round(value * 60, MidpointRounding.AwayFromZero) / 60;
}
