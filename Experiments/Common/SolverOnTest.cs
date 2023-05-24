using Solvers.Abstractions;

namespace Experiments.Common;
public sealed class SolverOnTest
{
    private readonly ICollection<double> _currentBatch = new List<double>(_batchSize);
    const int _batchSize = Constants.BatchSize;
    private readonly List<double> _results = new();

    public ISolver Solver { get; }
    public IReadOnlyCollection<double> Results => _results;

    public SolverOnTest(ISolver solver) 
    {
        Solver = solver;
    }

    internal void AddToBatch(double value)
    {
        _currentBatch.Add(value);
        if (_currentBatch.Count == _batchSize - 1)
        {
            _results.Add(_currentBatch.Sum(b => b) / _batchSize);
            _currentBatch.Clear();
        }
    }
}
