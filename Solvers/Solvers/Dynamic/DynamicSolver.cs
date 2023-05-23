using Solvers.Common;

namespace Solvers.Solvers.Dynamic;

public sealed class DynamicSolver : ISolver
{
    public Solution Solve(MilitaryObject[] militaryObjects, int maxSoldiersCount)
    {
        var t = new double[militaryObjects.Length + 1, maxSoldiersCount + 1][];
        int k;
        for (k = 0; k <= maxSoldiersCount; k++)
        {
            t[0, k] = new[] { 0.0, 0.0 };
        }
        for (var j = 1; j <= militaryObjects.Length; j++)
        {
            for (k = 0; k <= maxSoldiersCount; k++)
            {
                if (k >= militaryObjects[j-1].SoldiersCount)
                {
                    var newSequence = new double[]
                    {
                        t[j - 1, k - militaryObjects[j - 1].SoldiersCount][0] + 1,
                        t[j - 1, k - militaryObjects[j - 1].SoldiersCount][1] + militaryObjects[j - 1].Time
                    };
                    if (t[j - 1, k][0] < newSequence[0])
                    {
                        t[j, k] = newSequence;
                    }
                    else
                    {
                        t[j, k] = t[j - 1, k];
                    }
                }
                else
                {
                    t[j, k] = t[j - 1, k];
                }
            }
        }
        var x = new Solution();
        k = maxSoldiersCount;
        for (var j = militaryObjects.Length; j >= 1; j--)
        {
            if (t[j, k][1] != t[j - 1, k][1])
            {
                x.AddFirstMilitaryObject(militaryObjects[j - 1]);
                k -= militaryObjects[j - 1].SoldiersCount;
            }
        }
        return x;
    }
}
