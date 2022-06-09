using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeShipmentRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            string addressPath;
            string namePath;
            if (args.Length == 0)
            {
                Console.Write("Filepath for addresses: ");
                addressPath = Console.ReadLine();

                Console.Write("Filepath for names: ");
                namePath = Console.ReadLine();
            }
            else
            {
                addressPath = args[0];
                namePath = args[1];
            }

            ExecuteAlgorithm(addressPath, namePath);
            Console.WriteLine("Done");
            Console.ReadKey(true);
        }

        private static void ExecuteAlgorithm(string addressPath, string namePath)
        {
            var addressTask = File.ReadAllLinesAsync(addressPath);
            var nameTask = File.ReadAllLinesAsync(namePath);
            var taskList = new List<Task> { addressTask, nameTask };
            Task.WhenAll(taskList);

            var addresses = addressTask.Result.ToList();
            var names = nameTask.Result.ToList();

            var algorithm = new RoutingAlgorithm(addresses, names);
            var bestScores = Task.Run(async () => await algorithm.CalculateBestRoute());
            var totalScore = bestScores.Result.Values.Sum();
            Console.WriteLine(totalScore);
        }
    }
}
