using Experiments.Abstractions;
using Experiments.Common;
using ProblemGenerators;
using Solvers.Abstractions;
using System.Diagnostics;
using Common;

namespace Experiments.Runners.ObjectsCount;
public sealed class ObjectsCountTimeExperimentsRunner : IExperimentRunner
{
    private readonly Stopwatch _stopwatch = new();

    private readonly IEnumerable<ISolver> _solvers;

    public LoopParameters<int> LoopParameters { get; set; } = new(5, 5, 25);

    public ObjectsCountTimeExperimentsRunner(IEnumerable<ISolver> solvers)
    {
        _solvers = solvers;
    }

    public ExperimentResult Run(ProblemGenerator problemGenerator)
    {
        var solversOnTest = _solvers.Select(s => new SolverOnTest(s)).ToList();
        var experimentResult = new ExperimentResult(solversOnTest, ExperimentType.Time, "Objects count");

        for (var i = LoopParameters.Start; i <= LoopParameters.End; i += LoopParameters.Step)
        {
            experimentResult.AddValue(i);
            for (var j = 0; j < Constants.BatchSize; j++)
            {
                var problem = problemGenerator.Generate(i);
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
