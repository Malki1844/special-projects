using ConsoleApp1;
using Serializer1;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;


string html = await Load("https://forum.netfree.link/category/1/%D7%94%D7%9B%D7%A8%D7%96%D7%95%D7%AA");
 static HtmlElement HtmiSerializer(string html)   
{
 
    var cleanHtml = new Regex("\\s").Replace(html, " ");
    var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0); 
    htmlLines = htmlLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

    string [] allTags = HtmlHelper.Instance.AllTags;
    string [] selfClosing = HtmlHelper.Instance.selfClosing;

    HtmlElement root = new HtmlElement() { };
    HtmlElement currentElement = root;

    foreach (var line in htmlLines)
    {
        string firstWord = line.Split(" ")[0];
        if (firstWord == "html")
        {
            currentElement.Name = firstWord;

            var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);

            foreach (Match attribute in attributes)
            {
                string attributeName = attribute.Groups[1].Value;
                string attributeValue = attribute.Groups[2].Value;

                if (attributeName == "class")
                {
  
                    currentElement.Classes = attributeValue.Split(' ').ToList();
                }
                else if (attributeName == "id")
                {
                    currentElement.Id = attributeValue;
                }
                else
                {
                    currentElement.Attributes.Add(attribute.Name + " = " + attribute.Value);
                }
            }

        }
        else if (firstWord == "html/")
        {
            Console.WriteLine("finish to pass");
            break;
        }
        else if (firstWord.StartsWith("/"))
        {
            if (currentElement.Parent != null)
            {
                currentElement = currentElement.Parent;
            }
        }
        else if (allTags.Contains(firstWord))
        {

            HtmlElement newElement = new HtmlElement();
            newElement.Parent = currentElement;

            currentElement.Children.Add(newElement);
            newElement.Name = firstWord;

            var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);

            foreach (Match attribute in attributes)
            {
                string attributeName = attribute.Groups[1].Value;
                string attributeValue = attribute.Groups[2].Value;

                if (attributeName == "class")
                {

                    newElement.Classes = attributeValue.Split(' ').ToList();
                }
                else if (attributeName == "id")
                {
                    newElement.Id = attributeValue;
                }
                else
                {
                    newElement.Attributes.Add(attribute.Name + " = " + attribute.Value);
                }
            }
            if (!(line.EndsWith("/") || selfClosing.Contains(firstWord)))
            {
                currentElement = newElement;
            }
        }
        else if(currentElement!=null)
        {
          
                if (!line.Contains("<script") && !line.Contains("function()"))
                {
                    currentElement.InnerHtml += line;
                }
        
        }
    }
    return root;

}

HtmlElement htmlTree = HtmiSerializer(html);

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

//string search = "ul.nav.navbar-nav";
//string search = "div button#mobile-menu";
//string search = "div.btn-group";
string search = "p a.permalink";

//התוצאה
Selector rootResult = Selector.SelectorElement(search);
List<HtmlElement> result = htmlTree.searchFunc(rootResult);
Console.WriteLine("count of result = " + result.Count);












////פונקציה להדפסת העץ
//void PrintHtmlElementFields(HtmlElement element, int depth = 0)
//{
//    string indent = new string(' ', depth * 2);

//    Console.WriteLine($"{indent}Name: {element.Name}");
//    Console.WriteLine($"{indent}ID: {element.Id}");
//    Console.WriteLine($"{indent}Parent: {element.Parent?.Name}");

//    if (element.Attributes.Count > 0)
//    {
//        Console.WriteLine($"{indent}Attributes: {string.Join(", ", element.Attributes)}");
//    }
//    if (element.Classes.Count > 0)
//    {
//        Console.WriteLine($"{indent}Classes: {string.Join(", ", element.Classes)}");
//    }
//    if (!string.IsNullOrWhiteSpace(element.InnerHtml))
//    {
//        Console.WriteLine($"{indent}InnerHtml: {element.InnerHtml}");
//    }
//    if (element.Children.Count > 0)
//    {
//        Console.WriteLine($"{indent}Children:");
//        foreach (var child in element.Children)
//        {
//            PrintHtmlElementFields(child, depth + 1);
//        }
//    }
//}
////קורא לפונקצית הדפסת העץ
//PrintHtmlElementFields(htmlTree);





