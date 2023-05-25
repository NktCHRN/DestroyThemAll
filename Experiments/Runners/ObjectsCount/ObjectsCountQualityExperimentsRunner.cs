using Common;
using Experiments.Abstractions;
using Experiments.Common;
using ProblemGenerators;
using Solvers.Abstractions;

namespace Experiments.Runners.ObjectsCount;
public sealed class ObjectsCountQualityExperimentsRunner : IExperimentRunner
{
    private readonly IEnumerable<ISolver> _solvers;
    private readonly ISolver _idealSolver;

    public LoopParameters<int> LoopParameters { get; set; } = new(5, 5, 25);

    public ObjectsCountQualityExperimentsRunner(IEnumerable<ISolver> solvers, ISolver idealSolver)
    {
        _solvers = solvers;
        _idealSolver = idealSolver;
    }

    public ExperimentResult Run(ProblemGenerator problemGenerator)
    {
        var solversOnTest = _solvers.Select(s => new SolverOnTest(s)).ToList();
        var experimentResult = new ExperimentResult(solversOnTest, ExperimentType.Quality, "Objects count");

        for (var i = LoopParameters.Start; i <= LoopParameters.End; i += LoopParameters.Step)
        {
            experimentResult.AddValue(i);
            for (var j = 0; j < Constants.BatchSize; j++)
            {
                var problem = problemGenerator.Generate(i);
                var idealSolution = _idealSolver.Solve(problem);
                foreach (var solver in solversOnTest)
                {
                    var solution = solver.Solver.Solve(problem);
                    solver.AddToBatch(idealSolution.TotalMilitaryObjectsCount - solution.TotalMilitaryObjectsCount);
                    solver.AddToSecondaryBatch(idealSolution.TotalTime - solution.TotalTime);
                }
            }
        }

        return experimentResult;
    }
}
