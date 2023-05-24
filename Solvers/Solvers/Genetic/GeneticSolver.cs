using Solvers.Common;

namespace Solvers.Solvers.Genetic;

public sealed class GeneticSolver : ISolver
{
    private IReadOnlyList<MilitaryObject> _militaryObjects = null!;
    private int _maxSoldiersCount;

    private List<int[]> _population = null!;
    private Solution _resultSolution = null!;

    public int GenerationsCount {get;set;}
    public int PopulationSize {get;set;}
    public double CrossoverRate { get; set; }
    public double MutationRate { get; set; }

    public Solution Solve(IReadOnlyList<MilitaryObject> militaryObjects, int maxSoldiersCount)
    {
        _militaryObjects = militaryObjects;
        _maxSoldiersCount = maxSoldiersCount;

        SetupDefaultProperties();

        var population = CreatePopulation();
        int[] bestIndividual = null!;
        var filteredPopulation = FilterPopulation(population);

        _resultSolution = new();
        for (int i = 0; i < GenerationsCount; i++)
        {
            var (parent1, parent2) = ChooseParents(filteredPopulation);
            var (child1, child2) = SinglePointCrossover(parent1, parent2);

            child1 = Mutation(child1, MutationRate);
            child2 = Mutation(child2, MutationRate);

            TryAddMutatedChild(child1, filteredPopulation);
            TryAddMutatedChild(child2, filteredPopulation);

            bestIndividual = Fitness(filteredPopulation);
        }
        
        for (int i = 0; i < militaryObjects.Count; i++)
        {
            if (bestIndividual[i] == 1)
            {
                _resultSolution.AddLastMilitaryObject(militaryObjects[i]);
            }
        }

        return _resultSolution;
    }

    private void TryAddMutatedChild(int[] mutatedChild, List<int[]> population)
    {
        int total = CheckIfCorrect(mutatedChild);
        if (total <= _maxSoldiersCount && total != 0)
        {
            population.Add(mutatedChild);
        }
    }

    private int CheckIfCorrect(int[] mutatedChild)
    {
        int totalAmountOfPeople = 0;
            
        foreach (int gene in mutatedChild)
        {
            totalAmountOfPeople = Enumerable.Range(0, _militaryObjects.Count)
                .Sum(i => _militaryObjects[i].SoldiersCount * gene);
        }

        return totalAmountOfPeople; 
    }

    private void SetupDefaultProperties()
    {
        if (GenerationsCount == 0)
        {
            GenerationsCount = 50;
        }

        if (PopulationSize == 0)
        {
            PopulationSize = 100;
        }

        if (CrossoverRate == 0)
        {
            CrossoverRate = 0.8;
        }

        if (MutationRate == 0)
        {
            MutationRate = 0.1;
        }
    }

    private List<int[]> CreatePopulation()
    {
        var random = new Random();

        _population = new(PopulationSize);
        
        for (int i = 0; i < PopulationSize; i++)
        {

            var chromosome = new int[_militaryObjects.Count];
            for (int j = 0; j < _militaryObjects.Count; j++)
            {
                chromosome[j] = random.Next(2); 
            }

            _population.Add(chromosome);
        }

        return _population;
    }

    private List<int[]> FilterPopulation(List<int[]> population)
    {
        var filteredPopulation = new List<int[]>();

        foreach (int[] individual in population)
        {
            int totalAmountOfPeople = Enumerable.Range(0, _militaryObjects.Count)
                .Sum(i => _militaryObjects[i].SoldiersCount * individual[i]);

            if (totalAmountOfPeople <= _maxSoldiersCount && totalAmountOfPeople != 0)
            {
                filteredPopulation.Add(individual);
            }
        }
        
        if (filteredPopulation.Count == 0)
        {
            CreatePopulation(); 
            filteredPopulation = FilterPopulation(_population);
        }

        return filteredPopulation; 
    }

    private static int HelpFitness(int[] chromosome) => chromosome.Sum();

    private double HelpFitnessTime(int[] chromosome)
    { 
        double totalTime = Enumerable.Range(0, _militaryObjects.Count)
            .Sum(i => _militaryObjects[i].Time * chromosome[i]);

        return totalTime;
    }

    private int[] Fitness(List<int[]> population)
    {
        var bestFitness = double.NegativeInfinity;
        var bestTotalTime = double.NegativeInfinity;
        int[] bestIndividual = null!;
        
        foreach (var individual in population)
        {
            var fitness = HelpFitness(individual);

            if (fitness > bestFitness)
            {
                bestFitness = fitness;
                bestIndividual = individual;
                bestTotalTime = HelpFitnessTime(individual);
            }
            else if (fitness == bestFitness)
            {
                var totalTime = HelpFitnessTime(individual);

                if (totalTime < bestTotalTime)
                {
                    bestFitness = fitness;
                    bestTotalTime = totalTime;
                    bestIndividual = individual;
                }
            }
        }

        return bestIndividual;
    }

    private (int[], int[]) ChooseParents(List<int[]> population)
    {
        var random = new Random();
        var parent1 = population[random.Next(population.Count)];

        var parent2 = Fitness(population);
        
        

        return (parent1, parent2);
    }

    private static (int[], int[]) SinglePointCrossover(int[] parent1, int[] parent2)
    {
        var random = new Random();
        var s = random.Next(1, parent1.Length - 1);

        var child1 = new int[parent1.Length];
        var child2 = new int[parent2.Length];

        Array.Copy(parent1, child1, s);
        Array.Copy(parent2, s, child1, s, parent1.Length - s);

        Array.Copy(parent2, child2, s);
        Array.Copy(parent1, s, child2, s, parent2.Length - s);

        return (child1, child2);
    }

    private static int[] Mutation(int[] chromosome, double mutationRate)
    {
        var random = new Random();
        var mutatedChromosome = (int[])chromosome.Clone();

        for (int i = 0; i < chromosome.Length; i++)
        {
            if (random.NextDouble() < mutationRate)
            {
                mutatedChromosome[i] = 1 - mutatedChromosome[i];
            }
        }

        return mutatedChromosome;
    }
}