using Common;
using Demo.UI;
using Solvers.Common;
using Solvers.Solvers.Bruteforce;
using Solvers.Solvers.Dynamic;
using Solvers.Solvers.Genetic;
using Solvers.Solvers.Greedy;

namespace Demo.Printers;
public sealed class SolverPrinter : IPrinter
{
    public void Print()
    {
        HelperMethods.PrintHeader(Constants.Header);
        Console.WriteLine("How do you want to enter the problem?");
        Problem problem = null!;
        new LiteMenu
        {
            IsQuitable = true,
            Name = "option",
            Items = new LiteMenuItem[]
            {
                new LiteMenuItem
                {
                    Text = "Enter the problem manually",
                    Action = () => problem = EnterTheProblemManually()
                },
                new LiteMenuItem
                {
                    Text = "Generate the problem"
                },
                new LiteMenuItem
                {
                    Text = "Enter the problem from file"
                },
            },
            Callback = () =>
            {
                SolveProblem(problem);
                HelperMethods.Quit();
            }
        }.Print();
    }

    private static Problem EnterTheProblemManually()
    {
        Console.WriteLine();
        var objectsCount = new NumberForm<int>
        {
            Name = "objects quantity"
        }
        .WithMinGreaterThanOrEqualTo(1)
        .GetNumber();

        var objects = new List<MilitaryObject>(objectsCount);
        Console.WriteLine();

        for (var i = 0; i < objectsCount; i++)
        {
            Console.WriteLine($"Fill in some values for object {i + 1}");
            var objectName = new StringForm
            {
                Name = $"name",
                ErrorMessage = "empty or duplicate value",
                IsValid = name => !string.IsNullOrEmpty(name) && !objects.Any(o => o.Name == name)
            }.GetString();
            var soldiersCount = new NumberForm<int>
            {
                Name = "soldiers quantity"
            }
            .WithMinGreaterThanOrEqualTo(1)
            .GetNumber();
            var time = new NumberForm<double>
            {
                Name = "estimated time to destroy"
            }
            .WithMinGreaterThan(0)
            .GetNumber();
            objects.Add(new(objectName, soldiersCount, time));
            Console.WriteLine();
        }
        Console.WriteLine();

        var maxSoldiersCount = new NumberForm<int>
        {
            Name = "quantity of available soldiers"
        }
        .WithMinGreaterThanOrEqualTo(1)
        .GetNumber();

        return new Problem(objects, maxSoldiersCount);
    }

    private static void SolveProblem(Problem problem)
    {
        Console.WriteLine();
        var geneticSolver = new GeneticSolver();
        SetupGeneticSolver(geneticSolver);

        var solvers = new ISolver[] { new GreedySolver(), geneticSolver, new BruteforceSolver(), new DynamicSolver() };
        var solutions = solvers.Select(s => s.Solve(problem)).ToList();

        for (var i = 0; i < solvers.Length; i++)
        {
            var solver = solvers[i];
            var solution = solutions[i];
            Console.WriteLine($"{solver.AlgorithmName} algorithm:");
            Console.WriteLine($"Total objects count: {solution.MilitaryObjects.Count}; Total time: {solution.TotalTime} hours");
            Console.WriteLine($"Total soldiers count: {solution.TotalSoldiersCount}");
            Console.WriteLine("Objects:");
            var j = 1;
            foreach (var militaryObject in solution.MilitaryObjects)
            {
                Console.WriteLine($"{j}. Name: {militaryObject.Name}; Time: {militaryObject.Time}; Soldiers count: {militaryObject.SoldiersCount}");
                j++;
            }
            Console.WriteLine();
        }
    }

    private static void SetupGeneticSolver(GeneticSolver solver)
    {
        new Dialog
        {
            Question = $"Do you want to change quantity of generations (default: {solver.GenerationsCount})?",
            YAction = () =>
            {
                solver.GenerationsCount = new NumberForm<int>
                {
                    Name = "quantity of generations"
                }
                .WithMinGreaterThanOrEqualTo(1)
                .GetNumber();
            }
        }.Print();

        new Dialog
        {
            Question = $"Do you want to change the initial population size (default: {solver.PopulationSize})?",
            YAction = () =>
            {
                solver.PopulationSize = new NumberForm<int>
                {
                    Name = "population size"
                }
                .WithMinGreaterThanOrEqualTo(2)
                .GetNumber();
            }
        }.Print();
    }
}
