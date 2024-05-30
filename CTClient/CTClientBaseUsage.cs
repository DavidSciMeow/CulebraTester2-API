using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CulebraTesterAPI
{
    public partial class CTClient : IDisposable
    {
        public Task<string> Conf_GetWaitForIdleTimeout() => Cli.GetStringAsync($"{Url}/v2/configurator/getWaitForIdleTimeout");
        public Task<string> Conf_SetWaitForIdleTimeout(long timeout) => Cli.GetStringAsync($"{Url}/v2/configurator/setWaitForIdleTimeout?timeout={timeout}");

        public Task<string> AM_ForceStop(string pkg) => Cli.GetStringAsync($"{Url}/v2/am/forceStop?pkg={pkg}");

        public Task<string> Help(string api) => Cli.GetStringAsync($"{Url}/v2/culebra/help/{api}");
        public Task<string> Info() => Cli.GetStringAsync($"{Url}/v2/culebra/info");
        public Task<string> Quit() => Cli.GetStringAsync($"{Url}/v2/culebra/quit");

        public Task<string> D_DisplayRealSize() => Cli.GetStringAsync($"{Url}/v2/device/displayRealSize");
        public Task<string> D_Dumpsys(string service, string arg1 = null, string arg2 = null, string arg3 = null) => Cli.GetStringAsync($"{Url}/v2/device/dumpsys?service={service}{(string.IsNullOrWhiteSpace(arg1) ? "" : $"&arg1={arg1}")}{(string.IsNullOrWhiteSpace(arg2) ? "" : $"&arg2={arg2}")}{(string.IsNullOrWhiteSpace(arg3) ? "" : $"&arg3={arg3}")}");
        public Task<string> D_Locale() => Cli.GetStringAsync($"{Url}/v2/device/locale");
        public Task<string> D_WaitForNewToast(long timeout) => Cli.GetStringAsync($"{Url}/v2/device/waitForNewToast?timeout={timeout}");

        public Task<string> OS_Clear() => Cli.GetStringAsync($"{Url}/v2/objectStore/clear");
        public Task<string> OS_List() => Cli.GetStringAsync($"{Url}/v2/objectStore/list");
        public Task<string> OS_Remove(int oid) => Cli.GetStringAsync($"{Url}/v2/objectStore/remove?oid={oid}");

        public Task<string> TC_StartActivity(string pkg, string cls, string uri = null) => Cli.GetStringAsync($"{Url}/v2/targetContext/startActivity?pkg={pkg}&cls={cls}{(string.IsNullOrWhiteSpace(uri) ? "" : $"&uri={uri}")}");

        public Task<bool> UD_ClearLastTraversedText() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/clearLastTraversedText"))["status"].ToString()));
        public Task<bool> UD_Click(long x, long y) => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/click?x={x}&y={y}"))["status"].ToString()));
        public Task<string> UD_Click(UINode node) => Cli.GetStringAsync($"{Url}/v2/uiDevice/click?x={node.ClickCenter.X}&y={node.ClickCenter.Y}");
        public Task<string> UD_CurrentPackageName() => Cli.GetStringAsync($"{Url}/v2/uiDevice/currentPackageName");
        public Task<string> UD_DisplayHeight() => Cli.GetStringAsync($"{Url}/v2/uiDevice/displayHeight");
        public Task<string> UD_DisplayRotation() => Cli.GetStringAsync($"{Url}/v2/uiDevice/displayRotation");
        public Task<string> UD_DisplaySizeDp() => Cli.GetStringAsync($"{Url}/v2/uiDevice/displaySizeDp");
        public Task<string> UD_DisplayWidth() => Cli.GetStringAsync($"{Url}/v2/uiDevice/displayWidth");
        public Task<string> UD_Drag(long startX, long startY, long endX, long endY, long steps = 1) => Cli.GetStringAsync($"{Url}/v2/uiDevice/drag?startX={startX}&startY={startY}&endX={endX}&endY={endY}&steps={steps}");
        public Task<string> UD_DumpWindowHierarchy() => Cli.GetStringAsync($"{Url}/v2/uiDevice/dumpWindowHierarchy");

        public Task<HttpResponseMessage> UD_FindObject(FindObjectQueryStruct foqs) => Cli.PostAsync($"{Url}/v2/uiDevice/findObject", new StringContent(JsonConvert.SerializeObject(foqs, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })));
        public Task<HttpResponseMessage> UD_FindObjects(FindObjectQueryStruct foqs) => Cli.PostAsync($"{Url}/v2/uiDevice/findObjects", new StringContent(JsonConvert.SerializeObject(foqs, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })));
        public Task<HttpResponseMessage> UD_HasObject(FindObjectQueryStruct foqs) => Cli.PostAsync($"{Url}/v2/uiDevice/hasObject", new StringContent(JsonConvert.SerializeObject(foqs, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })));

        public Task<string> UD_FreezeRotation() => Cli.GetStringAsync($"{Url}/v2/uiDevice/freezeRotation");
        public Task<bool> UD_IsNaturalOrientation() => Task.Run(async () => JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/isNaturalOrientation"))["value"].ToString() == "true");
        public Task<bool> UD_IsScreenOn() => Task.Run(async () => JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/isScreenOn"))["value"].ToString() == "true");
        public Task<string> UD_LastTraversedText() => Cli.GetStringAsync($"{Url}/v2/uiDevice/lastTraversedText");
        public Task<string> UD_Pixel() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pixel");
        public Task<bool> UD_PressBack() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressBack"))["status"].ToString()));
        public Task<bool> UD_PressDPadCenter() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadCenter"))["status"].ToString()));
        public Task<bool> UD_PressDPadDown() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadDown"))["status"].ToString()));
        public Task<bool> UD_PressDPadLeft() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadLeft"))["status"].ToString()));
        public Task<bool> UD_PressDPadRight() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadRight"))["status"].ToString()));
        public Task<bool> UD_PressDPadUp() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadUp"))["status"].ToString()));
        public Task<bool> UD_PressDelete() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDelete"))["status"].ToString()));
        public Task<bool> UD_PressEnter() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressEnter"))["status"].ToString()));
        public Task<bool> UD_PressHome() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressHome"))["status"].ToString()));
        public Task<string> UD_PressKeyCode(Key key, int? metaState = null) => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressKeyCode?keyCode={(int)key}{(metaState == null ? "" : $"&metaState={metaState}")}");
        public Task<bool> UD_PressRecentApps() => Task.Run(async () => "OK".Equals(JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/pressRecentApps"))["status"].ToString()));
        public Task<string> UD_ProductName() => Task.Run(async () => JObject.Parse(await Cli.GetStringAsync($"{Url}/v2/uiDevice/productName"))["productName"].ToString());
        public Task<byte[]> UD_Screenshot(int scale = 1, int quality = 100) => Cli.GetByteArrayAsync($"{Url}/v2/uiDevice/screenshot?scale={scale}&quality={quality}");
        public Task<string> UD_Swipe(long startX, long startY, long endX, long endY, long steps = 1) => Cli.GetStringAsync($"{Url}/v2/uiDevice/swipe?startX={startX}&startY={startY}&endX={endX}&endY={endY}&steps={steps}");
        public Task<string> UD_UnfreezeRotation() => Cli.GetStringAsync($"{Url}/v2/uiDevice/unfreezeRotation");
        public Task<string> UD_Wait(long oid, int? timeout = null) => Cli.GetStringAsync($"{Url}/v2/uiDevice/wait?oid={oid}{(timeout == null ? "" : $"&timeout={timeout}")}");
        public Task<string> UD_WaitForIdle(int timeout) => Cli.GetStringAsync($"{Url}/v2/uiDevice/waitForIdle?timeout={timeout}");
        public Task<string> UD_WaitForWindowUpdate(string packageName, int? timeout = null) => Cli.GetStringAsync($"{Url}/v2/uiDevice/waitForWindowUpdate?packageName={packageName}{(timeout == null ? "" : $"&timeout={timeout}")}");

        public Task<string> UIO_ClearTextField(long oid) => Cli.GetStringAsync($"{Url}/v2/uiObject/{oid}/clearTextField");
        public Task<string> UIO_Click(long oid) => Cli.GetStringAsync($"{Url}/v2/uiObject/{oid}/click");
        public Task<string> UIO_ClickAndWaitForNewWindow(long oid) => Cli.GetStringAsync($"{Url}/v2/uiObject/{oid}/clickAndWaitForNewWindow");
        public Task<string> UIO_Dump(long oid) => Cli.GetStringAsync($"{Url}/v2/uiObject/{oid}/dump");
        public Task<string> UIO_Exists(long oid) => Cli.GetStringAsync($"{Url}/v2/uiObject/{oid}/exists");
        public Task<string> UIO_GetBounds(long oid) => Cli.GetStringAsync($"{Url}/v2/uiObject/{oid}/getBounds");
        public Task<string> UIO_GetChild(long oid, string uiSelector) => Cli.GetStringAsync($"{Url}/v2/uiObject/{oid}/getChild?uiSelector={uiSelector}");
        public Task<string> UIO_GetChildCount(long oid) => Cli.GetStringAsync($"{Url}/v2/uiObject/{oid}/getChildCount");

        public Task<string> UIO2_Clear(long oid) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/clear");
        public Task<string> UIO2_Click(long oid) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/click");
        public Task<string> UIO2_ClickAndWait(long oid, int eventConditionRef, int? timeout = null) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/clickAndWait?eventConditionRef={eventConditionRef}{(timeout == null ? "" : $"&timeout={timeout}")}");
        public Task<string> UIO2_Dump(long oid) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/dump");
        public Task<string> UIO2_GetChildCount(long oid) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/getChildCount");

        public Task<string> UIO2_SetText(long oid, string text) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/setText?text={text}");
        public Task<string> UIO2_LongClick(long oid) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/longClick");



    }
}
