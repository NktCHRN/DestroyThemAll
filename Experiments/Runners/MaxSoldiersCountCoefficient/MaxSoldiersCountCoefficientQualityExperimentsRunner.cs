using Common;
using Experiments.Abstractions;
using Experiments.Common;
using ProblemGenerators;
using Solvers.Abstractions;

namespace Experiments.Runners.MaxSoldiersCountCoefficient;
public sealed class MaxSoldiersCountCoefficientQualityExperimentsRunner : IExperimentRunner
{
    private readonly IEnumerable<ISolver> _solvers;
    private readonly ISolver _idealSolver;

    public LoopParameters<double> LoopParameters { get; set; } = new(0.25, 0.25, 0.75);
    public int ObjectsCount { get; set; } = 10;

    public MaxSoldiersCountCoefficientQualityExperimentsRunner(IEnumerable<ISolver> solvers, ISolver idealSolver)
    {
        _solvers = solvers;
        _idealSolver = idealSolver;
    }

    public ExperimentResult Run(ProblemGenerator problemGenerator)
    {
        var solversOnTest = _solvers.Select(s => new SolverOnTest(s)).ToList();
        var experimentResult = new ExperimentResult(solversOnTest, ExperimentType.Quality, "Max soldiers count coeff.");

        for (var i = LoopParameters.Start; i <= LoopParameters.End; i += LoopParameters.Step)
        {
            problemGenerator.MaxSoldiersCountCoefficient = i;
            experimentResult.AddValue(i);
            for (var j = 0; j < Constants.BatchSize; j++)
            {
                var problem = problemGenerator.Generate(ObjectsCount);
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
