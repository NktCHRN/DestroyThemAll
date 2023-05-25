namespace Common;
public sealed record SolutionModel(
    IEnumerable<MilitaryObject> MilitaryObjects, 
    int TotalMilitaryObjectsCount, 
    int TotalSoldiersCount, 
    double TotalTime);
