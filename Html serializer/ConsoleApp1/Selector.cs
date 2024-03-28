

using ConsoleApp1;
using Serializer1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serializer1
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector()
        {
            TagName = null;
            Id = null;
            Classes = new List<string>();
            Child = null;
            Parent = null;
        }

        public static Selector SelectorElement(string select)              
        {
            List<string> selectorParts = select.Split(' ').ToList();

            Selector rootSelector = new Selector();
            Selector currentSelector = rootSelector;

            string [] allTags = HtmlHelper.Instance.AllTags;
            foreach (var part in selectorParts)
            {
                string[] filters = Regex.Split(part, @"(?=[#.])").Where(s => s.Length > 0).ToArray();
                List<string> classes = new List<string>();
                string id = null;
                string tagName = null;
                foreach (var filter in filters)
                {
                    if (filter.StartsWith("#"))
                    {
                        currentSelector.Id = filter.Substring(1);
                    }
                    else if (filter.StartsWith("."))
                    {
                        currentSelector.Classes.Add(filter.Substring(1));
                    }
                    else if (allTags.Contains(filter))
                    {
                        currentSelector.TagName = filter;
                    }
                    else
                    {
                        Console.WriteLine("the filter didint find any thing");
                    }

                    Selector newSelector = new Selector();
                    if (currentSelector!=rootSelector)
                    {
                      newSelector.Parent = currentSelector;
                    }
                    currentSelector.Child = newSelector;

                    currentSelector = newSelector;
                }
            }
            return rootSelector;
        }


        public override bool Equals(object obj)
        {
            if (obj is HtmlElement)
            {
                HtmlElement element = (HtmlElement)obj;
                if ((this.Id == null || this.Id != null && this.Id.Equals(element.Id)) &&
                (this.TagName == null || this.TagName != null && this.TagName.Equals(element.Name)))
                {
                    if (this.Classes.Count == 0)
                        return true;
                    else
                    {
                        foreach (var c in this.Classes)
                        {
                            if (!element.Classes.Contains(c))
                            {
                                return false;
                            }
                            
                        }
                        return true;
                    }
                }

                return false;
            }
            return false;
        }
    }

}
