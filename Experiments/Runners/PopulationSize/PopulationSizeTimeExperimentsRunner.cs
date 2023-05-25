using Common;
using Experiments.Abstractions;
using Experiments.Common;
using ProblemGenerators;
using Solvers.Solvers.Genetic;
using System.Diagnostics;

namespace Experiments.Runners.PopulationSize;
public sealed class PopulationSizeTimeExperimentsRunner : IExperimentRunner
{
    private readonly Stopwatch _stopwatch = new();

    private readonly GeneticSolver _geneticSolver;

    public LoopParameters<int> LoopParameters { get; set; } = new(25, 25, 150);
    public int ObjectsCount { get; set; } = 10;

    public PopulationSizeTimeExperimentsRunner(GeneticSolver geneticSolver)
    {
        _geneticSolver = geneticSolver;
    }

    public ExperimentResult Run(ProblemGenerator problemGenerator)
    {
        var geneticSolverOntest = new SolverOnTest(_geneticSolver);
        var solversOnTest = new SolverOnTest[] { geneticSolverOntest };
        var experimentResult = new ExperimentResult(solversOnTest, ExperimentType.Time, "Population size");

        for (var i = LoopParameters.Start; i <= LoopParameters.End; i += LoopParameters.Step)
        {
            experimentResult.AddValue(i);
            _geneticSolver.PopulationSize = i;
            for (var j = 0; j < Constants.BatchSize; j++)
            {
                var problem = problemGenerator.Generate(ObjectsCount);
                _stopwatch.Restart();
                geneticSolverOntest.Solver.Solve(problem);
                _stopwatch.Stop();
                geneticSolverOntest.AddToBatch(_stopwatch.Elapsed.TotalSeconds);
            }
        }

        return experimentResult;
    }
}
