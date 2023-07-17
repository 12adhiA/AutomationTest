using NUnit.Framework;
using RestSharp;
using Newtonsoft.Json;
using NUnit.Framework.Internal;

using static RestAssured.Dsl;
using RestAssured.Request.Logging;

namespace AutomationTest.API
{

    internal class Program
    {
        private const string BaseUrl = "https://petstore.swagger.io/v2/";
        static void Main(string[] args)
        {
            PetStore petStore = new PetStore();
            petStore.CreatePet();
        }
     

        [TestFixture]
        public class PetStore
        {
            [Test]
            public void CreatePet()
            {
                string username = "";
                // Define the JSON payload
                string requestBody = "[\n" +
                                      "  {\n" +
                                      "    \"id\": 43433,\n" +
                                      "    \"username\": \"323243431qw\",\n" +
                                      "    \"firstName\": \"RRRR\",\n" +
                                      "    \"lastName\": \"LLL\",\n" +
                                      "    \"email\": \"we@gmail.com\",\n" +
                                      "    \"password\": \"23dwewe\",\n" +
                                      "    \"phone\": \"2324433\",\n" +
                                      "    \"userStatus\": 0\n" +
                                      "  }\n" +
                                      "]";
                var petList = JsonConvert.DeserializeObject<IList<PetModel>>(requestBody);

                // Send the POST request to create a user with the given JSON payload
                IRestResponse postResponse = PostPet(requestBody);
                if (postResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("User created successfully.");

                    // Send the GET request to retrieve the user information
                    IRestResponse getResponse = GetPetDetailsByUsername(petList[0].username);

                    if (getResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine("User retrieved successfully.");

                        // Deserialize the response content into dynamic objects
                        var user = JsonConvert.DeserializeObject<PetModel>(getResponse.Content);

                        // Assert the retrieved user information
                        Assert.AreEqual(43433, (int)user.id);
                        Assert.AreEqual("323243431qw", user.username);
                        Assert.AreEqual("RRRR", user.firstName);
                        Assert.AreEqual("LLL", user.lastName);
                        Assert.AreEqual("we@gmail.com", user.email);
                        Assert.AreEqual("23dwewe", user.password);
                        Assert.AreEqual("2324433", user.phone);
                        Assert.AreEqual(0, (int)user.userStatus);
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve user information.");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to create user.");
                }
            }

            [Test]
            private static IRestResponse PostPet(string requestBody)
            {
                var client = new RestClient(BaseUrl);
                var request = new RestRequest("user/createWithArray", Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", requestBody, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Assert.AreEqual(200, (int)response.StatusCode);
                return response;
            }

            [Test]
            private static IRestResponse GetPetDetailsByUsername(string username)
            {
                var client = new RestClient(BaseUrl);
                var request = new RestRequest($"user/{username}", Method.GET);
                IRestResponse response = client.Execute(request);
                Assert.AreEqual(200, (int)response.StatusCode);
                return response;
            }
        }

        [Test]
        public void CreatePetStore()
        {
            string username = "";
            // Define the JSON payload
            string requestBody = "[\n" +
                                  "  {\n" +
                                  "    \"id\": 43433,\n" +
                                  "    \"username\": \"323243431qw\",\n" +
                                  "    \"firstName\": \"RRRR\",\n" +
                                  "    \"lastName\": \"LLL\",\n" +
                                  "    \"email\": \"we@gmail.com\",\n" +
                                  "    \"password\": \"23dwewe\",\n" +
                                  "    \"phone\": \"2324433\",\n" +
                                  "    \"userStatus\": 0\n" +
                                  "  }\n" +
                                  "]";
            var petList = JsonConvert.DeserializeObject<IList<PetModel>>(requestBody);
            PetModel model = new PetModel
            {
                id=1,
                username="",
                firstName="",
                lastName="",
                email="",
                password="",
                phone="",
                userStatus=0
            };

            Given()           
                .Body(model)
                .When()
                .Post(BaseUrl)
                .Then()
                .StatusCode(200);
        }

        [Test]
        public void Verify(string username)
        {
            PetModel model =

           (PetModel) Given()              
                .When()
                .Get(BaseUrl + $"user/{username}")
                .Then()
                .StatusCode(200).And().DeserializeTo(typeof(PetModel));

            Assert.That(model.id,Is.EqualTo(1));
        }
    }

    public class PetModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public int userStatus { get; set; }
    }
}