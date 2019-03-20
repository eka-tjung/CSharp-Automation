using NUnit.Framework;
using System;
using RestSharp;

namespace RestApiAutomation
{
    [TestFixture]
    class ApiTests
    {
        public string _baseUrl = "https://jsonplaceholder.typicode.com/";

        [Test]
        public void GetResource()
        {
            string url = String.Format("{0}todos/1", _baseUrl);
            RestClient client = new RestClient();
            RestRequest request = new RestRequest(url, Method.GET);
            request.AddHeader("Accept", "application/json");

            Data queryResult = client.Execute<Data>(request).Data;

            Assert.That(queryResult.id.Equals(1));
            Assert.That(queryResult.title.Equals("delectus aut autem"));

            Console.WriteLine(queryResult.title);
        }

        [Test]
        public void GetResource2()
        {
            Data myData = new Data();
            myData.id = 43;
            myData.userId = 2;
            myData.title = "abc";
            myData.completed = true;

            string url = String.Format("{0}/posts/1", _baseUrl);
            RestClient client = new RestClient();
            RestRequest request = new RestRequest(url, Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Accept", "application/json");
            request.AddBody(myData);

            Data queryResult = client.Execute<Data>(request).Data;

            ////Assert.That(queryResult.id.Equals(43));

            Console.WriteLine(queryResult.id);
        }

    }
}
