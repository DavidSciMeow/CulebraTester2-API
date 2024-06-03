using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace CulebraTesterAPI.BasicStruct
{
    public struct UIObject2 : IEquatable<UIObject2>
    {
        public CTClient Ctc { get; private set; }
        public string ClassName { get; }
        public long Oid { get; }

        public UIObject2(JToken jo, CTClient ctc)
        {
            Ctc = ctc;
            ClassName = jo["className"].ToString();
            Oid = long.Parse(jo["oid"].ToString());
        }
        public override string ToString() => $"UIObj :: {Oid} :: [{ClassName}]";

        public UINode GetNode() => new UINode(Ctc.UIO2_Dump(Oid).GetAwaiter().GetResult(), 0);

        public Task<bool> Clear() => Ctc.UIO2_Clear(Oid);
        public Task<bool> Click() => Ctc.UIO2_Click(Oid);
        public Task<bool> ClickAndWait(int eventcond, int? timeout = null) => Ctc.UIO2_ClickAndWait(Oid, eventcond, timeout);
        public Task<JObject> Dump() => Ctc.UIO2_Dump(Oid);
        public Task<long> GetChildCount() => Ctc.UIO2_GetChildCount(Oid);
        public Task<List<UIObject2>> GetChildren() => Ctc.UIO2_GetChildren(Oid);
        public Task<string> GetContentDescription() => Ctc.UIO2_GetContentDescription(Oid);
        //public Task<string> FindObject() => Ctc.UIO2_FindObject(Oid);
        public Task<string> GetText() => Ctc.UIO2_GetText(Oid);
        public Task<bool> LongClick() => Ctc.UIO2_LongClick(Oid);
        public Task<bool> SetText(string text) => Ctc.UIO2_SetText(Oid, text);
        public Task<bool> Remove() => Ctc.OS_Remove(Oid);

        public bool Equals(UIObject2 other) => ClassName == other.ClassName;
        public override int GetHashCode() => ClassName.GetHashCode();
        public override bool Equals(object obj) => obj is UIObject2 other && Equals(other);

        public static bool operator ==(UIObject2 left, UIObject2 right) => left.Equals(right);
        public static bool operator !=(UIObject2 left, UIObject2 right) => !left.Equals(right);
    }
}
