using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ATS_Supplier_Portal_Test
{
    internal class AnypointPlatform
    {
        private const string anypointHost = "https://anypoint.mulesoft.com";
        private const string environmentID = "0ebf123c-831b-407f-b6d6-3bc7f4799296";
        private const string username = "ranorex";
        private const string password = "9AxKfdfaAtTT7pe";
        private static readonly string[] applications = new[] { "ats-supplier-portal-dev", "ats-jdedwards-api-dev", "jdedwards-inbound-requisition-m4" };
        private static string token;

        internal static async Task RestartApplications()
        {
            await GetToken();
            HttpClient client = GetClientWithDefaultHeaders(environmentID);
            foreach (var domain in applications)
            {
                var body = new StringContent("{\n \"status\":\"restart\"\n}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/cloudhub/api/applications/" + domain + "/status", body);
                var value = await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> GetToken()
        {
            if (token != null)
            {
                return token;
            }
            HttpClient client = GetClient();
            var body = new StringContent("{\"username\": \"" + username + "\", \"password\": \"" + password + "\"}", Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/accounts/login", body);
            var clientJson = await response.Content.ReadAsStringAsync();
            token = (JsonConvert.DeserializeObject<JObject>(clientJson))["access_token"].ToString();
            return token;
        }

        private static HttpClient GetClientWithDefaultHeaders(string environmentID = null, TimeSpan? timeout = null)
        {
            HttpClient client = GetClient(timeout);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (environmentID != null)
            {
                client.DefaultRequestHeaders.Add("X-ANYPNT-ENV-ID", environmentID);
            }
            return client;
        }
        private static HttpClient GetClient(TimeSpan? timeout = null)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(anypointHost);
            if (timeout != null)
            {
                client.Timeout = (TimeSpan)timeout;
            }
            return client;
        }
    }
}