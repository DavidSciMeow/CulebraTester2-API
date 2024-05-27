using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

namespace CulebraTesterAPI
{
    public class CTClient : IDisposable
    {
        public string Url { get; }
        public HttpClient Cli { get; } = new HttpClient();
        public CTClient(string url = "http://localhost", int port = 9987) => Url = $"{url}:{port}";
        public void Dispose() => ((IDisposable)Cli).Dispose();

        public Task<HashSet<UINode>> GetUI(bool debugLogStruct = false) => Task.Run(
            async () =>
            {
                var ret = new HashSet<UINode>();

                var jo = await UD_DumpWindowHierarchy();

                var s = JObject.Load(new JsonTextReader(new StringReader(jo)) { MaxDepth = 1024 });
                Stack<Tuple<JToken, int>> stack = new Stack<Tuple<JToken, int>>();
                stack.Push(new Tuple<JToken, int>(s, 0));
                while (stack.Count > 0)
                {
                    var tuple = stack.Pop();
                    var token = tuple.Item1;
                    var depth = tuple.Item2;
                    var uinode = new UINode(token as JObject, depth);

                    ret.Add(uinode);

                    if (debugLogStruct)
                    {
                        Debug.WriteLine($"{new string('│', depth)}{uinode}");
                    }

                    var children = (JArray)token["children"];
                    for (int i = 0; i < children.Count; i++)
                    {
                        stack.Push(new Tuple<JToken, int>(children[i], depth + 1));
                    }
                }
                return ret;
            });

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

        public Task<string> UD_ClearLastTraversedText() => Cli.GetStringAsync($"{Url}/v2/uiDevice/clearLastTraversedText");
        public Task<string> UD_Click(long x, long y) => Cli.GetStringAsync($"{Url}/v2/uiDevice/click?x={x}&y={y}");
        public Task<string> UD_Click(UINode node) => Cli.GetStringAsync($"{Url}/v2/uiDevice/click?x={node.ClickCenter.X}&y={node.ClickCenter.Y}");
        public Task<string> UD_CurrentPackageName() => Cli.GetStringAsync($"{Url}/v2/uiDevice/currentPackageName");
        public Task<string> UD_DisplayHeight() => Cli.GetStringAsync($"{Url}/v2/uiDevice/displayHeight");
        public Task<string> UD_DisplayRotation() => Cli.GetStringAsync($"{Url}/v2/uiDevice/displayRotation");
        public Task<string> UD_DisplaySizeDp() => Cli.GetStringAsync($"{Url}/v2/uiDevice/displaySizeDp");
        public Task<string> UD_DisplayWidth() => Cli.GetStringAsync($"{Url}/v2/uiDevice/displayWidth");
        public Task<string> UD_Drag(long startX, long startY, long endX, long endY, long steps = 1) => Cli.GetStringAsync($"{Url}/v2/uiDevice/drag?startX={startX}&startY={startY}&endX={endX}&endY={endY}&steps={steps}");
        public Task<string> UD_DumpWindowHierarchy() => Cli.GetStringAsync($"{Url}/v2/uiDevice/dumpWindowHierarchy");

        //find object
        //find objects
        public Task<string> UD_FreezeRotation() => Cli.GetStringAsync($"{Url}/v2/uiDevice/freezeRotation");
        //has objects
        public Task<string> UD_IsNaturalOrientation() => Cli.GetStringAsync($"{Url}/v2/uiDevice/isNaturalOrientation");
        public Task<string> UD_IsScreenOn() => Cli.GetStringAsync($"{Url}/v2/uiDevice/isScreenOn");
        public Task<string> UD_LastTraversedText() => Cli.GetStringAsync($"{Url}/v2/uiDevice/lastTraversedText");
        public Task<string> UD_Pixel() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pixel");
        public Task<string> UD_PressBack() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressBack");
        public Task<string> UD_PressDPadCenter() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadCenter");
        public Task<string> UD_PressDPadDown() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadDown");
        public Task<string> UD_PressDPadLeft() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadLeft");
        public Task<string> UD_PressDPadRight() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadRight");
        public Task<string> UD_PressDPadUp() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDPadUp");
        public Task<string> UD_PressDelete() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressDelete");
        public Task<string> UD_PressEnter() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressEnter");
        public Task<string> UD_PressHome() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressHome");
        public Task<string> UD_PressKeyCode(Key key, int? metaState = null) => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressKeyCode?keyCode={key}{(metaState == null ? "" : $"&metaState={metaState}")}");
        public Task<string> UD_PressRecentApps() => Cli.GetStringAsync($"{Url}/v2/uiDevice/pressRecentApps");
        public Task<string> UD_ProductName() => Cli.GetStringAsync($"{Url}/v2/uiDevice/productName");
        public Task<string> UD_Screenshot() => Cli.GetStringAsync($"{Url}/v2/uiDevice/screenshot");
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
        public Task<string> UIO2_ClickAndWait(long oid,int eventConditionRef, int? timeout = null) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/clickAndWait?eventConditionRef={eventConditionRef}{(timeout == null ? "" : $"&timeout={timeout}")}");
        public Task<string> UIO2_Dump(long oid) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/dump");
        public Task<string> UIO2_GetChildCount(long oid) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/getChildCount");

        public Task<string> UIO2_SetText(long oid, string text) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/setText?text={text}");
        public Task<string> UIO2_LongClick(long oid) => Cli.GetStringAsync($"{Url}/uiObject2/{oid}/longClick");



    }
}
