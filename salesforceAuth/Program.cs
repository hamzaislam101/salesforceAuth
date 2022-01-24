using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace salesforceAuth
{
    class Program
    {

        public const string TokenEndpoint = @"https://gdigic3-dev-ed.my.salesforce.com/services/oauth2/token";
        
        

        private static string Username { get; set; }
        private static string Password { get; set; }
        private static string ClientId { get; set; }
        private static string ClientSecret { get; set; }
        public static string AuthToken { get; set; }
        public static string InstanceUrl { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing...");

            ClientId = "3MVG9DREgiBqN9WkqrBdBgWVO8_LrL_383svSdrhYlDzfyStamsye0dKgBopyTdQ6oxDxxwW_TS3RL8ZxBb.Q";
            ClientSecret = "58966451532E97BC24BE948FFEC9E7C1B11CD0C6655A81EE4CA7D9C97410D4FA";
            Username = "hamzaislam101@gmail.com";
            Password = "0493ravian";

            Console.WriteLine("Fetching Token...");
            FetchToken();

            Console.WriteLine("Fetching Data...");
            Console.WriteLine(FetchData());
        }

        private static void FetchToken()
        {
            using (HttpClient Client = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                  {"grant_type", "password"},
                  {"client_id", ClientId},
                  {"client_secret", ClientSecret},
                  {"username", Username},
                  {"password", Password}
                });

                HttpResponseMessage message = Client.PostAsync(TokenEndpoint, content).Result;

                string response = message.Content.ReadAsStringAsync().Result;
                Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(response);

                AuthToken = (string)obj["access_token"];
                InstanceUrl = (string)obj["instance_url"];
            }
        }

        private static string FetchData()
        {
            using (HttpClient client = new HttpClient())
            {
                string restQuery = $"{InstanceUrl}/services/data";

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, restQuery);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.SendAsync(request).Result;

                var x = response.Content.ReadAsStringAsync().Result;
                return x;
            }
        }


        
        



    }
}
