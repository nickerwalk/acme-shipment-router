#if DEBUG
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcmeShipmentRouter
{
    public class RandomDataGenerator
    {
        private const string apiAddress = "https://random-data-api.com/api/address/random_address";
        private const string apiName = "https://random-data-api.com/api/name/random_name";
        public List<string> Names { get; set; } = new List<string>();
        public List<string> Addresses { get; set; } = new List<string>();

        public async Task GenerateData(int amount = 20)
        {
            using var httpClient = new HttpClient();
            for (int i = 0; i < amount; i++)
            {
                var addressTask = httpClient.GetAsync(apiAddress);
                var nameTask = httpClient.GetAsync(apiName);

                using (var response = await addressTask)
                {
                    using var content = response.Content;
                    //get the json result from your api
                    var result = await content.ReadAsStringAsync();
                    var root = JsonConvert.DeserializeObject<RandomAddressData>(result);
                    Addresses.Add(root.street_address);
                }

                using (var response = await nameTask)
                {
                    using var content = response.Content;
                    //get the json result from your api
                    var result = await content.ReadAsStringAsync();
                    var root = JsonConvert.DeserializeObject<RandomNameData>(result);
                    Names.Add(root.name);
                }
            }
        }

        public class RandomAddressData
        {
            public string street_address { get; set; }
        }
        public class RandomNameData
        {
            public string name { get; set; }
        }
    }
}
#endif