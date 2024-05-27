using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Collections.Generic;

namespace CulebraTesterAPI
{
    public static class UISyncDevice
    {
        public static bool Click(this CTClient Client, UINode node) => (JObject.Parse(Client.UD_Click(node).GetAwaiter().GetResult())?["status"]?.ToString()?.ToUpperInvariant() ?? "ERR") == "OK";
        public static bool ClickUIWhereTextContains(this CTClient c, string ss)
        {
            var uio = c.GetUI();
            var element = from a in uio where a.Text.Trim().Contains(ss) select a;
            if (element.Any())
            {
                var cc = element.First();
                var ret = Click(c, cc);
                Console.WriteLine($"{cc}::{ret}");
            }
            else
            {
                foreach (var i in uio)
                {
                    Console.WriteLine(i);
                }
                return false;
            }
            return true;
        }
        public static bool ClickUIWhereDescIs(this CTClient c, string ss)
        {
            var uio = c.GetUI();
            var element = from a in uio where a.Content_desc.Trim() == ss select a;
            if (element.Any())
            {
                var cc = element.First();
                var ret = Click(c, cc);
                Console.WriteLine($"{cc}::{ret}");
            }
            else
            {
                foreach (var i in uio)
                {
                    Console.WriteLine(i);
                }
                return false;
            }
            return true;
        }
        public static bool ClickUIWhereTextIs(this CTClient c, string ss)
        {
            var uio = c.GetUI();
            var element = from a in uio where a.Text.Trim() == ss select a;
            if (element.Any())
            {
                var cc = element.First();
                var ret = Click(c, cc);
                Console.WriteLine($"{cc}::{ret}");
            }
            else
            {
                foreach (var i in uio)
                {
                    Console.WriteLine(i);
                }
                return false;
            }
            return true;
        }

        public static List<UINode> GetAllElementWithTextSpecific(this HashSet<UINode> ui, string ss)
        {
            return (from a in ui where a.Text.Trim() == ss select a).ToList();
        }
        public static UINode? GetFirstElementWithTextSpecific(this HashSet<UINode> ui, string ss)
        {
            foreach(var i in ui)
            {
                if(i.Text.Trim() == ss)
                {
                    return i;
                }
            }
            return null;
        }

        public static List<UINode> GetAllElementsInDepth(this HashSet<UINode> ui, int depth)
        {
            var ret = from a in ui where a.Depth == depth select a;
            return ret.ToList();
        }
        public static Dictionary<UINode,List<UINode>> GetAllElementsInDepthAndFirstChilds(this HashSet<UINode> ui, int depth)
        {
            Dictionary<UINode, List<UINode>> dp = new Dictionary<UINode, List<UINode>>();
            var ret = from a in ui where a.Depth == depth select a;
            foreach(var i in ret)
            {
                dp.Add(i, GetAllChildrenBelow(ui, i.Id, 1));
            }
            return dp;
        }
        public static List<UINode> GetAllElementsInParents(this HashSet<UINode> ui, int parents)
        {
            var ret = from a in ui where a.Parent == parents select a;
            return ret.ToList();
        }
        public static Dictionary<UINode, List<UINode>> GetAllElementsInParentsAndFirstChilds(this HashSet<UINode> ui, int parents)
        {
            Dictionary<UINode, List<UINode>> dp = new Dictionary<UINode, List<UINode>>();
            var ret = from a in ui where a.Parent == parents select a;
            foreach (var i in ret)
            {
                dp.Add(i, GetAllChildrenBelow(ui, i.Id, 1));
            }
            return dp;
        }

        public static UINode? GetParentNode(this HashSet<UINode> ui, UINode p)
        {
            foreach(var i in ui)
            {
                if(i.Id == p.Parent)
                {
                    return i;
                }
            }
            return null;
        }
        public static IEnumerable<UINode> GetAllParentAbove(this HashSet<UINode> allNodes, UINode Node, int depth = -1)
        {
            if (depth > 0 || depth == -1)
            {
                var _p = allNodes.Where(node => node.Id == Node.Parent);

                foreach (var i in _p)
                {
                    yield return i;

                    int newDepth = (depth == -1) ? -1 : depth - 1;

                    foreach (var gp in GetAllParentAbove(allNodes, i))
                    {
                        yield return gp;
                    }
                }
            }
        }

        public static UINode GetNode(this HashSet<UINode> ui, long id)
        {
            foreach (var i in ui)
            {
                if (i.Id == id)
                {
                    return i;
                }
            }
            return new UINode();
        }   

        public static List<UINode> GetAllChildrenBelow(this HashSet<UINode> ui, long id, int depth = -1)
        {
            List<UINode> nodes = new List<UINode>();
            foreach (var i in ui)
            {
                if (i.Id == id)
                {
                    var _chi = ui.GetAllChildren(i, depth);
                    foreach (var j in _chi)
                    {
                        nodes.Add(j);
                    }
                    break;
                }
            }
            return nodes;
        }
        public static IEnumerable<UINode> GetAllChildren(this HashSet<UINode> allNodes, UINode parentNode, int depth = -1)
        {
            if (depth > 0 || depth == -1)
            {
                var children = allNodes.Where(node => node.Parent == parentNode.Id);

                foreach (var child in children)
                {
                    yield return child;

                    int newDepth = (depth == -1) ? -1 : depth - 1;

                    foreach (var grandChild in GetAllChildren(allNodes, child))
                    {
                        yield return grandChild;
                    }
                }
            }
        }
    }
}
