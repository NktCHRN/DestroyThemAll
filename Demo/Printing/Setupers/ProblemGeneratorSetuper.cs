using Demo.UI;
using ProblemGenerators;

namespace Demo.Printing.Setupers;
internal static class ProblemGeneratorSetuper
{
    public static int GetObjectsCount(ProblemGenerator generator)
    {
        return new NumberForm<int>
            {
                Name = "objects quantity"
            }
            .WithMinGreaterThanOrEqualTo(1)
            .GetNumber();
    }

    public static void SetupProblemGenerator(ProblemGenerator generator)
    {
        Console.WriteLine("Let's setup a problem generator");
        SetupProblemGeneratorOptionalProperties(generator);
        SetupMaxSoldiersCountCoefficient(generator);
    }

    public static void SetupProblemGeneratorOptionalProperties(ProblemGenerator generator)
    {
        new Dialog
        {
            Question = $"Do you want to change min and max soldiers per object quantity (default: [{generator.MinSoldiersPerObjectCount};{generator.MaxSoldiersPerObjectCount}])?",
            YAction = () =>
            {
                generator.MinSoldiersPerObjectCount = new NumberForm<int>
                {
                    Name = "min soldiers per object quantity"
                }
                .WithMinGreaterThanOrEqualTo(1)
                .GetNumber();

                generator.MaxSoldiersPerObjectCount = new NumberForm<int>
                {
                    Name = "max soldiers per object quantity"
                }
                .WithMinGreaterThanOrEqualTo(generator.MinSoldiersPerObjectCount)
                .GetNumber();
            }
        }.Print();

        new Dialog
        {
            Question = $"Do you want to change min and max time per object (default: [{generator.MinTimePerObject};{generator.MaxTimePerObject}])?",
            YAction = () =>
            {
                generator.MinTimePerObject = new NumberForm<double>
                {
                    Name = "min time per object"
                }
                .WithMinGreaterThan(0)
                .GetNumber();

                generator.MaxTimePerObject = new NumberForm<double>
                {
                    Name = "max time per object"
                }
                .WithMinGreaterThanOrEqualTo(generator.MinTimePerObject)
                .GetNumber();
            }
        }.Print();
    }

    public static void SetupMaxSoldiersCountCoefficient(ProblemGenerator generator)
    {
        new Dialog
        {
            Question = $"Do you want to change max soldiers count coefficient (default: {generator.MaxSoldiersCountCoefficient})?",
            YAction = () =>
            {
                generator.MaxSoldiersCountCoefficient = new NumberForm<double>
                {
                    Name = "max soldiers count coefficient"
                }
                .WithMinGreaterThan(0)
                .WithMaxLessThanOrEqualTo(1)
                .GetNumber();
            }
        }.Print();
    }
}
