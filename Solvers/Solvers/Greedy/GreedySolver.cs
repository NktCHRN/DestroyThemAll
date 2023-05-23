using Solvers.Common;

namespace Solvers.Solvers.Greedy;

public sealed class GreedySolver : ISolver
{
    public Solution Solve(IReadOnlyList<MilitaryObject> militaryObjects, int maxSoldiersCount)
    {
        var x = new Solution();
        var sortedObjects = 
            militaryObjects.OrderBy(obj => obj.SoldiersCount / obj.Time);
        foreach (var obj in sortedObjects)
        {
            if (maxSoldiersCount-x.TotalSoldiersCount >= obj.SoldiersCount)
            {
                x.AddLastMilitaryObject(obj);
            }
        }
        return x;
    }
}
