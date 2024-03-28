using ConsoleApp1;
using Serializer1;

public static class Extension
{
    //חיפוש בעץ את כל האלמנטים שעונים לתנאי תוך שימוש בפונקציה הרקורסיבית והדפסה
    public static List<HtmlElement> searchFunc(this HtmlElement element, Selector selector)
    {
        HashSet<HtmlElement> matcheSet = new HashSet<HtmlElement>();
        searchRecurs(element, selector, matcheSet);
        foreach (HtmlElement element2 in matcheSet)
        {
            Console.Write("<name="+ element2.Name+" ");
            if (element2.Id!="")
            {
             Console.WriteLine("Id=" +element2.Id+">");
            }
            else
            {
                Console.WriteLine("Id=null>");
            }
            
        }
        return matcheSet.ToList();
    }
    
    public static void searchRecurs(HtmlElement currentElement, Selector selector, HashSet<HtmlElement> set)
    {
       //רשימת הילדים של הסלקטור הנוכחי
        IEnumerable<HtmlElement> children = currentElement.Descendants();
        List<HtmlElement> matchesList = new List<HtmlElement>();
        foreach (var child in children)
        {
         
            if (selector.Equals(child))
            {
                matchesList.Add(child);
            }
        }
        //הרמה האחרונה בעץ
        if (selector.Child.Child == null)
        {
            set.UnionWith(matchesList);
            return;
        }

        else
        {   //הפעלת הפונקציה הרקורסיבית שוב במקרה שלא ענה על התנאים הקודמים אז הוא חוזר לקריאה הקודמת שלו
            foreach (var match in matchesList)
            {
                searchRecurs(match, selector.Child, set);
            }
        }

    }

}
