using Experiments.Common;
using ProblemGenerators;

namespace Experiments.Abstractions;
public interface IExperimentRunner
{
    ExperimentResult Run(ProblemGenerator problemGenerator);
}
