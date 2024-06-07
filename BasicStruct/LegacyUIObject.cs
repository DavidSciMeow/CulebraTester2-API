using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CulebraTesterAPI.BasicStruct
{
    public readonly struct LegacyUIObject : IUIObject
    {
        public CTClient Ctc { get; }
        public string ClassName { get; }
        public string Obj { get; }
        public long Oid { get; }
        public string ContentDescription { get => Ctc.UIO_GetContentDescription(Oid).GetAwaiter().GetResult(); }

        public LegacyUIObject(JToken jo, CTClient ctc)
        {
            Ctc = ctc;
            ClassName = jo["className"]?.ToString() ?? null;
            Obj = jo["obj"]?.ToString() ?? null;
            Oid = long.TryParse(jo["oid"]?.ToString() ?? "-1", out var _oid) ? _oid : -1;
        }

        public Task<bool> ExistsAsync() => Ctc.UIO_Exists(Oid);
        public Task<(long bottom, long left, long right, long top)> GetBoundsAsync() => Ctc.UIO_GetBounds(Oid);
        public Task<bool> ClickAndWaitNewWindowAsync() => Ctc.UIO_ClickAndWaitForNewWindow(Oid);
        public (long bottom, long left, long right, long top) GetBounds() => GetBoundsAsync().GetAwaiter().GetResult();
        public bool Exists() => ExistsAsync().GetAwaiter().GetResult();
        public bool ClickAndWaitNewWindow() => ClickAndWaitNewWindowAsync().GetAwaiter().GetResult();

        #region Implementation of (interface, cood)
        public UINode GetNode() => new UINode(Ctc.UIO_Dump(Oid).GetAwaiter().GetResult(), 0);
        public Task<bool> ClearAsync() => Ctc.UIO_ClearTextField(Oid);
        public Task<bool> ClickAsync() => Ctc.UIO_Click(Oid);
        public Task<JObject> DumpAsync() => Ctc.UIO_Dump(Oid);
        public Task<long> GetChildCountAsync() => Ctc.UIO_GetChildCount(Oid);
        public Task<bool> RemoveAsync() => Ctc.OS_Remove(Oid);
        public bool Click() => ClickAsync().GetAwaiter().GetResult();
        public bool Clear() => ClearAsync().GetAwaiter().GetResult();
        public JObject Dump() => DumpAsync().GetAwaiter().GetResult();
        public long GetChildCount() => GetChildCountAsync().GetAwaiter().GetResult();
        public bool Remove() => RemoveAsync().GetAwaiter().GetResult();
        public bool Equals(IUIObject other) => ClassName == other.ClassName;
        public bool Equals(LegacyUIObject other) => ClassName == other.ClassName;
        public override int GetHashCode() => ClassName.GetHashCode();
        public override bool Equals(object obj) => obj is LegacyUIObject other && Equals(other);
        public static bool operator ==(LegacyUIObject left, LegacyUIObject right) => left.Equals(right);
        public static bool operator !=(LegacyUIObject left, LegacyUIObject right) => !left.Equals(right);
        public override string ToString() => $"LL_UIObj :: {Oid} :: [{ClassName}] ";
        #endregion
    }
}
