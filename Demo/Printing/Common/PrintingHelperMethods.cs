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
}
