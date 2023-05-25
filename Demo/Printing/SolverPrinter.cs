using Common;
using Demo.Printing.Setupers;
using Demo.UI;
using Newtonsoft.Json;
using ProblemGenerators;
using Solvers.Abstractions;
using Solvers.Solvers.Bruteforce;
using Solvers.Solvers.Dynamic;
using Solvers.Solvers.Genetic;
using Solvers.Solvers.Greedy;
using static Demo.Printing.Common.PrintingHelperMethods;

namespace Demo.Printing;
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
                    Action = () => problem = EnterProblemManually()
                },
                new LiteMenuItem
                {
                    Text = "Generate the problem",
                    Action = () => problem = GenerateProblem()
                },
                new LiteMenuItem
                {
                    Text = "Read the problem from file",
                    Action = () => problem = ReadProblemFromFile()
                },
            },
            Callback = () =>
            {
                SolveProblem(problem);
                HelperMethods.Quit();
            }
        }.Print();
    }

    private static Problem EnterProblemManually()
    {
        Console.WriteLine();
        var objectsCount = GetObjectsCount();

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

    private static Problem GenerateProblem()
    {
        var generator = new ProblemGenerator();
        ProblemGeneratorSetuper.SetupProblemGenerator(generator);

        Console.WriteLine();

        var objectsCount = GetObjectsCount();
        var problem = generator.Generate(objectsCount);
        PrintProblem(problem);

        return problem;
    }

    private static Problem ReadProblemFromFile()
    {
        Console.WriteLine();
        var fileName = new StringForm
        {
            Name = "json file name",
            IsValid = f => File.Exists(f),
            ErrorMessage = "File does not exist"
        }.GetString();

        var fileData = File.ReadAllText(fileName);
        var problem = JsonConvert.DeserializeObject<Problem>(fileData) ?? throw new InvalidOperationException("JSON object is null");

        Console.WriteLine();
        PrintProblem(problem);

        return problem;
    }

    private static void SolveProblem(Problem problem)
    {
        Console.WriteLine();
        var geneticSolver = new GeneticSolver();
        GeneticSolverSetuper.SetupGeneticSolver(geneticSolver);

        Console.WriteLine();
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

        new Dialog
        {
            Question = "Do you want to write problem and solutions to files as Json?",
            YAction = () =>
            {
                Console.WriteLine();

                var serializedProblem = JsonConvert.SerializeObject(problem);
                var serializedSolutions = solutions.Select(s => JsonConvert.SerializeObject((SolutionModel)s)).ToList();

                const string folderName = "Output";
                Directory.CreateDirectory(folderName);
                var problemFileName = Path.Combine(folderName, "problem.json");
                File.WriteAllText(problemFileName, serializedProblem);
                Console.WriteLine($"The problem was written to {problemFileName}");
                Console.Write("Solutions were written to files:");
                for (var i = 0; i < solvers.Length; i++)
                {
                    var fileName = Path.Combine(folderName, $"{solvers[i].AlgorithmName.Replace(" ", "-").ToLower()}-solution.json");
                    File.WriteAllText(fileName, serializedSolutions[i]);
                    Console.Write($" {fileName}");
                }
                Console.WriteLine(Environment.NewLine);
            }
        }.Print();
    }
}
