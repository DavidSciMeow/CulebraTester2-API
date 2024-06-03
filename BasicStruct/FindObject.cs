using Newtonsoft.Json;
using System;
using System.Text;

namespace CulebraTesterAPI.BasicStruct
{
    public struct FindObjectQueryStruct : IEquatable<FindObjectQueryStruct>
    {
        //[JsonProperty("hasChild")]
        //public FindObjectQueryStruct HasChild { get; set; }
        //[JsonProperty("hasDescendant")]
        //public FindObjectQueryStruct HasDescendant { get; set; }

        [JsonProperty("clazz")]
        public string Class { get; set; }
        [JsonProperty("clickable")]
        public bool? Clickable { get; set; }
        [JsonProperty("depth")]
        public int? Depth { get; set; }
        [JsonProperty("desc")]
        public string Desc { get; set; }
        [JsonProperty("pkg")]
        public string Pkg { get; set; }
        [JsonProperty("res")]
        public string Res { get; set; }
        [JsonProperty("scrollable")]
        public bool? Scrollable { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("index")]
        public int? Index { get; set; }
        [JsonProperty("instance")]
        public int? Instance { get; set; }

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
        public override bool Equals(object obj)
        {
            if (obj is FindObjectQueryStruct)
            {
                return Equals((FindObjectQueryStruct)obj);
            }

            return false;
        }
        public static bool operator ==(FindObjectQueryStruct left, FindObjectQueryStruct right) => left.Equals(right);
        public static bool operator !=(FindObjectQueryStruct left, FindObjectQueryStruct right) => !left.Equals(right);
        
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
