using Solvers.Common;

namespace Solvers.Solvers.Bruteforce;

public sealed class BruteforceSolver : ISolver
{
    private Solution _currentSolution = null!;
    private Solution _resultSolution = null!;

    private MilitaryObject[] _militaryObjects = null!;
    private int _maxSoldiersCount;

    public Solution Solve(MilitaryObject[] militaryObjects, int maxSoldiersCount)
    {
        _currentSolution = new();
        _resultSolution = new();
        _militaryObjects = militaryObjects;
        _maxSoldiersCount = maxSoldiersCount;

        SolveInternal(0);

        return _resultSolution;

        void SolveInternal(int startJ)
        {
            for (var j = startJ; j < _militaryObjects.Length; j++)
            {
                var currentObject = _militaryObjects[j];
                if (currentObject.SoldiersCount + _currentSolution.TotalSoldiersCount <= _maxSoldiersCount)
                {
                    _currentSolution.AddMilitaryObject(currentObject);
                    if (_currentSolution.TotalMilitaryObjectsCount > _resultSolution.TotalMilitaryObjectsCount
                            || (_currentSolution.TotalMilitaryObjectsCount == _resultSolution.TotalMilitaryObjectsCount
                                && _currentSolution.TotalTime < _resultSolution.TotalTime))
                    {
                        _resultSolution = (Solution)_currentSolution.Clone();
                    }

                    SolveInternal(j + 1);

                    _currentSolution.PopMilitaryObject();
                }
            }
        }
    }
}
