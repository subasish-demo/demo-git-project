using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.PLMWorx.WebServices
{
    public class ServerInfo
    {
        /// <summary>
        /// CA01A2709 for QA and DEV
        /// CA01A2710 for PROD
        /// </summary>
        private static string server { get; set; }

        /// <summary>
        /// 8085 for all environments
        /// </summary>
        private static int port { get; set; }
        public static string GetBaseAddress
        {
            get
            {
                return $"http://{server}:{port}";
            }
        }

        /// <summary>
        /// Set the server name or ip and port for use by all other web service methods
        /// </summary>
        /// <param name="server">Server name or ip address</param>
        /// <param name="port">Communication port</param>
        public static void SetServerInfo(string server, int port)
        {
            ServerInfo.server = server;
            ServerInfo.port = port;
        }

        /// <summary>
        /// Checks if the currently configured server information returns expected values
        /// </summary>
        /// <returns>True if successful</returns>
        public static bool ValidateServerInfo()
        {

            if (server == null)
                return false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://{server}:{port}");
                var request = new HttpRequestMessage(HttpMethod.Get, "/atsplmworxapi");
                var sendTask = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var response = sendTask.Result;
            }

            return true;
        }

        /// <summary>
        /// Used by file services and item services to communicate with PLMWorx server
        /// </summary>
        /// <param name="method">HTTP Method type</param>
        /// <param name="requestURI">Relative URI excluding server and port information</param>
        /// <returns>Return message from web service</returns>
        public static string SubmitRequest(HttpMethod method, string requestURI)
        {
            if (ServerInfo.ValidateServerInfo())
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GetBaseAddress);
                    var request = new HttpRequestMessage(method, requestURI);
                    var sendTask = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                    var val = sendTask.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return val;
                }
            }
            else
            {
                throw new Exception("Invalid server information");
            }
        }

        /// <summary>
        /// Alternate method for communicating with server if HttpClient fails
        /// </summary>
        /// <param name="requestURI">Relative URI excluding server and port information</param>
        /// <returns>Return message from web service</returns>
        public static string SubmitRequestAlt(string requestURI)
        {
            string address = GetBaseAddress + requestURI;
            var myURI = new Uri(address);
            var myReq = WebRequest.Create(myURI);
            myReq.Method = "POST";
            myReq.ContentType = "application/json";

            try
            {
                using (var response = myReq.GetResponse())
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                return "0";
            }

        }
    }
}
