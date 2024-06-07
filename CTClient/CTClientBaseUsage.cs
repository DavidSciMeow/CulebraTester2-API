using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using CulebraTesterAPI.BasicStruct;
using CulebraTesterAPI.BasicEnums;

namespace CulebraTesterAPI
{
    public partial class CTClient : IDisposable
    {
        #region Private Methods
        private Task<(string country, string lang, string varient)> D_Locale() => Task.Run(async () => { var obj = await CUrlGetJObject("/device/locale"); return (obj["country"].ToString(), obj["language"].ToString(), obj["variant"].ToString()); });
        private Task<int> Conf_GetWaitForIdleTimeout() => Task.Run(async () => await CUrlGetJsonParsed<int>("configurator/getWaitForIdleTimeout", "timeout"));
        private Task<bool> Conf_SetWaitForIdleTimeout(long timeout) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("configurator/setWaitForIdleTimeout", "status", (nameof(timeout), timeout.ToString()))));
        private Task<(long versionCode, string versionName)> Info() => Task.Run(async () => { var obj = await CUrlGetJObject("culebra/info"); return (obj["versionCode"].ToObject<long>(), obj["versionName"].ToString()); });
        private Task<bool> UD_FreezeRotation() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/freezeRotation", "status")));
        private Task<bool> UD_UnfreezeRotation() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/unfreezeRotation", "status")));
        private Task<bool> UD_IsNaturalOrientation() => Task.Run(async () => "true".Equals(await CUrlGetJsonParsed<string>("uiDevice/isNaturalOrientation", "value")));
        private Task<bool> UD_IsScreenOn() => Task.Run(async () => "true".Equals(await CUrlGetJsonParsed<string>("uiDevice/isScreenOn", "value")));
        private Task<long> UD_DisplayHeight() => Task.Run(async () => await CUrlGetJsonParsed<long>("uiDevice/displayHeight", "displayHeight"));
        private Task<int> UD_DisplayRotation() => Task.Run(async () => int.Parse((await CUrlGetJsonParsed<string>("uiDevice/displayRotation", "displayRotation")).Substring(1)));
        private Task<(long DpX, long DpY)> UD_DisplaySizeDp() => Task.Run(async () => { var obj = await CUrlGetJObject("uiDevice/displaySizeDp"); return (obj["displaySizeDpX"].ToObject<long>(), obj["displaySizeDpY"].ToObject<long>()); });
        private Task<long> UD_DisplayWidth() => Task.Run(async () => await CUrlGetJsonParsed<long>("uiDevice/displayWidth", "displayWidth"));
        private Task<string> UD_ProductName() => Task.Run(async () => await CUrlGetJsonParsed<string>("uiDevice/productName", "productName"));
        #endregion

        #region Public Alter Managed Methods
        public Task<string> D_Dumpsys(string service, string arg1 = null, string arg2 = null, string arg3 = null) => CUrl($"device/dumpsys", (nameof(service), service), (nameof(arg1), arg1), (nameof(arg2), arg2), (nameof(arg3), arg3));
        public Task<string> D_WaitForNewToast(long timeout) => CUrl($"device/waitForNewToast", (nameof(timeout), timeout.ToString()));
        public Task<string> UD_DumpWindowHierarchy() => CUrl("uiDevice/dumpWindowHierarchy"); //leave 

        public Task<string> TC_StartActivity(string pkg, string cls, string uri = null) => CUrlGetJsonParsed<string>("targetContext/startActivity", "status", (nameof(pkg), pkg), (nameof(cls), cls), (nameof(uri), uri));
        public Task<byte[]> UD_Screenshot(int scale = 1, int quality = 100) => Task.Run(async () => await CUrlBytes("uiDevice/screenshot", (nameof(scale), scale.ToString()), (nameof(quality), quality.ToString())));

        public Task<string> UD_CurrentPackageName() => Task.Run(async () => await CUrlGetJsonParsed<string>("uiDevice/currentPackageName", "currentPackageName"));

        public Task<bool> OS_Clear() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("objectStore/clear", "status")));
        public Task<List<UIObject>> OS_List() => Task.Run(async () => (await CUrlGetJArray("objectStore/list")).Select(i => new UIObject(i, this)).ToList());
        public Task<bool> OS_Remove(long oid) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("objectStore/remove", "status", (nameof(oid), oid.ToString()))));

        public Task<bool> UD_PressBack() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressBack", "status")));
        public Task<bool> UD_PressDPadCenter() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressDPadCenter", "status")));
        public Task<bool> UD_PressDPadDown() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressDPadDown", "status")));
        public Task<bool> UD_PressDPadLeft() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressDPadLeft", "status")));
        public Task<bool> UD_PressDPadRight() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressDPadRight", "status")));
        public Task<bool> UD_PressDPadUp() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressDPadUp", "status")));
        public Task<bool> UD_PressDelete() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressDelete", "status")));
        public Task<bool> UD_PressEnter() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressEnter", "status")));
        public Task<bool> UD_PressHome() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressHome", "status")));
        public Task<bool> UD_PressRecentApps() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressRecentApps", "status")));
        public Task<bool> UD_ClearLastTraversedText() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/clearLastTraversedText", "status")));
        public Task<bool> UD_Click(long x, long y) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/click", "status", (nameof(x), x.ToString()), (nameof(y), y.ToString()))));
        public Task<bool> UD_Click(UINode node) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/click", "status", ("x", node.ClickCenter.X.ToString()), ("y", node.ClickCenter.Y.ToString()))));
        public Task<bool> UD_Swipe(long startX, long startY, long endX, long endY, long steps = 1) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>($"uiDevice/pressHome", "status", (nameof(startX), startX.ToString()), (nameof(startY), startY.ToString()), (nameof(endX), endX.ToString()), (nameof(endY), endY.ToString()), (nameof(steps), steps.ToString()))));
        public Task<bool> UD_Drag(long startX, long startY, long endX, long endY, long steps = 1) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>($"uiDevice/drag", "status", (nameof(startX), startX.ToString()), (nameof(startY), startY.ToString()), (nameof(endX), endX.ToString()), (nameof(endY), endY.ToString()), (nameof(steps), steps.ToString()))));
        public Task<(int a, int b, int g, int r)> UD_Pixel() => Task.Run(async () => { var obj = await CUrlGetJObject("uiDevice/pixel"); return (obj["a"].ToObject<int>(), obj["b"].ToObject<int>(), obj["g"].ToObject<int>(), obj["r"].ToObject<int>()); });
        public Task<bool> UD_PressKeyCode(Key key, int? metaState = null) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("uiDevice/pressKeyCode", "status", (nameof(key), key.ToString()), (nameof(metaState), metaState.ToString()))));
        public Task<bool> Quit() => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>("culebra/quit", "status")));
        public Task<(string devname, int x, int y)> D_DisplayRealSize() => Task.Run(async () => { var obj = await CUrlGetJObject("device/displayRealSize"); return (obj["device"].ToString(), obj["x"].ToObject<int>(), obj["y"].ToObject<int>()); });

        //use UIO2 //public Task<bool> UIO_ClearTextField(long oid) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>($"uiObject/{oid}/clearTextField", "status")));
        //use UIO2 //public Task<bool> UIO_Click(long oid) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>($"uiObject/{oid}/click", "status")));
        //use UIO2 //public Task<bool> UIO_ClickAndWaitForNewWindow(long oid) => Task.Run(async () => "true".Equals((await CUrlGetJsonParsed<string>($"uiObject/{oid}/clickAndWaitForNewWindow", "value")).ToLowerInvariant()));
        //use UIO2_Dump //public Task<JObject> UIO_Dump(long oid) => Task.Run(async () => await CUrlGetJObject($"uiObject/{oid}/dump"));
        //use UD_hasObject //public Task<bool> UIO_Exists(long oid) => Task.Run(async () => "true".Equals((await CUrlGetJsonParsed<string>($"uiObject/{oid}/exists", "value")).ToLowerInvariant()));
        //use UIO2 //public Task<string> UIO_GetContentDescription(long oid) => Task.Run(async () => await CUrlGetJsonParsed<string>($"uiObject/{oid}/getContentDescription", "value"));
        //use UIO2 //public Task<string> UIO_GetClassName(long oid) => Task.Run(async () => await CUrlGetJsonParsed<string>($"uiObject/{oid}/getClassName", "value"));
        //use UIO2 //public Task<long> UIO_GetChildCount(long oid) => Task.Run(async () => await CUrlGetJsonParsed<long>($"uiObject/{oid}/getChildCount", "value"));
        //use UIO2 //public Task<UIObject> UIO_GetChild(long oid, FindObjectQueryStruct foqs) => Task.Run(async () => new UIObject(await CUrlGetJObject($"uiObject/{oid}/getChild", ("uiSelector", foqs.ToString())), this));

        public Task<UIObject> UD_FindObject(FindObjectQueryStruct foqs) => Task.Run(async () => new UIObject(await CUrlGetJObject("uiDevice/findObject", ("bySelector", foqs.ToString())), this));
        public Task<(long bottom, long left, long right, long top)> UIO_GetBounds(long oid) => Task.Run(async () => { var jo = await CUrlGetJObject($"uiObject/{oid}/getBounds"); return (jo["bottom"].ToObject<long>(), jo["left"].ToObject<long>(), jo["right"].ToObject<long>(), jo["top"].ToObject<long>()); });

        public Task<List<UIObject>> UD_FindObjects(FindObjectQueryStruct foqs) => Task.Run(async () => (await CUrlGetJArray("uiDevice/findObjects", ("bySelector", foqs.ToString()))).Select(i => new UIObject(i, this)).ToList());
        public Task<bool> UD_HasObject(FindObjectQueryStruct foqs) => Task.Run(async () => "true".Equals((await CUrlGetJsonParsed<string>("uiDevice/hasObject", "value", ("bySelector", foqs.ToString()))).ToLowerInvariant()));
        public Task<bool> UIO2_Clear(long oid) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>($"uiObject2/{oid}/clear", "status")));
        public Task<bool> UIO2_Click(long oid) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>($"uiObject2/{oid}/click", "status")));
        public Task<bool> UIO2_ClickAndWait(long oid, int eventConditionRef, int? timeout = null) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>($"uiObject2/{oid}/clickAndWait", "status", (nameof(eventConditionRef), eventConditionRef.ToString()), (nameof(timeout), timeout.ToString())))); //Cli.GetStringAsync($"{Url}/uiObject2/{oid}/clickAndWait?eventConditionRef={eventConditionRef}{(timeout == null ? "" : $"&timeout={timeout}")}");
        public Task<JObject> UIO2_Dump(long oid) => Task.Run(async () => await CUrlGetJObject($"uiObject2/{oid}/dump"));
        public Task<long> UIO2_GetChildCount(long oid) => Task.Run(async () => await CUrlGetJsonParsed<long>($"uiObject2/{oid}/getChildCount", "value"));
        public Task<List<UIObject>> UIO2_GetChildren(long oid) => Task.Run(async () => (await CUrlGetJArray($"uiObject2/{oid}/getChildren")).Select(i => new UIObject(i, this)).ToList());
        public Task<string> UIO2_GetContentDescription(long oid) => Task.Run(async () => await CUrlGetJsonParsed<string>($"uiObject2/{oid}/getContentDescription", "text"));
        public Task<bool> UIO2_LongClick(long oid) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>($"uiObject2/{oid}/longClick", "status")));
        public Task<bool> UIO2_SetText(long oid, string text) => Task.Run(async () => "OK".Equals(await CUrlGetJsonParsed<string>($"uiObject2/{oid}/setText", "status", (nameof(text), text))));
        public Task<string> UIO2_GetText(long oid) => Task.Run(async () => await CUrlGetJsonParsed<string>($"/uiObject2/{oid}/getText", "text"));

        #endregion



        //public Task<string> AM_ForceStop(string pkg) => Cli.GetStringAsync($"{Url}/v2/am/forceStop?pkg={pkg}");
        //public Task<string> UD_LastTraversedText() => Cli.GetStringAsync($"{Url}/v2/uiDevice/lastTraversedText");
        //public Task<string> UD_Pixel(int x, int y) => Cli.GetStringAsync($"{Url}/v2/uiDevice/pixel?x={x}&y={y}");
        //public Task<string> UD_Wait(long oid, int? timeout = null) => Cli.GetStringAsync($"{Url}/v2/uiDevice/wait?oid={oid}{(timeout == null ? "" : $"&timeout={timeout}")}");
        //public Task<string> UD_WaitForIdle(int timeout) => Cli.GetStringAsync($"{Url}/v2/uiDevice/waitForIdle?timeout={timeout}");
        //public Task<string> UD_WaitForWindowUpdate(string packageName, int? timeout = null) => Cli.GetStringAsync($"{Url}/v2/uiDevice/waitForWindowUpdate?packageName={packageName}{(timeout == null ? "" : $"&timeout={timeout}")}");

        //until


    }
}
