using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Id = "";
            Name = "";
            Attributes = new List<string>();
            Classes = new List<string>();
            InnerHtml = string.Empty;
            Parent = null;
            Children = new List<HtmlElement>();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var currentElement = queue.Dequeue();
                yield return currentElement;

                foreach (var child in currentElement.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        // פונקציה רוצה על כל העץ מהאלמנט ומעלה ומחזירה רשימה שטוחה של כל אבות האלמנט
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement currentElement = this;

            while (currentElement.Parent != null)
            {
                yield return currentElement.Parent;
                currentElement = currentElement.Parent;
            }
        }


    }
}
