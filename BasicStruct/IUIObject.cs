using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;

namespace CulebraTesterAPI.BasicStruct
{
    /// <summary>
    /// 一个UI对象接口
    /// </summary>
    public interface IUIObject : IEquatable<IUIObject>
    {
        /// <summary>
        /// 对照的机器实例
        /// </summary>
        CTClient Ctc { get; }
        /// <summary>
        /// 对象包含类名
        /// </summary>
        string ClassName { get; }
        /// <summary>
        /// 对象引用标志
        /// </summary>
        string Obj { get; }
        /// <summary>
        /// 被选择元素的唯一标识
        /// </summary>
        long Oid { get; }
        /// <summary>
        /// 元素的内容描述
        /// </summary>
        string ContentDescription { get; }

        /// <summary>
        /// 清除可编辑元素内部的文字(异步)
        /// </summary>
        /// <returns>如果操作成功就返回 <see langword="true"/>, 反之则为 <see langword="false"/></returns>
        Task<bool> ClearAsync();
        /// <summary>
        /// 清除可编辑元素内部的文字(同步)
        /// </summary>
        /// <returns>如果操作成功就返回 <see langword="true"/>, 反之则为 <see langword="false"/></returns>
        bool Clear();
        /// <summary>
        /// 点击元素(异步)
        /// </summary>
        /// <returns>如果成功点击就返回 <see langword="true"/>, 反之则为 <see langword="false"/></returns>
        Task<bool> ClickAsync();
        /// <summary>
        /// 点击元素(同步)
        /// </summary>
        /// <returns>如果成功点击就返回 <see langword="true"/>, 反之则为 <see langword="false"/></returns>
        bool Click();
        /// <summary>
        /// 下载一份元素的信息(异步)
        /// </summary>
        /// <returns>元素的Json信息</returns>
        Task<JObject> DumpAsync();
        /// <summary>
        /// 下载一份元素的信息(同步)
        /// </summary>
        /// <returns>元素的Json信息</returns>
        JObject Dump();
        /// <summary>
        /// 获取一个UINode
        /// </summary>
        /// <returns>元素的UINode表示</returns>
        UINode GetNode();
        /// <summary>
        /// 获取子元素的数量(异步)
        /// </summary>
        /// <returns>子元素的数量</returns>
        Task<long> GetChildCountAsync();
        /// <summary>
        /// 获取子元素的数量(同步)
        /// </summary>
        /// <returns>子元素的数量</returns>
        long GetChildCount();
        /// <summary>
        /// 从自带管理的元素列表上移除元素(异步)
        /// </summary>
        /// <returns>如果成功移除就返回 <see langword="true"/>, 反之则为 <see langword="false"/></returns>
        Task<bool> RemoveAsync();
        /// <summary>
        /// 从自带管理的元素列表上移除元素(异步)
        /// </summary>
        /// <returns>如果成功移除就返回 <see langword="true"/>, 反之则为 <see langword="false"/></returns>
        bool Remove();
    }
}
