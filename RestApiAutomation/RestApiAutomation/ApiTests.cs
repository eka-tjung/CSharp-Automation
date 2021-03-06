﻿using NUnit.Framework;
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
    }
}
