namespace Solvers.Common;

public interface ISolver
{
    Solution Solve(IReadOnlyList<MilitaryObject> militaryObjects, int maxSoldiersCount);
}
