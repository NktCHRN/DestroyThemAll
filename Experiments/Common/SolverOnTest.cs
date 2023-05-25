using Solvers.Abstractions;

namespace Experiments.Common;
public sealed class SolverOnTest
{
    private readonly ICollection<double> _currentBatch = new List<double>(_batchSize);
    private readonly ICollection<double> _currentSecondaryBatch = new List<double>(_batchSize);
    const int _batchSize = Constants.BatchSize;
    private readonly List<double> _results = new();
    private readonly List<double> _secondaryResults = new();

    public ISolver Solver { get; }
    public IReadOnlyCollection<double> Results => _results;
    public IReadOnlyCollection<double> SecondaryResults => _secondaryResults;

    public SolverOnTest(ISolver solver) 
    {
        Solver = solver;
    }

    internal void AddToBatch(double value)
    {
        _currentBatch.Add(value);
        if (_currentBatch.Count == _batchSize)
        {
            _results.Add(_currentBatch.Sum(b => b) / _batchSize);
            _currentBatch.Clear();
        }
    }

    internal void AddToSecondaryBatch(double value)
    {
        _currentSecondaryBatch.Add(value);
        if (_currentSecondaryBatch.Count == _batchSize)
        {
            _secondaryResults.Add(_currentSecondaryBatch.Sum(b => b) / _batchSize);
            _currentSecondaryBatch.Clear();
        }
    }
}
