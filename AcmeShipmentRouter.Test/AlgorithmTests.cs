using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeShipmentRouter.Test
{
    [TestClass]
    public class AlgorithmTests
    {
        RoutingAlgorithm algorithm;

        [TestInitialize]
        [DeploymentItem(@"addresses.list", "TestFiles")]
        [DeploymentItem(@"names.list", "TestFiles")]
        public void Init()
        {
            var addressPath = "TestFiles/addresses.list";
            var namePath = "TestFiles/names.list";
            var addressTask = File.ReadAllLinesAsync(addressPath);
            var nameTask = File.ReadAllLinesAsync(namePath);
            var taskList = new List<Task> { addressTask, nameTask };
            Task.WhenAll(taskList);

            var addresses = addressTask.Result.ToList();
            var names = nameTask.Result.ToList();

            algorithm = new RoutingAlgorithm(addresses, names);
        }

        [TestMethod]
        public void GetVowelCount_Some()
        {
            var expected = 17;
            var actual = algorithm.GetVowelCount("This is a test. All vowels accounted for. a e i o u y");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetVowelCount_None()
        {
           var expected = 0;
           var actual = algorithm.GetVowelCount("BCDFG");
           Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetVowelCount_Empty()
        {
            var expected = 0;
            var actual = algorithm.GetVowelCount("");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetVowelCount_CaseInsensitive()
        {
            var expected = 3;
            var actual = algorithm.GetVowelCount("SpOnGeBoB");
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void CalculateSuitabilityScore()
        {
            var expected = 13.5;
            var actual = algorithm.CalculateSuitabilityScore("123 fake street", "Tommy Tester");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetNameFromScore()
        {
            var expected = "Tommy Tester";
            var actual = RoutingAlgorithm.GetNameFromScore(new KeyValuePair<string, double>("123 fake street|Tommy Tester", 13.5));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAddressFromScore()
        {
            var expected = "123 fake street";
            var actual = RoutingAlgorithm.GetAddressFromScore(new KeyValuePair<string, double>("123 fake street|Tommy Tester", 13.5));
            Assert.AreEqual(expected, actual);
        }
    }
}
