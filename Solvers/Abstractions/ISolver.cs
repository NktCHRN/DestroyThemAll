using Common;

namespace Solvers.Abstractions;

public interface ISolver
{
    Solution Solve(Problem problem);

    string AlgorithmName { get; }
}
