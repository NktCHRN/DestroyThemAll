namespace Solvers.Common;

public interface ISolver
{
    Solution Solve(MilitaryObject[] militaryObjects, int maxSoldiersCount);
}
