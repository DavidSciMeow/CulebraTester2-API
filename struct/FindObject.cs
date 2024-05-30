using Newtonsoft.Json;

namespace CulebraTesterAPI
{
    public class UIObjectStruct
    {

    }
    public struct FindObjectQueryStruct
    {
        [JsonProperty("checkable")]
        public bool Checkable { get; set; }
        [JsonProperty("_checked")]
        public bool Checked { get; set; }
        [JsonProperty("clazz")]
        public string Class { get; set; }
        [JsonProperty("clickable")]
        public bool Clickable { get; set; }
        [JsonProperty("depth")]
        public int Depth { get; set; }
        [JsonProperty("desc")]
        public string Desc { get; set; }
        //[JsonProperty("hasChild")]
        //public FindObjectQueryStruct HasChild { get; set; }
        //[JsonProperty("hasDescendant")]
        //public FindObjectQueryStruct HasDescendant { get; set; }
        [JsonProperty("pkg")]
        public string Pkg { get; set; }
        [JsonProperty("res")]
        public string Res { get; set; }
        [JsonProperty("scrollable")]
        public bool Scrollable { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("instance")]
        public int Instance { get; set; }
    }
}
