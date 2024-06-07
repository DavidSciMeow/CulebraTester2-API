using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;
using System.CodeDom;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;

namespace CulebraTesterAPI.BasicStruct
{
    public struct UIObject : IEquatable<UIObject>, IEnumerable<UIObject>
    {
        public CTClient Ctc { get; }
        public string ClassName { get; }
        public string Obj { get; }
        public long Oid { get; }

        public string Text { get => Ctc.UIO2_GetText(Oid).GetAwaiter().GetResult(); set => Ctc.UIO2_SetText(Oid, value).GetAwaiter().GetResult(); }
        public string ContentDescription { get => Ctc.UIO2_GetContentDescription(Oid).GetAwaiter().GetResult(); }

        public UIObject(JToken jo, CTClient ctc)
        {
            Ctc = ctc;
            ClassName = jo["className"]?.ToString() ?? null;
            Obj = jo["obj"]?.ToString() ?? null;
            Oid = long.TryParse(jo["oid"].ToString(), out var _oid) ? _oid : -1;
        }
        public override string ToString() => $"UIObj :: {Oid} :: [{ClassName}] {Text}";

        public UINode GetNode() => new UINode(Ctc.UIO2_Dump(Oid).GetAwaiter().GetResult(), 0);

        public bool Clear() => ClearAsync().GetAwaiter().GetResult();
        public bool Click() => ClickAsync().GetAwaiter().GetResult();
        public bool LongClick() => LongClickAsync().GetAwaiter().GetResult();
        public bool ClickAndWait(int eventcond, int? timeout = null) => ClickAndWaitAsync(eventcond, timeout).GetAwaiter().GetResult();
        public JObject Dump() => DumpAsync().GetAwaiter().GetResult();
        public long GetChildCount() => GetChildCountAsync().GetAwaiter().GetResult();
        public List<UIObject> GetChildren() => GetChildrenAsync().GetAwaiter().GetResult();
        public bool SetText(string text) => SetTextAsync(text).GetAwaiter().GetResult();
        public bool Remove() => RemoveAsync().GetAwaiter().GetResult();

        public Task<bool> ClearAsync() => Ctc.UIO2_Clear(Oid);
        public Task<bool> ClickAsync() => Ctc.UIO2_Click(Oid);
        public Task<bool> LongClickAsync() => Ctc.UIO2_LongClick(Oid);
        public Task<bool> ClickAndWaitAsync(int eventcond, int? timeout = null) => Ctc.UIO2_ClickAndWait(Oid, eventcond, timeout);
        public Task<JObject> DumpAsync() => Ctc.UIO2_Dump(Oid);
        public Task<long> GetChildCountAsync() => Ctc.UIO2_GetChildCount(Oid);
        public Task<List<UIObject>> GetChildrenAsync() => Ctc.UIO2_GetChildren(Oid);
        //public Task<string> FindObject() => Ctc.UIO2_FindObject(Oid);
        public Task<bool> SetTextAsync(string text) => Ctc.UIO2_SetText(Oid, text);
        public Task<bool> RemoveAsync() => Ctc.OS_Remove(Oid);

        public bool Equals(UIObject other) => ClassName == other.ClassName;
        public override int GetHashCode() => ClassName.GetHashCode();
        public override bool Equals(object obj) => obj is UIObject other && Equals(other);

        public IEnumerator<UIObject> GetEnumerator() => GetChildren().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static bool operator ==(UIObject left, UIObject right) => left.Equals(right);
        public static bool operator !=(UIObject left, UIObject right) => !left.Equals(right);
    }
}
