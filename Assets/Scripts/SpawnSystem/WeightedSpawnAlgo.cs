using System.Collections.Generic;
using Random = System.Random;

public class WeightedSpawnAlgo
{
    private Random _rnd = new Random();
    private List<int> _alias = new List<int>();
    private List<double> _probabilities = new List<double>();
    
    public WeightedSpawnAlgo(List<double> spawnChances)
    {
        for (int i = 0; i < spawnChances.Count; i++)
        {
            _probabilities.Add(0);
            _alias.Add(0);
        }
        
        GenerateDistribution(spawnChances);
    }
    
    private void GenerateDistribution(List<double> passedProbabilities)
    {
        double avg = 1d / passedProbabilities.Count;

        Stack<int> small = new Stack<int>();
        Stack<int> large = new Stack<int>();

        for (int i = 0; i < passedProbabilities.Count; i++)
        {
            if (passedProbabilities[i] >= avg)
            {
                large.Push(i);
            }
            else
            {
                small.Push(i);
            }
        }

        while (small.Count != 0 && large.Count != 0)
        {
            int less = small.Pop();
            int more = large.Pop();

            _probabilities[less] = passedProbabilities[less] * passedProbabilities.Count;
            _alias[less] = more;

            passedProbabilities[more] = (passedProbabilities[more] + passedProbabilities[less]) - avg;

            if (passedProbabilities[more] >= 1.0 / passedProbabilities.Count)
            {
                large.Push(more);
            }
            else
            {
                small.Push(more);
            }
        }

        while (small.Count != 0)
        {
            _probabilities[small.Pop()] = 1.0;
        }

        while (large.Count != 0)
        {
            _probabilities[large.Pop()] = 1.0;
        }
    }

    public int PickValue()
    {
        int col = _rnd.Next(_probabilities.Count);
        
        bool coinToss = _rnd.NextDouble() < _probabilities[col];
        
        return coinToss ? col : _alias[col];
    }
}
