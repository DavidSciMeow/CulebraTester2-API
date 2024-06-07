using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using CulebraTesterAPI.BasicStruct;

namespace CulebraTesterAPI
{
    public partial class CTClient : IDisposable
    {
        private string Url { get; }
        private HttpClient Cli { get; } = new HttpClient();
        private bool ShutServerWhenQuit { get; } = false;
        private CTClient() { }
        public CTClient(string url = "http://localhost", int port = 9987, bool _shutServerWhenQuit = false)
        {
            Url = $"{url}:{port}";
            ShutServerWhenQuit = _shutServerWhenQuit;
        }

        public (string Country, string Language, string Varient) LangSet { get => D_Locale().GetAwaiter().GetResult(); }
        public int IdleTimeout { get => Conf_GetWaitForIdleTimeout().GetAwaiter().GetResult(); set => Conf_SetWaitForIdleTimeout(value).GetAwaiter().GetResult(); }
        public long VersionCode { get => Info().GetAwaiter().GetResult().versionCode; }
        public string VersionName { get => Info().GetAwaiter().GetResult().versionName; }
        public bool ScreenRotationFreeze { get => UD_FreezeRotation().GetAwaiter().GetResult(); set => _ = value ? UD_FreezeRotation().GetAwaiter().GetResult() : UD_UnfreezeRotation().GetAwaiter().GetResult(); }
        public bool ScreenNatualOrientation { get => UD_IsNaturalOrientation().GetAwaiter().GetResult(); }
        public bool ScreenOn { get => UD_IsScreenOn().GetAwaiter().GetResult(); }
        public long DisplayHeight { get => UD_DisplayHeight().GetAwaiter().GetResult(); }
        public long DisplayWidth { get => UD_DisplayWidth().GetAwaiter().GetResult(); }
        public int DisplayRotation { get => UD_DisplayRotation().GetAwaiter().GetResult(); }
        public long DisplayDPX { get => UD_DisplaySizeDp().GetAwaiter().GetResult().DpX; }
        public long DisplayDPY { get => UD_DisplaySizeDp().GetAwaiter().GetResult().DpY; }
        public string ProductName { get => UD_ProductName().GetAwaiter().GetResult(); }


        public void Dispose()
        {
            if (ShutServerWhenQuit)
            {
                _ = Quit().GetAwaiter().GetResult(); // Quit Server
            }
            ((IDisposable)Cli).Dispose();
        }

        private async Task<string> CUrl(string endpoint, params (string key, string value)[] queryParams)
        {
            var builder = new UriBuilder($"{Url}/v2/{endpoint}");
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var (key, value) in queryParams)
            {
                if (value != null)
                {
                    query[key] = value;
                }
            }
            builder.Query = query.ToString();
            var maxRetries = 4;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    var response = await Cli.GetAsync(builder.ToString());
                    Debug.WriteLine($"CUrlGetJson __ Request : {response.StatusCode}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ret = await response.Content.ReadAsStringAsync();
                        return ret;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return "{}";
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CUrlGetJson __ Request failed: {ex.Message}");
                    if (i == maxRetries - 1) throw;
                }
                await Task.Delay(1000);
            }
            return null;
        }
        private async Task<byte[]> CUrlBytes(string endpoint, params (string key, string value)[] queryParams)
        {
            var builder = new UriBuilder($"{Url}/v2/{endpoint}");
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var (key, value) in queryParams)
            {
                if (value != null)
                {
                    query[key] = value;
                }
            }
            builder.Query = query.ToString();
            var maxRetries = 4;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return await Cli.GetByteArrayAsync(builder.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CUrlGetJson __ Request failed: {ex.Message}");
                    if (i == maxRetries - 1) throw; // rethrow the last exception
                }
                await Task.Delay(1000); // wait for a second before retrying
            }
            return null; // return null if all retries failed
        }
        private async Task<JObject> CUrlGetJObject(string endpoint, params (string key, string value)[] queryParams) => JObject.Parse(await CUrl(endpoint, queryParams)??"{}");
        private async Task<JArray> CUrlGetJArray(string endpoint, params (string key, string value)[] queryParams) => JArray.Parse(await CUrl(endpoint, queryParams) ?? "[]");
        private async Task<T> CUrlGetJsonParsed<T>(string endpoint, string Key, params (string key, string value)[] queryParams) => (await CUrlGetJObject(endpoint, queryParams)).TryGetValue(Key, out JToken value) ? value.ToObject<T>() : default;

    }
}
