using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace praktikod_2
{
    public class Selector
    {
        public string TagName { get; set; } = "";

        public string Id { get; set; } = "";

        public List<string> Classes { get; set; } = new List<string>();

        public Selector? Parent { get; set; }

        public Selector? Child { get; set; }


        public static Selector Parse(string query)
        {
            var parts = query.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries
            );

            var root = new Selector();
            var currentSelector = root;

            for (int i = 0; i < parts.Length; i++)
            {
                var selectorParts =
                    Regex.Matches(parts[i], @"(^[^#\.]+)|([#\.][^#\.]+)");

                foreach (Match match in selectorParts)
                {
                    var value = match.Value;

                    if (value.StartsWith("#"))
                    {
                        currentSelector.Id = value.Substring(1);
                    }
                    else if (value.StartsWith("."))
                    {
                        currentSelector.Classes.Add(value.Substring(1));
                    }
                    else if (HtmlHelper.Instance.HtmlTags
         .Contains(value, StringComparer.OrdinalIgnoreCase))
                    {
                        currentSelector.TagName = value.ToLower();
                    }
                }

                if (i < parts.Length - 1)
                {
                    var child = new Selector();

                    currentSelector.Child = child;
                    child.Parent = currentSelector;

                    currentSelector = child;
                }
            }

            return root;
        }

    }
}
