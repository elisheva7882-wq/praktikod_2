using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace praktikod_2
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }


        private HtmlHelper()
        {
            var htmlTagsJson = File.ReadAllText("JSON Files/HtmlTags.json");
            var htmlVoidTagsJson = File.ReadAllText("JSON Files/HtmlVoidTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(htmlTagsJson) ?? Array.Empty<string>();
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(htmlVoidTagsJson) ?? Array.Empty<string>();
        }

    }
}
