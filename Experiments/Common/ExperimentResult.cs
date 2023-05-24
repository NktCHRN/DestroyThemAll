namespace Experiments.Common;
public sealed class ExperimentResult
{
    public IEnumerable<double> Values => _values;
    private readonly ICollection<double> _values = new List<double>();

    public IReadOnlyCollection<SolverOnTest> Solvers { get; }
    public ExperimentType ExperimentType { get; }
    public string ExperimentVariableName { get; }

    public ExperimentResult(IReadOnlyCollection<SolverOnTest> solvers, ExperimentType experimentType, string experimentVariableName)
    {
        Solvers = solvers;
        ExperimentType = experimentType;
        ExperimentVariableName = experimentVariableName;
    }

    internal void AddValue(double value) => _values.Add(value);
}
