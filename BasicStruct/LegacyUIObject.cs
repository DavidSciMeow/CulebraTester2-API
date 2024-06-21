using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CulebraTesterAPI.BasicStruct
{
    /// <summary>
    /// 旧版UI对象
    /// </summary>
    public readonly struct LegacyUIObject : IUIObject
    {
        /// <inheritdoc/>
        public CTClient Ctc { get; }
        /// <inheritdoc/>
        public string ClassName { get; }
        /// <inheritdoc/>
        public string Obj { get; }
        /// <inheritdoc/>
        public long Oid { get; }
        /// <inheritdoc/>
        public string ContentDescription { get => Ctc.UIO_GetContentDescription(Oid).GetAwaiter().GetResult(); }

        /// <summary>
        /// 实例化一个旧版UI对象 (内部参数决定)
        /// </summary>
        /// <param name="jo">内部参数决定</param>
        /// <param name="ctc">内部参数决定</param>
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
        /// <inheritdoc/>
        public UINode GetNode() => new UINode(Ctc.UIO_Dump(Oid).GetAwaiter().GetResult(), 0);
        /// <inheritdoc/>
        public Task<bool> ClearAsync() => Ctc.UIO_ClearTextField(Oid);
        /// <inheritdoc/>
        public Task<bool> ClickAsync() => Ctc.UIO_Click(Oid);
        /// <inheritdoc/>
        public Task<JObject> DumpAsync() => Ctc.UIO_Dump(Oid);
        /// <inheritdoc/>
        public Task<long> GetChildCountAsync() => Ctc.UIO_GetChildCount(Oid);
        /// <inheritdoc/>
        public Task<bool> RemoveAsync() => Ctc.OS_Remove(Oid);
        /// <inheritdoc/>
        public bool Click() => ClickAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public bool Clear() => ClearAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public JObject Dump() => DumpAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public long GetChildCount() => GetChildCountAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public bool Remove() => RemoveAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public bool Equals(IUIObject other) => ClassName == other.ClassName;
        /// <inheritdoc/>
        public bool Equals(LegacyUIObject other) => ClassName == other.ClassName;
        /// <inheritdoc/>
        public override int GetHashCode() => ClassName.GetHashCode();
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LegacyUIObject other && Equals(other);
        /// <inheritdoc/>
        public static bool operator ==(LegacyUIObject left, LegacyUIObject right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(LegacyUIObject left, LegacyUIObject right) => !left.Equals(right);
        /// <inheritdoc/>
        public override string ToString() => $"LL_UIObj :: {Oid} :: [{ClassName}] ";
        #endregion
    }
}
