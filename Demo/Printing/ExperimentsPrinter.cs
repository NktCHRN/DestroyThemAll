using Demo.Printing.Common;
using Demo.Printing.Setupers;
using Demo.UI;
using Experiments.Abstractions;
using Experiments.Common;
using Experiments.Runners.MaxSoldiersCountCoefficient;
using Experiments.Runners.ObjectsCount;
using Experiments.Runners.PopulationSize;
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
                                $"start: {specificRunner.LoopParameters.Start}; step: {specificRunner.LoopParameters.Step}; end: {specificRunner.LoopParameters.End}))",
                            YAction = () => specificRunner.LoopParameters = PrintingHelperMethods.GetLoopParameters(0)
                        }.Print();
                    }
                },
                new LiteMenuItem
                {
                    Text = "ObjectsCount / Quality",
                    Action = () =>
                    {
                        Console.WriteLine();
                        problemGenerator = new ProblemGenerator();
                        ProblemGeneratorSetuper.SetupProblemGenerator(problemGenerator);

                        Console.WriteLine();
                        var geneticSolver = new GeneticSolver();
                        GeneticSolverSetuper.SetupGeneticSolver(geneticSolver);

                        Console.WriteLine();
                        var solvers = new ISolver[] { new GreedySolver(), geneticSolver };
                        var specificRunner = new ObjectsCountQualityExperimentsRunner(solvers, new BruteforceSolver());
                        experimentRunner = specificRunner;

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects count loop parameters? (default: (" +
                                $"start: {specificRunner.LoopParameters.Start}; step: {specificRunner.LoopParameters.Step}; end: {specificRunner.LoopParameters.End}))",
                            YAction = () => specificRunner.LoopParameters = PrintingHelperMethods.GetLoopParameters(0)
                        }.Print();
                    }
                },
                new LiteMenuItem
                {
                    Text = "Max soldiers count coefficient / Time",
                    Action = () =>
                    {
                        Console.WriteLine();
                        problemGenerator = new ProblemGenerator();
                        Console.WriteLine("Let's setup a problem generator");
                        ProblemGeneratorSetuper.SetupProblemGeneratorOptionalProperties(problemGenerator);

                        Console.WriteLine();
                        var geneticSolver = new GeneticSolver();
                        GeneticSolverSetuper.SetupGeneticSolver(geneticSolver);

                        Console.WriteLine();
                        var solvers = new ISolver[] { new GreedySolver(), geneticSolver, new BruteforceSolver(), new DynamicSolver() };
                        var specificRunner = new MaxSoldiersCountCoefficientTimeExperimentsRunner(solvers);
                        experimentRunner = specificRunner;

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects quantity? (default: {specificRunner.ObjectsCount})",
                            YAction = () => specificRunner.ObjectsCount = PrintingHelperMethods.GetObjectsCount()
                        }.Print();

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects count loop parameters? (default: (" +
                                $"start: {specificRunner.LoopParameters.Start}; step: {specificRunner.LoopParameters.Step}; end: {specificRunner.LoopParameters.End}))",
                            YAction = () => specificRunner.LoopParameters = PrintingHelperMethods.GetLoopParameters<double>(0, 1)
                        }.Print();
                    }
                },
                new LiteMenuItem
                {
                    Text = "Max soldiers count coefficient / Quality",
                    Action = () =>
                    {
                        Console.WriteLine();
                        problemGenerator = new ProblemGenerator();
                        Console.WriteLine("Let's setup a problem generator");
                        ProblemGeneratorSetuper.SetupProblemGeneratorOptionalProperties(problemGenerator);

                        Console.WriteLine();
                        var geneticSolver = new GeneticSolver();
                        GeneticSolverSetuper.SetupGeneticSolver(geneticSolver);

                        Console.WriteLine();
                        var solvers = new ISolver[] { new GreedySolver(), geneticSolver };
                        var specificRunner = new MaxSoldiersCountCoefficientQualityExperimentsRunner(solvers, new BruteforceSolver());
                        experimentRunner = specificRunner;

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects quantity? (default: {specificRunner.ObjectsCount})",
                            YAction = () => specificRunner.ObjectsCount = PrintingHelperMethods.GetObjectsCount()
                        }.Print();

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects count loop parameters? (default: (" +
                                $"start: {specificRunner.LoopParameters.Start}; step: {specificRunner.LoopParameters.Step}; end: {specificRunner.LoopParameters.End}))",
                            YAction = () => specificRunner.LoopParameters = PrintingHelperMethods.GetLoopParameters<double>(0, 1)
                        }.Print();
                    }
                },
                new LiteMenuItem
                {
                    Text = "Population size / Time",
                    Action = () =>
                    {
                        Console.WriteLine();
                        problemGenerator = new ProblemGenerator();
                        ProblemGeneratorSetuper.SetupProblemGenerator(problemGenerator);

                        Console.WriteLine();
                        var geneticSolver = new GeneticSolver();
                        Console.WriteLine("Let's setup a genetic solver");
                        GeneticSolverSetuper.SetupGeneticSolverGenerationsCount(geneticSolver);

                        Console.WriteLine();
                        var specificRunner = new PopulationSizeTimeExperimentsRunner(geneticSolver);
                        experimentRunner = specificRunner;

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects quantity? (default: {specificRunner.ObjectsCount})",
                            YAction = () => specificRunner.ObjectsCount = PrintingHelperMethods.GetObjectsCount()
                        }.Print();

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects count loop parameters? (default: (" +
                                $"start: {specificRunner.LoopParameters.Start}; step: {specificRunner.LoopParameters.Step}; end: {specificRunner.LoopParameters.End}))",
                            YAction = () => specificRunner.LoopParameters = PrintingHelperMethods.GetLoopParameters(2)
                        }.Print();
                    }
                },
                new LiteMenuItem
                {
                    Text = "Population size / Quality",
                    Action = () =>
                    {
                        Console.WriteLine();
                        problemGenerator = new ProblemGenerator();
                        ProblemGeneratorSetuper.SetupProblemGenerator(problemGenerator);

                        Console.WriteLine();
                        var geneticSolver = new GeneticSolver();
                        Console.WriteLine("Let's setup a genetic solver");
                        GeneticSolverSetuper.SetupGeneticSolverGenerationsCount(geneticSolver);

                        Console.WriteLine();
                        var specificRunner = new PopulationSizeQualityExperimentsRunner(geneticSolver, new BruteforceSolver());
                        experimentRunner = specificRunner;

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects quantity? (default: {specificRunner.ObjectsCount})",
                            YAction = () => specificRunner.ObjectsCount = PrintingHelperMethods.GetObjectsCount()
                        }.Print();

                        Console.WriteLine();
                        new Dialog
                        {
                            Question = $"Do you want to change objects count loop parameters? (default: (" +
                                $"start: {specificRunner.LoopParameters.Start}; step: {specificRunner.LoopParameters.Step}; end: {specificRunner.LoopParameters.End}))",
                            YAction = () => specificRunner.LoopParameters = PrintingHelperMethods.GetLoopParameters(2)
                        }.Print();
                    }
                },
            },
            Callback = () =>
            {
                Console.WriteLine();
                HelperMethods.Wait();
                var results = experimentRunner.Run(problemGenerator);
                if (results.ExperimentType is ExperimentType.Time)
                {
                    ShowTimeExperimentResult(results);
                }
                else
                {
                    ShowQualityExperimentResult(results);
                }
                HelperMethods.Quit();
            }
        }.Print();
    }

    private static void ShowTimeExperimentResult(ExperimentResult result)
    {
        var charts = new List<Plotly.NET.GenericChart.GenericChart>();
        foreach (var solver in result.Solvers)
        {
            charts.Add(Chart.Line<double, double, string>(
                x: result.Values,
                y: solver.Results
            ).WithTraceInfo($"{solver.Solver.AlgorithmName}"));
        }

        Chart.Combine(charts)
            .WithYAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("Time (sec)"))
            .WithXAxisStyle<double, double, string>(Title: Plotly.NET.Title.init(result.ExperimentVariableName))
            .WithSize(Width: 800)
            .Show();
    }

    private static void ShowQualityExperimentResult(ExperimentResult result)
    {
        var primaryCharts = new List<Plotly.NET.GenericChart.GenericChart>();
        var secondaryCharts = new List<Plotly.NET.GenericChart.GenericChart>();
        foreach (var solver in result.Solvers)
        {
            primaryCharts.Add(Chart.Line<double, double, string>(
                x: result.Values,
                y: solver.Results
            ).WithTraceInfo($"{solver.Solver.AlgorithmName} (Objects count)"));
            secondaryCharts.Add(Chart.Line<double, double, string>(
                x: result.Values,
                y: solver.SecondaryResults
            ).WithTraceInfo($"{solver.Solver.AlgorithmName} (Total time)"));
        }

        Chart.Grid(new Plotly.NET.GenericChart.GenericChart[]
        {
            Chart.Combine(primaryCharts)
                .WithYAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("Deviation (Destroyed objects count)"))
                .WithXAxisStyle<double, double, string>(Title: Plotly.NET.Title.init(result.ExperimentVariableName)),
            Chart.Combine(secondaryCharts)
                .WithYAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("Deviation (Total time)"))
                .WithXAxisStyle<double, double, string>(Title: Plotly.NET.Title.init(result.ExperimentVariableName))
        }, 1, 2)
            .WithSize(Width: 1200)
            .Show();
    }
}
