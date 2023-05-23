using Solvers.Common;

namespace Solvers.Solvers.Bruteforce;

public sealed class BruteforceSolver : ISolver
{
    public Solution Solve(MilitaryObject[] militaryObjects, int maxSoldiersCount)
    {
        var currentSolution = new Solution();
        var resultSolution = new Solution();

        SolveInternal(0);

        return resultSolution;

        void SolveInternal(int startJ)
        {
            for (var j = startJ; j < militaryObjects.Length; j++)
            {
                var currentObject = militaryObjects[j];
                if (currentObject.SoldiersCount + currentSolution.TotalSoldiersCount <= maxSoldiersCount)
                {
                    currentSolution.AddLastMilitaryObject(currentObject);
                    if (currentSolution.TotalMilitaryObjectsCount > resultSolution.TotalMilitaryObjectsCount
                            || (currentSolution.TotalMilitaryObjectsCount == resultSolution.TotalMilitaryObjectsCount
                                && currentSolution.TotalTime < resultSolution.TotalTime))
                    {
                        resultSolution = (Solution)currentSolution.Clone();
                    }

                    SolveInternal(j + 1);

                    currentSolution.RemoveLastMilitaryObject();
                }
            }
        }
    }
}
