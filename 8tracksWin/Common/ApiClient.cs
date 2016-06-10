using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public static class ApiClient
    {
        static string devApiKey = "1814ff5a6d53aac74fc28827f0602d26ffc5d191";
        static UriBuilder baseApiUri = new UriBuilder("https://8tracks.com/");

        private static HttpRequestMessage CreateRequestObj(string methodPath, HttpMethod method, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams =  null)
        {
            //Always adding the api_version to the url
            if (queryParams != null)
                queryParams.Add("api_version", "3");
            else
                queryParams = new Dictionary<string, string>() { { "api_version", "3" } };

            methodPath += Uri.EscapeDataString(CreateQueryStringFromParameters(queryParams));
            baseApiUri.Path += methodPath;
            HttpRequestMessage request = new HttpRequestMessage(method, baseApiUri.Uri);
            request.Headers.Add("X-Api-Key", devApiKey);
            if (Authentication.UserToken != string.Empty)
                request.Headers.Add("X-User-Token", Authentication.UserToken);

            if(headers != null)
                foreach (string key in headers.Keys)
                    request.Headers.Add(key, headers[key]);

            return request;
        }

        private static string CreateQueryStringFromParameters(Dictionary<string,string> queryParams)
        {
            System.Text.StringBuilder queryString = new System.Text.StringBuilder("?");

            foreach (string key in queryParams.Keys)
                queryString.Append("&" + key + "=" + queryParams[key]);
            queryString.Remove(0, 1);                  //removing the first '&'

            return queryString.ToString();
        }

        public static HttpResponseMessage Post(string methodPath, string data, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
        {
            HttpRequestMessage req = CreateRequestObj(methodPath, HttpMethod.Post, headers, queryParams);
            req.Content = new StringContent(data, System.Text.Encoding.UTF8);

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> response = client.SendAsync(req);
                response.Wait();
                return response.Result;
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(string methodPath, string data, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
        {
            HttpRequestMessage req = CreateRequestObj(methodPath, HttpMethod.Post, headers, queryParams);
            req.Content = new StringContent(data, System.Text.Encoding.UTF8);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(req);
                return response;
            }
        }

        public static HttpResponseMessage Get(string methodPath, Dictionary<string,string> headers = null, Dictionary<string,string> queryParams = null)
        {

            HttpRequestMessage req = CreateRequestObj(methodPath, HttpMethod.Get, headers, queryParams);

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> response = client.SendAsync(req);
                response.Wait();
                return response.Result;
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(string methodPath, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
        {
            HttpRequestMessage req = CreateRequestObj(methodPath, HttpMethod.Post, headers, queryParams);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(req);
                return response;
            }
        }
    }
}
