using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;

namespace CulebraTesterAPI.BasicStruct
{
    public struct UIObject
    {
        public CTClient Ctc { get; private set; }
        public string ClassName { get; }
        public long Oid { get; }

        public UIObject(string className, long oid)
        {
            ClassName = className;
            Oid = oid;
            Ctc = null;
        }

        public UIObject(JToken jo, CTClient ctc)
        {
            Ctc = ctc;
            ClassName = jo["className"].ToString();
            Oid = long.Parse(jo["oid"].ToString());
        }
        public override string ToString() => $"{Oid} :: [{ClassName}]";

        public Task<bool> Click() => Ctc.UIO2_Click(Oid);
    }
}
