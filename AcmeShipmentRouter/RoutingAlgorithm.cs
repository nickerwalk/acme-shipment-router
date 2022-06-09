using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AcmeShipmentRouter
{
    public class RoutingAlgorithm
    {
        private readonly List<string> _addresses;
        private readonly List<string> _names;

        public RoutingAlgorithm(List<string> addresses, List<string> names)
        {
            _addresses = addresses;
            _names = names;
        }

        internal async Task<Dictionary<string, double>> CalculateBestRoute()
        {
            var tasks = new List<Task>();
            var addressFullScores = new Dictionary<string, double>();
            foreach (var address in _addresses)
            {
                foreach (var name in _names)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        var score = CalculateSuitabilityScore(address, name);
                        lock (addressFullScores)
                        {
                            addressFullScores.Add($"{address}|{name}", score);
                        }
                    }));
                }
            }

            await Task.WhenAll(tasks);

            return OptimizeBestScores(addressFullScores);
        }

        private Dictionary<string, double> OptimizeBestScores(Dictionary<string, double> suitabilityScores)
        {
            var orderedScores = suitabilityScores.OrderByDescending(_ => _.Value).ToDictionary(_ => _.Key, _ => _.Value);

            var bestScores = new Dictionary<string, double>();

            while (orderedScores.Any())
            { 
                //Taking the top score in an ordered list
                //To improve the results, consider the eliminated options for a potential better overall score
                var topScore = orderedScores.First(); 
                bestScores.Add(topScore.Key, topScore.Value);

                var address = GetAddressFromScore(topScore);
                var name = GetNameFromScore(topScore);
                var scoresToRemove = orderedScores.Where(_ => _.Key.StartsWith(address) || _.Key.Contains(name));
                foreach (var s in scoresToRemove)
                {
                    orderedScores.Remove(s.Key);
                }
            }

            return bestScores;
        }

        public static string GetAddressFromScore(KeyValuePair<string, double> score)
        {
            return score.Key.Substring(0, score.Key.IndexOf('|'));
        }

        public static string GetNameFromScore(KeyValuePair<string, double> score)
        {
            return score.Key.Substring(score.Key.IndexOf('|') + 1, score.Key.Length - score.Key.IndexOf('|') - 1);
        }

        public double CalculateSuitabilityScore(string destination, string name)
        {
            var evenMultiplier = 1.5;
            var oddMultiplier = 1;
            var commonFactorBonus = BigInteger.GreatestCommonDivisor(destination.Length, name.Length) > 1 ? 0.5 : 0;

            double suitabilityScore;

            if (destination.Length % 2 == 0)
            {
                suitabilityScore = GetVowelCount(name) * evenMultiplier;
            }
            else
            {
                suitabilityScore = name.Length - GetVowelCount(name) * oddMultiplier;
            }
            return suitabilityScore + (suitabilityScore * commonFactorBonus);
        }

        static readonly HashSet<char> vowels = new() { 'a', 'e', 'i', 'o', 'u' }; //Y is a vowel?
        public int GetVowelCount(string text)
        {
            return text.ToLower().Count(_ => vowels.Contains(_));
        }
    }
}
