using Common;
using Experiments.Abstractions;
using Experiments.Common;
using ProblemGenerators;
using Solvers.Abstractions;
using Solvers.Solvers.Genetic;

namespace Experiments.Runners.GenerationsCount;
public sealed class GenerationsCountQualityExperimentsRunner : IExperimentRunner
{
    private readonly GeneticSolver _geneticSolver;
    private readonly ISolver _idealSolver;

    public LoopParameters<int> LoopParameters { get; set; } = new(20, 20, 100);
    public int ObjectsCount { get; set; } = 10;

    public GenerationsCountQualityExperimentsRunner(GeneticSolver geneticSolver, ISolver idealSolver)
    {
        _geneticSolver = geneticSolver;
        _idealSolver = idealSolver;
    }

    public ExperimentResult Run(ProblemGenerator problemGenerator)
    {
        var geneticSolverOntest = new SolverOnTest(_geneticSolver);
        var solversOnTest = new SolverOnTest[] { geneticSolverOntest };
        var experimentResult = new ExperimentResult(solversOnTest, ExperimentType.Quality, "Generations count");

        for (var i = LoopParameters.Start; i <= LoopParameters.End; i += LoopParameters.Step)
        {
            experimentResult.AddValue(i);
            _geneticSolver.GenerationsCount = i;
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
