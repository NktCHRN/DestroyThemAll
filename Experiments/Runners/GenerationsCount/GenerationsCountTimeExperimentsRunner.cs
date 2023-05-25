using Common;
using Experiments.Abstractions;
using Experiments.Common;
using ProblemGenerators;
using Solvers.Solvers.Genetic;
using System.Diagnostics;

namespace Experiments.Runners.GenerationsCount;
public sealed class GenerationsCountTimeExperimentsRunner : IExperimentRunner
{
    private readonly Stopwatch _stopwatch = new();

    private readonly GeneticSolver _geneticSolver;

    public LoopParameters<int> LoopParameters { get; set; } = new(20, 20, 100);
    public int ObjectsCount { get; set; } = 10;

    public GenerationsCountTimeExperimentsRunner(GeneticSolver geneticSolver)
    {
        _geneticSolver = geneticSolver;
    }

    public ExperimentResult Run(ProblemGenerator problemGenerator)
    {
        var geneticSolverOntest = new SolverOnTest(_geneticSolver);
        var solversOnTest = new SolverOnTest[] { geneticSolverOntest };
        var experimentResult = new ExperimentResult(solversOnTest, ExperimentType.Time, "Generations count");

        for (var i = LoopParameters.Start; i <= LoopParameters.End; i += LoopParameters.Step)
        {
            experimentResult.AddValue(i);
            _geneticSolver.GenerationsCount = i;
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
