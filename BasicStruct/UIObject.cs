using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace CulebraTesterAPI.BasicStruct
{
    /// <summary>
    /// 新版UI对象
    /// </summary>
    public readonly struct UIObject : IUIObject, IEnumerable<UIObject>
    {
        /// <inheritdoc/>
        public CTClient Ctc { get; }
        /// <inheritdoc/>
        public string ClassName { get; }
        /// <inheritdoc/>
        public string Obj { get; }
        /// <inheritdoc/>
        public long Oid { get; }

        public string Text { get => Ctc.UIO2_GetText(Oid).GetAwaiter().GetResult(); set => Ctc.UIO2_SetText(Oid, value).GetAwaiter().GetResult(); }
        /// <inheritdoc/>
        public string ContentDescription { get => Ctc.UIO2_GetContentDescription(Oid).GetAwaiter().GetResult(); }

        /// <summary>
        /// 实例化一个UI对象 (内部参数决定)
        /// </summary>
        /// <param name="jo">内部参数决定</param>
        /// <param name="ctc">内部参数决定</param>
        public UIObject(JToken jo, CTClient ctc)
        {
            Ctc = ctc;
            ClassName = jo["className"]?.ToString() ?? null;
            Obj = jo["obj"]?.ToString() ?? null;
            Oid = long.TryParse(jo["oid"]?.ToString() ?? "-1", out var _oid) ? _oid : -1;
        }

        public bool LongClick() => LongClickAsync().GetAwaiter().GetResult();
        public bool ClickAndWait(int eventcond, int? timeout = null) => ClickAndWaitAsync(eventcond, timeout).GetAwaiter().GetResult();
        public List<UIObject> GetChildren() => GetChildrenAsync().GetAwaiter().GetResult();
        public bool SetText(string text) => SetTextAsync(text).GetAwaiter().GetResult();
        
        public Task<bool> ClickAndWaitAsync(int eventcond, int? timeout = null) => Ctc.UIO2_ClickAndWait(Oid, eventcond, timeout);
        public Task<bool> SetTextAsync(string text) => Ctc.UIO2_SetText(Oid, text);
        public Task<bool> LongClickAsync() => Ctc.UIO2_LongClick(Oid);
        public Task<List<UIObject>> GetChildrenAsync() => Ctc.UIO2_GetChildren(Oid);


        #region Implementation of (interface, cood)
        /// <inheritdoc/>
        public UINode GetNode() => new UINode(Ctc.UIO2_Dump(Oid).GetAwaiter().GetResult(), 0);
        /// <inheritdoc/>
        public bool Clear() => ClearAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public bool Click() => ClickAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public JObject Dump() => DumpAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public long GetChildCount() => GetChildCountAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public bool Remove() => RemoveAsync().GetAwaiter().GetResult();
        /// <inheritdoc/>
        public Task<bool> ClearAsync() => Ctc.UIO2_Clear(Oid);
        /// <inheritdoc/>
        public Task<bool> ClickAsync() => Ctc.UIO2_Click(Oid);
        /// <inheritdoc/>
        public Task<JObject> DumpAsync() => Ctc.UIO2_Dump(Oid);
        /// <inheritdoc/>
        public Task<long> GetChildCountAsync() => Ctc.UIO2_GetChildCount(Oid);
        /// <inheritdoc/>
        public Task<bool> RemoveAsync() => Ctc.OS_Remove(Oid);
        /// <inheritdoc/>
        public bool Equals(UIObject other) => ClassName == other.ClassName;
        /// <inheritdoc/>
        public override int GetHashCode() => ClassName.GetHashCode();
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is UIObject other && Equals(other);
        /// <inheritdoc/>
        public IEnumerator<UIObject> GetEnumerator() => GetChildren().GetEnumerator();
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        /// <inheritdoc/>
        public bool Equals(IUIObject other) => ClassName == other.ClassName;
        /// <inheritdoc/>
        public static bool operator ==(UIObject left, UIObject right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(UIObject left, UIObject right) => !left.Equals(right);
        /// <inheritdoc/>
        public override string ToString() => $"UIObj :: {Oid} :: [{ClassName}] {Text}";
        #endregion
    }
}
