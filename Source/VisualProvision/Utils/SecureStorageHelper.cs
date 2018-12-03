using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace VisualProvision.Utils
{
    public static class SecureStorageHelper
    {
        public static async Task<T> GetObjectAsync<T>(string key)
        {
            string serialized = await SecureStorage.GetAsync(key);
            T result = default(T);

            try
            {
                JsonSerializerSettings serializeSettings = GetSerializerSettings();
                result = JsonConvert.DeserializeObject<T>(serialized);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return result;
        }

        public static async Task SetObjectAsync<T>(string key, T obj)
        {
            try
            {
                JsonSerializerSettings serializeSettings = GetSerializerSettings();
                string serialized = JsonConvert.SerializeObject(obj, serializeSettings);
                await SecureStorage.SetAsync(key, serialized);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None,
            };
        }
    }
}
