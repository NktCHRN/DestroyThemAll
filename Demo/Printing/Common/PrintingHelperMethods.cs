using Common;
using Demo.UI;

namespace Demo.Printing.Common;
internal static class PrintingHelperMethods
{
    public static void PrintProblem(Problem problem)
    {
        Console.WriteLine("Objects:");
        var j = 1;
        foreach (var militaryObject in problem.MilitaryObjects)
        {
            Console.WriteLine($"{j}. Name: {militaryObject.Name}; Time: {militaryObject.Time}; Soldiers count: {militaryObject.SoldiersCount}");
            j++;
        }
        Console.WriteLine();
        Console.WriteLine($"Available soldiers: {problem.MaxSoldiersCount}");
    }

    public static int GetObjectsCount()
    {
        return new NumberForm<int>
        {
            Name = "objects quantity"
        }
            .WithMinGreaterThanOrEqualTo(1)
            .GetNumber();
    }

    public static LoopParameters<T> GetLoopParameters<T>(T lowerBoundary, T? upperBoundary = null) where T : struct, IComparable<T>
    {
        var start = new NumberForm<T>
        {
            Name = "start"
        }
            .WithMinGreaterThan(lowerBoundary)
            .GetNumber();

        var step = new NumberForm<T>
        {
            Name = "step"
        }
            .WithMinGreaterThan(default)
            .GetNumber();

        var endForm = new NumberForm<T>
        {
            Name = "end"
        }
            .WithMinGreaterThanOrEqualTo(start);
        if (upperBoundary is not null)
        {
            endForm = endForm
                .WithMaxLessThan(upperBoundary.Value);
        }
        var end = endForm.GetNumber();

        return new(start, step, end);
    }
}
