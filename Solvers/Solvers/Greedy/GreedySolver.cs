using Common;
using Solvers.Common;

namespace Solvers.Solvers.Greedy;

public sealed class GreedySolver : ISolver
{
    public Solution Solve(Problem problem)
    {
        var x = new Solution();
        var sortedObjects = problem.MilitaryObjects.OrderBy(obj => obj.SoldiersCount / obj.Time);
        foreach (var obj in sortedObjects)
        {
            if (problem.MaxSoldiersCount - x.TotalSoldiersCount >= obj.SoldiersCount)
            {
                x.AddLastMilitaryObject(obj);
            }
        }
        return x;
    }
}
