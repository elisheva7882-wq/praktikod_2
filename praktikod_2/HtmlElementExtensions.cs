using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace praktikod_2
{
    public static class HtmlElementExtensions
    {
        public static IEnumerable<HtmlElement> FindElements(
            this HtmlElement element,
            Selector selector)
        {
            var results = new HashSet<HtmlElement>();

            FindElementsRecursive(element, selector, results);

            return results;
        }

        private static void FindElementsRecursive(
            HtmlElement element,
            Selector selector,
            HashSet<HtmlElement> results)
        {
            var descendants = element.Descendants();

            var filtered = descendants.Where(e =>
                (string.IsNullOrEmpty(selector.TagName) || e.Name == selector.TagName) &&
                (string.IsNullOrEmpty(selector.Id) || e.Id == selector.Id) &&
                selector.Classes.All(c => e.Classes.Contains(c))
            ).ToList();

            if (selector.Child == null)
            {
                foreach (var item in filtered)
                {
                    results.Add(item);
                }
                return;
            }

            foreach (var match in filtered)
            {
                FindElementsRecursive(
                    match,
                    selector.Child,
                    results
                );
            }
        }
    }
}
