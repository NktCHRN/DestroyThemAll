using System.Collections;
using Solvers.Common;
using System.Collections.Generic;

namespace Solvers.Solvers.Genetic;

public sealed class GeneticSolver : ISolver
{

    private MilitaryObject[] _militaryObjects = null!;
    private int _maxSoldiersCount;
    
    public int GenerationsCount {get;set;}
    public int PopulationSize {get;set;}
    public double CrossoverRate { get; set; }
    public double MutationRate { get; set; }

    private List<int[]> Population;
    private Solution _resultSolution = null!;

    public Solution Solve(MilitaryObject[] militaryObjects, int maxSoldiersCount)
    {
        
        _militaryObjects = militaryObjects;
        _maxSoldiersCount = maxSoldiersCount;

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
        
        List<int[]> population = CreatePopulation();
        int[] BestIndividual = null;
        double totalTime = 0; 

        _resultSolution = new();
        for (int i = 0; i < GenerationsCount; i++)
        {
            List<int[]> filteredPopulation = FilterPopulation(population);
            (int[] parent1, int[] parent2) = ChooseParents(filteredPopulation);
            (int[] child1, int[] child2) = SinglePointCrossover(parent1, parent2);
            child1 = Mutation(child1, MutationRate);
            child2 = Mutation(child2, MutationRate);
            filteredPopulation.Add(child1);
            filteredPopulation.Add(child2);

            (BestIndividual, totalTime) = Fitness(filteredPopulation);
            
        }
        
        for (int i = 0; i < militaryObjects.Length; i++)
        {
            if (BestIndividual[i] == 1)
            {
                _resultSolution.AddLastMilitaryObject(militaryObjects[i]);
            }
        }

        return _resultSolution;
        
        
    }

    public List<int[]> CreatePopulation()
    {
        Random random = new Random();

        Population = new();
        
        for (int i = 0; i < PopulationSize; i++)
        {

            int[] chromosome = new int[_militaryObjects.Length];
            for (int j = 0; j < _militaryObjects.Length; j++)
            {
                chromosome[j] = random.Next(2); 
            }

            Population.Add(chromosome);
        }

        return Population;
    }

    public List<int[]> FilterPopulation(List<int[]> Population)
    {
        List<int[]> FilteredPopulation = new List<int[]>();

        int numObjects = _militaryObjects.Length; 
        
        foreach (int[] individual in Population)
        {
            int totalAmountOfPeople = Enumerable.Range(0, numObjects)
                .Sum(i => _militaryObjects[i].SoldiersCount * individual[i]);

            if (totalAmountOfPeople <= 16 && totalAmountOfPeople != 0)
            {
                FilteredPopulation.Add(individual);
            }
        }

        return FilteredPopulation;
    }

    public int HelpFitness(int[] Chromosome)
    {
        int totalObjects = Chromosome.Sum();

        return totalObjects;
    }

    public double HelpFitnessTime(int[] Chromosome)
    { 
        int numObjects = _militaryObjects.Length; 
        
        double totalTime = Enumerable.Range(0, numObjects)
            .Sum(i => _militaryObjects[i].Time * Chromosome[i]);

        return totalTime;
    }

    public (int[], double) Fitness(List<int[]> population)
    {
        float bestFitness = float.NegativeInfinity;
        double bestTotalTime = double.NegativeInfinity;
        int[] bestIndividual = null;
        int numObjects = _militaryObjects.Length;
        
        foreach (var individual in population)
        {
            float fitness = HelpFitness(individual);

            if (fitness > bestFitness)
            {
                bestFitness = fitness;
                bestIndividual = individual;
                bestTotalTime = HelpFitnessTime(individual);
            }
            else if (fitness == bestFitness)
            {
                double totalTime = HelpFitnessTime(individual);

                if (totalTime < bestTotalTime)
                {
                    bestFitness = fitness;
                    bestTotalTime = totalTime;
                    bestIndividual = individual;
                }
            }
        }

        return (bestIndividual, bestTotalTime);
    }
    
    public (int[], int[]) ChooseParents(List<int[]> population)
    {
        Random random = new Random();
        float bestFitness = 0;
        double totalTime = 0;
        int[] parent2 = new int[] { };
        int[] parent1 = population[random.Next(population.Count)];

        (parent2, totalTime) = Fitness(population);

        return (parent1, parent2);
    }
    
    public (int[], int[]) SinglePointCrossover(int[] parent1, int[] parent2)
    {
        Random random = new Random();
        int s = random.Next(1, parent1.Length - 1);

        int[] child1 = new int[parent1.Length];
        int[] child2 = new int[parent2.Length];

        Array.Copy(parent1, child1, s);
        Array.Copy(parent2, s, child1, s, parent1.Length - s);

        Array.Copy(parent2, child2, s);
        Array.Copy(parent1, s, child2, s, parent2.Length - s);

        return (child1, child2);
    }
    
    public int[] Mutation(int[] chromosome, double mutationRate)
    {
        Random random = new Random();
        int[] mutatedChromosome = (int[])chromosome.Clone();

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
