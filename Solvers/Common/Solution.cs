namespace Solvers.Common;

public sealed class Solution : ICloneable
{
    private LinkedList<MilitaryObject> _militaryObjects = new();

    public IReadOnlyCollection<MilitaryObject> MilitaryObjects => _militaryObjects;
    public int TotalMilitaryObjectsCount => _militaryObjects.Count;

    public int TotalSoldiersCount { get; private set; }
    public double TotalTime { get; private set; }

    public void AddMilitaryObject(MilitaryObject militaryObject)
    {
        _militaryObjects.AddLast(militaryObject);
        TotalSoldiersCount += militaryObject.SoldiersCount;
        TotalTime += militaryObject.Time;
    }

    public void PopMilitaryObject()
    {
        var militaryObject = _militaryObjects.Last!.Value;
        _militaryObjects.RemoveLast();
        TotalSoldiersCount -= militaryObject.SoldiersCount;
        TotalTime -= militaryObject.Time;
    }

    public object Clone()
    {
        var solutionCopy = new Solution
        {
            TotalSoldiersCount = TotalSoldiersCount,
            TotalTime = TotalTime,
            _militaryObjects = new (_militaryObjects)
        };
        return solutionCopy;
    }
}
