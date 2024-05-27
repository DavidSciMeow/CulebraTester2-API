using Newtonsoft.Json.Linq;
using System;

namespace CulebraTesterAPI
{

    /// <summary>
    /// UI节点的结构
    /// </summary>
    public readonly struct UINode
    {
        /// <summary>
        /// 本框的Id
        /// </summary>
        public long Id { get; }
        /// <summary>
        /// 本框的父框
        /// </summary>
        public long Parent { get; }
        /// <summary>
        /// 本框的深度
        /// </summary>
        public long Depth { get; }
        /// <summary>
        /// 本框在当前级别的索引
        /// </summary>
        public long Index { get; }
        /// <summary>
        /// 框上文字
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// 框上长文字
        /// </summary>
        public string LongText { get; }
        /// <summary>
        /// 资源的ID号
        /// </summary>
        public string Resource_id { get; }
        /// <summary>
        /// 资源的类
        /// </summary>
        public string Class { get; }
        /// <summary>
        /// 内部描述
        /// </summary>
        public string Content_desc { get; }
        /// <summary>
        /// 是否可以被选中
        /// </summary>
        public bool Checkable { get; }
        /// <summary>
        /// 是否已被选中
        /// </summary>
        public bool Checked { get; }
        /// <summary>
        /// 是否可以被点击
        /// </summary>
        public bool Clickable { get; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; }
        /// <summary>
        /// 是否可以获得焦点
        /// </summary>
        public bool Focusable { get; }
        /// <summary>
        /// 元素是否可以滚动
        /// </summary>
        public bool Scrollable { get; }
        /// <summary>
        /// 是否可以长按
        /// </summary>
        public bool Longclickable { get; }
        /// <summary>
        /// 是否是密码字段
        /// </summary>
        public bool Password { get; }
        /// <summary>
        /// 是否已被选中
        /// </summary>
        public bool Selected { get; }
        /// <summary>
        /// 位置的左上角标和右下角标
        /// </summary>
        public (long X1, long Y1, long X2, long Y2) Bounds { get; }

        /// <summary>
        /// 获取元素可点击的中心点
        /// </summary>
        public (long X, long Y) ClickCenter => ((Bounds.X1 + Bounds.X2) / 2, (Bounds.Y1 + Bounds.Y2) / 2);

        /// <summary>
        /// 实例化一个UINode的结构
        /// </summary>
        /// <param name="jo">UINode原生类</param>
        /// <param name="depth">UINode深度</param>
        public UINode(JObject jo, long depth)
        {
            Depth = depth;
            Parent = int.TryParse(jo["parent"]?.ToString() ?? "-1", out var parent) ? parent : -1;
            Index = int.TryParse(jo["index"]?.ToString() ?? "-1", out var index) ? index : -1;
            Id = int.TryParse(jo["id"]?.ToString() ?? "-1", out var id) ? id : -1;
            LongText = jo["longText"]?.ToString() ?? string.Empty;
            Text = jo["text"]?.ToString() ?? string.Empty;
            Resource_id = jo["resourceId"]?.ToString() ?? string.Empty;
            Class = jo["clazz"]?.ToString() ?? string.Empty;
            Content_desc = jo["contentDescription"]?.ToString() ?? string.Empty;
            Checkable = bool.TryParse(jo["checkable"]?.ToString() ?? "false", out var checkable) && checkable;
            Checked = bool.TryParse(jo["checked"]?.ToString() ?? "false", out var @checked) && @checked;
            Clickable = bool.TryParse(jo["clickable"]?.ToString() ?? "false", out var clickable) && clickable;
            Enabled = bool.TryParse(jo["enabled"]?.ToString() ?? "false", out var enabled) && enabled;
            Focusable = bool.TryParse(jo["focusable"]?.ToString() ?? "false", out var focusable) && focusable;
            Scrollable = bool.TryParse(jo["scrollable"]?.ToString() ?? "false", out var scrollable) && scrollable;
            Longclickable = bool.TryParse(jo["longClickable"]?.ToString() ?? "false", out var longclickable) && longclickable;
            Password = bool.TryParse(jo["password"]?.ToString() ?? "false", out var password) && password;
            Selected = bool.TryParse(jo["selected"]?.ToString() ?? "false", out var selected) && selected;
            var pd = jo["bounds"]?.ToString().Replace("][", ",").Replace("[", "").Replace("]", "").Trim().Split(',');
            if (pd != null && pd.Length == 4)
            {
                long.TryParse(pd[0], out var x1);
                long.TryParse(pd[1], out var y1);
                long.TryParse(pd[2], out var x2);
                long.TryParse(pd[3], out var y2);
                Bounds = (x1, y1, x2, y2);
            }
            else
            {
                Bounds = (-1, -1, -1, -1);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"│ {Id,-4} │ Pp:{Parent,-3} │ Dp:{Depth,-3} │ Ip:{Index,-3} " +
            $"││ Cc:{ClickCenter.X,-4},{ClickCenter.Y,-4} ││ Bd:{Bounds} ││ Ri:{Resource_id} / Cl:{Class} ││ " +
            $"{(Checkable ? "+" : "_")}" +
            $"{(Checked ? "+" : "_")}" +
            $"{(Clickable ? "+" : "_")}" +
            $"{(Enabled ? "+" : "_")}" +
            $"{(Focusable ? "+" : "_")}" +
            $"{(Scrollable ? "+" : "_")}" +
            $"{(Longclickable ? "+" : "_")}" +
            $"{(Password ? "+" : "_")}" +
            $"{(Selected ? "+" : "_")}" +
            $"" +
            $" ││ {(Text.Trim().Equals(Content_desc) ? $"{Text}" : $"{Text}:[{Content_desc}]")}";
    }
}
