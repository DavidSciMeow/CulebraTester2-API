using Newtonsoft.Json;
using System.Text;

namespace CulebraTesterAPI.BasicStruct
{
    public struct FindObjectQueryStruct
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if(!string.IsNullOrEmpty(Class)) sb.Append($"clazz@{Class},");
            if(Clickable.HasValue) sb.Append($"clickable@{Clickable},");
            if(Depth.HasValue) sb.Append($"depth@{Depth},");
            if(!string.IsNullOrEmpty(Desc)) sb.Append($"desc@{Desc},");
            if(!string.IsNullOrEmpty(Pkg)) sb.Append($"package@{Pkg},");
            if(!string.IsNullOrEmpty(Res)) sb.Append($"res@{Res},");
            if(Scrollable.HasValue) sb.Append($"scrollable@{Scrollable},");
            if(!string.IsNullOrEmpty(Text)) sb.Append($"text@{Text},");
            if(Index.HasValue) sb.Append($"index@{Index},");
            if(Instance.HasValue) sb.Append($"instance@{Instance},");
            return sb.ToString();
        }
    }
}
