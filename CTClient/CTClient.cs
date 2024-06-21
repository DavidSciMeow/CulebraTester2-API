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
    /// <summary>
    /// 一个代管客户端
    /// </summary>
    public partial class CTClient : IDisposable
    {
        /// <summary>
        /// 标准的请求URI
        /// </summary>
        private string Url { get; }
        /// <summary>
        /// 交互用的HTTPClient
        /// </summary>
        private HttpClient Cli { get; } = new HttpClient();
        /// <summary>
        /// 是否在退出时关闭服务器(默认不关闭)
        /// </summary>
        private bool ShutServerWhenQuit { get; } = false;
        /// <summary>
        /// 取消默认的实例化
        /// </summary>
        private CTClient() { }
        /// <summary>
        /// 实例化一个代管客户端
        /// </summary>
        /// <param name="url">客户端的端点 (默认是localhost(本机))</param>
        /// <param name="port">客户端的端口 (默认是9987)</param>
        /// <param name="_shutServerWhenQuit">是否在退出时关闭服务器 (默认不关闭)</param>
        public CTClient(string url = "http://localhost", int port = 9987, bool _shutServerWhenQuit = false)
        {
            Url = $"{url}:{port}";
            ShutServerWhenQuit = _shutServerWhenQuit;
        }

        /// <summary>
        /// 语言模式
        /// </summary>
        public (string Country, string Language, string Varient) LangSet { get => D_Locale().GetAwaiter().GetResult(); }
        /// <summary>
        /// 最大等待时间(可设置)
        /// </summary>
        public int IdleTimeout { get => Conf_GetWaitForIdleTimeout().GetAwaiter().GetResult(); set => Conf_SetWaitForIdleTimeout(value).GetAwaiter().GetResult(); }
        /// <summary>
        /// 版本号
        /// </summary>
        public long VersionCode { get => Info().GetAwaiter().GetResult().versionCode; }
        /// <summary>
        /// 版本名称
        /// </summary>
        public string VersionName { get => Info().GetAwaiter().GetResult().versionName; }
        /// <summary>
        /// 屏幕是否锁定
        /// </summary>
        public bool ScreenRotationFreeze { get => UD_FreezeRotation().GetAwaiter().GetResult(); set => _ = value ? UD_FreezeRotation().GetAwaiter().GetResult() : UD_UnfreezeRotation().GetAwaiter().GetResult(); }
        /// <summary>
        /// 屏幕是否自然方向
        /// </summary>
        public bool ScreenNatualOrientation { get => UD_IsNaturalOrientation().GetAwaiter().GetResult(); }
        /// <summary>
        /// 屏幕是否打开
        /// </summary>
        public bool ScreenOn { get => UD_IsScreenOn().GetAwaiter().GetResult(); }
        /// <summary>
        /// 显示高度
        /// </summary>
        public long DisplayHeight { get => UD_DisplayHeight().GetAwaiter().GetResult(); }
        /// <summary>
        /// 显示宽度
        /// </summary>
        public long DisplayWidth { get => UD_DisplayWidth().GetAwaiter().GetResult(); }
        /// <summary>
        /// 旋转角度
        /// </summary>
        public int DisplayRotation { get => UD_DisplayRotation().GetAwaiter().GetResult(); }
        /// <summary>
        /// 显示密度宽度
        /// </summary>
        public long DisplayDPX { get => UD_DisplaySizeDp().GetAwaiter().GetResult().DpX; }
        /// <summary>
        /// 显示密度高度
        /// </summary>
        public long DisplayDPY { get => UD_DisplaySizeDp().GetAwaiter().GetResult().DpY; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get => UD_ProductName().GetAwaiter().GetResult(); }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (ShutServerWhenQuit)
            {
                _ = Quit().GetAwaiter().GetResult(); // Quit Server
            }
            ((IDisposable)Cli).Dispose();
        }

        /// <summary>
        /// 内部方法,用于发送请求并获取返回的JSON字符串
        /// </summary>
        /// <param name="endpoint">请求的端点</param>
        /// <param name="queryParams">请求填加的命令组</param>
        /// <returns>请求结束的串</returns>
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
        /// <summary>
        /// 内部方法,用于发送请求并获取返回的字节数组
        /// </summary>
        /// <param name="endpoint">请求的端点</param>
        /// <param name="queryParams">请求填加的命令组</param>
        /// <returns>请求结束的串</returns>
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
        /// <summary>
        /// 内部方法,用于发送请求并获取返回的JObject
        /// </summary>
        /// <param name="endpoint">请求的端点</param>
        /// <param name="queryParams">请求填加的命令组</param>
        /// <returns>请求结束的JObject</returns>
        private async Task<JObject> CUrlGetJObject(string endpoint, params (string key, string value)[] queryParams) => JObject.Parse(await CUrl(endpoint, queryParams)??"{}");
        /// <summary>
        /// 内部方法,用于发送请求并获取返回的JArray
        /// </summary>
        /// <param name="endpoint">请求的端点</param>
        /// <param name="queryParams">请求填加的命令组</param>
        /// <returns>请求结束的JArray</returns>
        private async Task<JArray> CUrlGetJArray(string endpoint, params (string key, string value)[] queryParams) => JArray.Parse(await CUrl(endpoint, queryParams) ?? "[]");
        /// <summary>
        /// 内部方法,用于发送请求并获取返回的指定类型的对象
        /// </summary>
        /// <typeparam name="T">可Parse的对象类</typeparam>
        /// <param name="endpoint">请求的端点</param>
        /// <param name="Key">要Parse的返回键</param>
        /// <param name="queryParams">请求填加的命令组</param>
        /// <returns>请求结束的JArray</returns>
        private async Task<T> CUrlGetJsonParsed<T>(string endpoint, string Key, params (string key, string value)[] queryParams) => (await CUrlGetJObject(endpoint, queryParams)).TryGetValue(Key, out JToken value) ? value.ToObject<T>() : default;

    }
}
