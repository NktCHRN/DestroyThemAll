namespace Solvers.Common;

public sealed class Solution : ICloneable
{
    private Stack<MilitaryObject> _militaryObjects = new();

    public IEnumerable<MilitaryObject> MilitaryObjects => _militaryObjects;
    public int TotalMilitaryObjectsCount => _militaryObjects.Count;

    public int TotalSoldiersCount { get; private set; }
    public double TotalTime { get; private set; }

    public void AddMilitaryObject(MilitaryObject militaryObject)
    {
        _militaryObjects.Push(militaryObject);
        TotalSoldiersCount += militaryObject.SoldiersCount;
        TotalTime += militaryObject.Time;
    }

    public void PopMilitaryObject()
    {
        var militaryObject = _militaryObjects.Pop();
        TotalSoldiersCount -= militaryObject.SoldiersCount;
        TotalTime -= militaryObject.Time;
    }

    public object Clone()
    {
        var solutionCopy = new Solution
        {
            TotalSoldiersCount = TotalSoldiersCount,
            TotalTime = TotalTime,
            _militaryObjects = new Stack<MilitaryObject>(_militaryObjects)
        };
        return solutionCopy;
    }
}
