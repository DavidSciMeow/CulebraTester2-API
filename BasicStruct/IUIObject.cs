using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;

namespace CulebraTesterAPI.BasicStruct
{
    public interface IUIObject : IEquatable<IUIObject>
    {
        CTClient Ctc { get; }
        string ClassName { get; }
        string Obj { get; }
        long Oid { get; }
        string ContentDescription { get; }

        Task<bool> ClearAsync();
        bool Clear();
        Task<bool> ClickAsync();
        bool Click();
        Task<JObject> DumpAsync();
        JObject Dump();
        UINode GetNode();
        Task<long> GetChildCountAsync();
        long GetChildCount();
        Task<bool> RemoveAsync();
        bool Remove();
    }
}
