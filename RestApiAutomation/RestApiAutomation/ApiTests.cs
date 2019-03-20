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

            Console.WriteLine(queryResult.ToString());
        }

        [Test]
        public void PostResource()
        {
            Data toPost = new Data();
            toPost.id = 43;
            toPost.userId = 34;
            toPost.title = "Test";
            toPost.completed = true;

            string url = String.Format("{0}posts", _baseUrl);
            RestClient client = new RestClient();
            RestRequest request = new RestRequest(url, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Accept", "application/json");
            request.AddBody(toPost);
/*            request.AddBody(new {
                id = 43,
                userId = 34,
                title = "Test",
                completed = true
            });
*/
            Data queryResult = client.Execute<Data>(request).Data;

            Assert.AreEqual(101, queryResult.id);
            Assert.AreEqual(34, queryResult.userId);
            Assert.AreEqual("Test", queryResult.title);
            Assert.AreEqual(true, queryResult.completed);
        }

    }
}
