using Demo.Printing.Common;
using Demo.Printing.Setupers;
using Demo.UI;
using Experiments.Abstractions;
using Experiments.Common;
using Experiments.Runners.ObjectsCount;
using Plotly.NET.CSharp;
using ProblemGenerators;
using Solvers.Abstractions;
using Solvers.Solvers.Bruteforce;
using Solvers.Solvers.Dynamic;
using Solvers.Solvers.Genetic;
using Solvers.Solvers.Greedy;

namespace Demo.Printing;
public sealed class ExperimentsPrinter : IPrinter
{
    public void Print()
    {
        HelperMethods.PrintHeader(Constants.Header);
        Console.WriteLine("What experiment do you want to carry out?");
        IExperimentRunner experimentRunner = null!;
        ProblemGenerator problemGenerator = null!;
        new LiteMenu
        {
            IsQuitable = true,
            Name = "option",
            Items = new LiteMenuItem[]
            {
                new LiteMenuItem
                {
                    Text = "ObjectsCount / Time",
                    Action = () =>
                    {
                        Console.WriteLine();
                        problemGenerator = new ProblemGenerator();
                        ProblemGeneratorSetuper.SetupProblemGenerator(problemGenerator);

                        Console.WriteLine();
                        var geneticSolver = new GeneticSolver();
                        GeneticSolverSetuper.SetupGeneticSolver(geneticSolver);

                        Console.WriteLine();
                        var solvers = new ISolver[] { new GreedySolver(), geneticSolver, new BruteforceSolver(), new DynamicSolver() };
                        var specificRunner = new ObjectsCountTimeExperimentsRunner(solvers);
                        experimentRunner = specificRunner;

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects count loop parameters? (default: (" +
                                $"start: {specificRunner.LoopParameters.Start}; step: {specificRunner.LoopParameters.Step}; start: {specificRunner.LoopParameters.End}))",
                            YAction = () => specificRunner.LoopParameters = PrintingHelperMethods.GetLoopParameters(0)
                        }.Print();
                    }
                },
            },
            Callback = () =>
            {
                var results = experimentRunner.Run(problemGenerator);
                ShowResult(results);
                HelperMethods.Quit();
            }
        }.Print();
    }

    private static void ShowResult(ExperimentResult result)
    {
        var charts = new List<Plotly.NET.GenericChart.GenericChart>();
        foreach (var solver in result.Solvers)
        {
            charts.Add(Chart.Line<double, double, string>(
                x: result.Values,
                y: solver.Results
            ).WithTraceInfo($"{solver.Solver.AlgorithmName} Algorithm"));
        }

        Chart.Combine(charts)
            .WithYAxisStyle<double, double, string>(Title: Plotly.NET.Title.init(result.ExperimentType is ExperimentType.Time ? "Time (sec)" : "Deviation"))
            .WithXAxisStyle<double, double, string>(Title: Plotly.NET.Title.init(result.ExperimentVariableName))
            .Show();
    }
}
