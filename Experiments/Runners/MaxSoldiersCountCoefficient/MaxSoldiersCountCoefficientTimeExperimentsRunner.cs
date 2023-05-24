using Common;
using Experiments.Abstractions;
using Experiments.Common;
using ProblemGenerators;
using Solvers.Abstractions;
using System.Diagnostics;

namespace Experiments.Runners.MaxSoldiersCountCoefficient;
public sealed class MaxSoldiersCountCoefficientTimeExperimentsRunner : IExperimentRunner
{
    private readonly Stopwatch _stopwatch = new();

    private readonly IEnumerable<ISolver> _solvers;

    public LoopParameters<double> LoopParameters { get; set; } = new(0.25, 0.25, 0.75);
    public int ObjectsCount { get; set; } = 10;

    public MaxSoldiersCountCoefficientTimeExperimentsRunner(IEnumerable<ISolver> solvers)
    {
        _solvers = solvers;
    }

    public ExperimentResult Run(ProblemGenerator problemGenerator)
    {
        var solversOnTest = _solvers.Select(s => new SolverOnTest(s)).ToList();
        var experimentResult = new ExperimentResult(solversOnTest, ExperimentType.Time, "Max soldiers count coeff.");

        for (var i = LoopParameters.Start; i <= LoopParameters.End; i += LoopParameters.Step)
        {
            problemGenerator.MaxSoldiersCountCoefficient = i;
            experimentResult.AddValue(i);
            for (var j = 0; j < Constants.BatchSize; j++)
            {
                var problem = problemGenerator.Generate(ObjectsCount);
                foreach (var solver in solversOnTest)
                {
                    _stopwatch.Restart();
                    solver.Solver.Solve(problem);
                    _stopwatch.Stop();
                    solver.AddToBatch(_stopwatch.Elapsed.TotalSeconds);
                }
            }
        }

        return experimentResult;
    }
}
