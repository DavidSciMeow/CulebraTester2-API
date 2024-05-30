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
    public partial class CTClient : IDisposable
    {
        private string Url { get; }
        private HttpClient Cli { get; } = new HttpClient();
        private CTClient() { }
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
    }
}
