using Common.Model;
using Newtonsoft.Json;

namespace Common.Configuration
{
    public static class GlobalConfigs
    {
        const string currentUserConfigName = "currentUserSetting";

        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        static User currentUser = null;

        public static event System.EventHandler<bool> LoggedInUserExists;

        public static User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    object val = localSettings.Values[currentUserConfigName];
                    if (val != null)
                        currentUser = JsonConvert.DeserializeObject<User>((string)val);
                }

                return currentUser;
            }

            set
            {
                if (value == null)
                {
                    localSettings.Values.Remove(currentUserConfigName);
                    LoggedInUserExists(typeof(GlobalConfigs), false);
                }
                else
                {
                    string serialisedObj = JsonConvert.SerializeObject(value);
                    localSettings.Values[currentUserConfigName] = serialisedObj;
                    LoggedInUserExists(typeof(GlobalConfigs), true);
                }
            }
        }
    }
}
