using Newtonsoft.Json;
using System;
using System.Text;

namespace CulebraTesterAPI.BasicStruct
{
    /// <summary>
    /// 获取UI的查询结构
    /// </summary>
    public struct FindObjectQueryStruct : IEquatable<FindObjectQueryStruct>
    {
        //[JsonProperty("hasChild")]
        //public FindObjectQueryStruct HasChild { get; set; }
        //[JsonProperty("hasDescendant")]
        //public FindObjectQueryStruct HasDescendant { get; set; }

        /// <summary>
        /// UI的类
        /// </summary>
        [JsonProperty("clazz")]
        public string Class { get; set; }
        /// <summary>
        /// UI是否可点击
        /// </summary>
        [JsonProperty("clickable")]
        public bool? Clickable { get; set; }
        /// <summary>
        /// UI的深度层
        /// </summary>
        [JsonProperty("depth")]
        public int? Depth { get; set; }
        /// <summary>
        /// UI的描述
        /// </summary>
        [JsonProperty("desc")]
        public string Desc { get; set; }
        /// <summary>
        /// UI的包
        /// </summary>
        [JsonProperty("pkg")]
        public string Pkg { get; set; }
        /// <summary>
        /// UI的资源类
        /// </summary>
        [JsonProperty("res")]
        public string Res { get; set; }
        /// <summary>
        /// UI是否可滚动
        /// </summary>
        [JsonProperty("scrollable")]
        public bool? Scrollable { get; set; }
        /// <summary>
        /// UI的文字
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
        /// <summary>
        /// UI的索引
        /// </summary>
        [JsonProperty("index")]
        public int? Index { get; set; }
        /// <summary>
        /// UI的实例
        /// </summary>
        [JsonProperty("instance")]
        public int? Instance { get; set; }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                if (Class != null)
                    hash = hash * 23 + Class.GetHashCode();
                if (Clickable.HasValue)
                    hash = hash * 23 + Clickable.Value.GetHashCode();
                if (Depth.HasValue)
                    hash = hash * 23 + Depth.Value.GetHashCode();
                if (Desc != null)
                    hash = hash * 23 + Desc.GetHashCode();
                if (Pkg != null)
                    hash = hash * 23 + Pkg.GetHashCode();
                if (Res != null)
                    hash = hash * 23 + Res.GetHashCode();
                if (Scrollable.HasValue)
                    hash = hash * 23 + Scrollable.Value.GetHashCode();
                if (Text != null)
                    hash = hash * 23 + Text.GetHashCode();
                if (Index.HasValue)
                    hash = hash * 23 + Index.Value.GetHashCode();
                if (Instance.HasValue)
                    hash = hash * 23 + Instance.Value.GetHashCode();

                return hash;
            }

        }
        /// <inheritdoc/>
        public bool Equals(FindObjectQueryStruct other)
        {
            if (Class != null && !Class.Equals(other.Class))
                return false;
            if (Clickable.HasValue && !Clickable.Value.Equals(other.Clickable))
                return false;
            if (Depth.HasValue && !Depth.Value.Equals(other.Depth))
                return false;
            if (Desc != null && !Desc.Equals(other.Desc))
                return false;
            if (Pkg != null && !Pkg.Equals(other.Pkg))
                return false;
            if (Res != null && !Res.Equals(other.Res))
                return false;
            if (Scrollable.HasValue && !Scrollable.Value.Equals(other.Scrollable))
                return false;
            if (Text != null && !Text.Equals(other.Text))
                return false;
            if (Index.HasValue && !Index.Value.Equals(other.Index))
                return false;
            if (Instance.HasValue && !Instance.Value.Equals(other.Instance))
                return false;

            return true;
        }
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FindObjectQueryStruct _o && Equals(_o);
        /// <inheritdoc/>
        public static bool operator ==(FindObjectQueryStruct left, FindObjectQueryStruct right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(FindObjectQueryStruct left, FindObjectQueryStruct right) => !left.Equals(right);
        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Class)) sb.Append($"clazz@{Class},");
            if (Clickable.HasValue) sb.Append($"clickable@{Clickable},");
            if (Depth.HasValue) sb.Append($"depth@{Depth},");
            if (!string.IsNullOrEmpty(Desc)) sb.Append($"desc@{Desc},");
            if (!string.IsNullOrEmpty(Pkg)) sb.Append($"package@{Pkg},");
            if (!string.IsNullOrEmpty(Res)) sb.Append($"res@{Res},");
            if (Scrollable.HasValue) sb.Append($"scrollable@{Scrollable},");
            if (!string.IsNullOrEmpty(Text)) sb.Append($"text@{Text},");
            if (Index.HasValue) sb.Append($"index@{Index},");
            if (Instance.HasValue) sb.Append($"instance@{Instance},");
            return sb.ToString();
        }
    }
}