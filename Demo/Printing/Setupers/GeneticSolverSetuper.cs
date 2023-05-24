using Demo.UI;
using Solvers.Solvers.Genetic;

namespace Demo.Printing.Setupers;
internal static class GeneticSolverSetuper
{
    public static void SetupGeneticSolver(GeneticSolver solver)
    {
        Console.WriteLine("Let's setup a genetic solver");
        SetupGeneticSolverGenerationsCount(solver);
        SetupGeneticSolverPopulationSize(solver);
    }

    public static void SetupGeneticSolverGenerationsCount(GeneticSolver solver)
    {
        new Dialog
        {
            Question = $"Do you want to change quantity of generations (default: {solver.GenerationsCount})?",
            YAction = () =>
            {
                solver.GenerationsCount = new NumberForm<int>
                {
                    Name = "quantity of generations"
                }
                .WithMinGreaterThanOrEqualTo(1)
                .GetNumber();
            }
        }.Print();
    }

    public static void SetupGeneticSolverPopulationSize(GeneticSolver solver)
    {
        new Dialog
        {
            Question = $"Do you want to change the initial population size (default: {solver.PopulationSize})?",
            YAction = () =>
            {
                solver.PopulationSize = new NumberForm<int>
                {
                    Name = "population size"
                }
                .WithMinGreaterThanOrEqualTo(2)
                .GetNumber();
            }
        }.Print();
    }
}
