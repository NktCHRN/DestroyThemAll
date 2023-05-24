using ProblemGenerators;

namespace Experiments.Abstractions;
public interface IExperimentRunner
{
    void Run(ProblemGenerator problemGenerator);
}
