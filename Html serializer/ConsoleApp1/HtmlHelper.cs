using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
 
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] AllTags { get; private set; }
        public string[] selfClosing { get; private set; }

        private HtmlHelper()
        {
            try
            {
               
                string allTagsJson = File.ReadAllText("Tags/HtmlTags.json");
                string selfClosingTagsJson = File.ReadAllText("Tags/HtmlVoidTags.json");
             
                AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
                selfClosing = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("An error occurred while loading HTML tags: " + ex.Message);
                AllTags = new string[0]; 
                selfClosing = new string[0];
            }
        }
    }
}


