using Common;
using Solvers.Abstractions;

namespace Solvers.Solvers.Dynamic;

public sealed class DynamicSolver : ISolver
{
    public string AlgorithmName => "Dynamic programming";

    public Solution Solve(Problem problem)
    {
        var militaryObjects = problem.MilitaryObjects;
        var maxSoldiersCount = problem.MaxSoldiersCount;

        var t = new double[militaryObjects.Count + 1, maxSoldiersCount + 1][];
        int k;
        for (k = 0; k <= maxSoldiersCount; k++)
        {
            t[0, k] = new[] { 0.0, 0.0 };
        }
        for (var j = 1; j <= militaryObjects.Count; j++)
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
                    if (t[j - 1, k][0] <= newSequence[0])
                    {
                        t[j, k] = newSequence;
                        if (t[j - 1, k][0] == newSequence[0] && t[j - 1, k][1] < newSequence[1])
                        {
                            t[j, k] = t[j - 1, k];1
                                
                        }
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
        for (var j = militaryObjects.Count; j >= 1; j--)
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
