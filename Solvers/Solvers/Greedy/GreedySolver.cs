using Solvers.Common;

namespace Solvers.Solvers.Greedy;

public sealed class GreedySolver : ISolver
{
    public Solution Solve(MilitaryObject[] militaryObjects, int maxSoldiersCount)
    {
        var x = new Solution();
        var sortedObjects = 
            SortArray((MilitaryObject[])militaryObjects.Clone(), 0, militaryObjects.Length - 1);
        for (int j = 0; j < sortedObjects.Length; j++)
        {
            if (maxSoldiersCount-x.TotalSoldiersCount >= sortedObjects[j].SoldiersCount)
            {
                x.AddMilitaryObject(sortedObjects[j]);
            }
        }
        return x;
    }
    
    private MilitaryObject[] SortArray(MilitaryObject[] array, int leftIndex, int rightIndex)
    {
        var i = leftIndex;
        var j = rightIndex;
        var pivot = array[leftIndex].SoldiersCount/array[leftIndex].Time;
        while (i <= j)
        {
            while (array[i].SoldiersCount/array[i].Time < pivot)
            {
                i++;
            }
            
            while (array[j].SoldiersCount/array[j].Time > pivot)
            {
                j--;
            }
            if (i <= j)
            {
                (array[i], array[j]) = (array[j], array[i]);
                i++;
                j--;
            }
        }
        
        if (leftIndex < j)
            SortArray(array, leftIndex, j);
        if (i < rightIndex)
            SortArray(array, i, rightIndex);
        return array;
    }
}
