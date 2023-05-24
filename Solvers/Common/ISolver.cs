using Common;

namespace Solvers.Common;

public interface ISolver
{
    Solution Solve(Problem problem);

    string AlgorithmName { get; }
}
