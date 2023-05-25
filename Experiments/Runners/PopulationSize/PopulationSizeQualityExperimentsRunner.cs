using Common;
using Experiments.Abstractions;
using Experiments.Common;
using ProblemGenerators;
using Solvers.Abstractions;
using Solvers.Solvers.Genetic;
using System.Diagnostics;

namespace Experiments.Runners.PopulationSize;
public sealed class PopulationSizeQualityExperimentsRunner : IExperimentRunner
{
    private readonly GeneticSolver _geneticSolver;
    private readonly ISolver _idealSolver;

    public LoopParameters<int> LoopParameters { get; set; } = new(25, 25, 150);
    public int ObjectsCount { get; set; } = 10;

    public PopulationSizeQualityExperimentsRunner(GeneticSolver geneticSolver, ISolver idealSolver)
    {
        _geneticSolver = geneticSolver;
        _idealSolver = idealSolver;
    }

    public ExperimentResult Run(ProblemGenerator problemGenerator)
    {
        var geneticSolverOntest = new SolverOnTest(_geneticSolver);
        var solversOnTest = new SolverOnTest[] { geneticSolverOntest };
        var experimentResult = new ExperimentResult(solversOnTest, ExperimentType.Quality, "Population size");

        for (var i = LoopParameters.Start; i <= LoopParameters.End; i += LoopParameters.Step)
        {
            experimentResult.AddValue(i);
            _geneticSolver.PopulationSize = i;
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
