using System;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace MVC4ServicesBook.SmokeTests
{
    [TestFixture]
    public class ApiTests
    {
        public const string ApiUrlRoot = "http://localhost:11000/api";

        [Test]
        public void GetAllCategories()
        {
            var client = CreateWebClient();
            var response = client.DownloadString(ApiUrlRoot + "/categories");

            Console.Write(response);
        }

        [Test]
        public void GetAllUsers()
        {
            var client = CreateWebClient();
            var response = client.DownloadString(ApiUrlRoot + "/users");

            Console.Write(response);
        }

        [Test]
        public void AddNewCategory()
        {
            var client = CreateWebClient();

            const string url = ApiUrlRoot + "/categories";
            const string method = "POST";
            const string newCategory =
                "{\"Name\":\"Project Red\",\"Description\":\"Tasks that belong to project Red\"}";

            client.Headers.Add("Content-Type", "application/json");
            var response = client.UploadString(url, method, newCategory);

            Console.Write(response);
        }

        private WebClient CreateWebClient()
        {
            var webClient = new WebClient();

            const string creds = "jbob" + ":" + "jbob12345";
            var bcreds = Encoding.ASCII.GetBytes(creds);
            var base64Creds = Convert.ToBase64String(bcreds);
            webClient.Headers.Add("Authorization", "Basic " + base64Creds); // amJvYjpqYm9iMTIzNDU=
            return webClient;
        }
    }
}
