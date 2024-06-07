using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using CulebraTesterAPI.BasicStruct;
using System.Linq;

namespace CulebraTesterAPI
{
    public partial class CTClient : IDisposable
    {
        public HashSet<UINode> GetUI(bool debugLogConsole = false, bool debugLogStruct = false) => GetUIAsync(debugLogConsole, debugLogStruct).GetAwaiter().GetResult();
        public UIObject GetUIObject2By(FindObjectQueryStruct foqs) => GetUIObject2ByAsync(foqs).GetAwaiter().GetResult();
        public List<UIObject> GetAllUIObject2By(FindObjectQueryStruct foqs) => GetAllUIObject2ByAsync(foqs).GetAwaiter().GetResult();
        public (long bottom, long left, long right, long top) GetUIObject2FindBound(FindObjectQueryStruct foqs) => GetUIObject2FindBoundAsync(foqs).GetAwaiter().GetResult();



        public Task<HashSet<UINode>> GetUIAsync(bool debugLogConsole = false, bool debugLogStruct = false) => Task.Run(
            async () =>
            {
                var ret = new HashSet<UINode>();
                var s = JObject.Load(new JsonTextReader(new StringReader(await UD_DumpWindowHierarchy())) { MaxDepth = 1024 });
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
                    if (debugLogConsole)
                    {
                        Console.WriteLine($"{new string('│', depth)}{uinode}");
                    }

                    var children = (JArray)token["children"];
                    for (int i = 0; i < children.Count; i++)
                    {
                        stack.Push(new Tuple<JToken, int>(children[i], depth + 1));
                    }
                }
                return ret;
            });
        public Task<UIObject> GetUIObject2ByAsync(FindObjectQueryStruct foqs) => Task.Run(
            async () =>
            {
                try
                {
                    var i = await UD_FindObjects(foqs);
                    if (i.Count > 0)
                    {
                        return i.FirstOrDefault();
                    }
                }
                catch
                {
                }
                return default;
            });
        public Task<List<UIObject>> GetAllUIObject2ByAsync(FindObjectQueryStruct foqs) => Task.Run(
            async () =>
            {
                try
                {
                    var i = await UD_FindObjects(foqs);
                    if (i.Count > 0)
                    {
                        return i;
                    }
                }
                catch
                {
                }
                return new List<UIObject>();
            });
        public Task<(long bottom, long left, long right, long top)> GetUIObject2FindBoundAsync(FindObjectQueryStruct foqs)
        {
            return Task.Run(async () =>
            {
                var _fo = await UD_FindObject(foqs);
                if (_fo.Oid != -1)
                {
                    var _bound = await UIO_GetBounds(_fo.Oid);
                    var _comp = await OS_Remove(_fo.Oid);
                    return _bound;
                }
                else
                {
                    return (-1, -1, -1, -1);
                }
            });
        }

    }
}
