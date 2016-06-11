using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Common.Configuration;

namespace Common
{
    public static class Authentication
    {
        /// <summary>
        /// Login with username and password.
        /// </summary>
        /// <returns>True on successful login and false otherwise</returns>
        public static async System.Threading.Tasks.Task<bool> Login(string username, string password)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("login", username);
            queryParams.Add("password", password);

            HttpResponseMessage response = ApiClient.Post("sessions.json", null, queryParams);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject responseObj = JObject.Parse(responseContent);
                if (responseObj["logged_in"].Value<bool>())
                {
                    GlobalConfigs.UserToken = responseObj.SelectToken("$.user.user_token").Value<string>();
                    return true;
                }
            }

            return false;
        }

        public static bool LoginFacebook()
        {
            return true;
        }
    }
}
