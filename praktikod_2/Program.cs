using praktikod_2;
using System.Text.RegularExpressions;

var html = await Load("https://hebrewbooks.org/beis");


var htmlLines = new Regex("<(.*?)>")
    .Split(html)
    .Where(s => !string.IsNullOrWhiteSpace(s))
    .Select(s => s.Trim())
    .ToList();

var helper = HtmlHelper.Instance;
HtmlElement? root = null;
HtmlElement? currentElement = null;

foreach (var item in htmlLines)
{
    var firstWord = item.Split(' ')[0];

    if (firstWord == "/html")
    {
        break;
    }

    if (firstWord.StartsWith("/"))
    {
        if (currentElement?.Parent != null)
        {
            currentElement = currentElement.Parent;
        }

        continue;
    }

    if (helper.HtmlTags.Contains(firstWord))
    {
        var newElement = new HtmlElement();

        newElement.Name = firstWord;
        newElement.Parent = currentElement;

        if (root == null)
        {
            root = newElement;
        }
        else
        {
            currentElement!.Children.Add(newElement);
        }

        var attributesText = item.Substring(firstWord.Length).Trim();

        var matches =
            new Regex("([^\\s=]+)=\"(.*?)\"")
            .Matches(attributesText);

        foreach (Match match in matches)
        {
            var attributeName = match.Groups[1].Value;
            var attributeValue = match.Groups[2].Value;

            newElement.Attributes.Add(
                $"{attributeName}=\"{attributeValue}\""
            );

            if (attributeName == "id")
            {
                newElement.Id = attributeValue;
            }

            if (attributeName == "class")
            {
                var classes = attributeValue.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries
                );

                newElement.Classes.AddRange(classes);
            }
        }

        var isSelfClosing =
            item.EndsWith("/") ||
            helper.HtmlVoidTags.Contains(firstWord);

        if (!isSelfClosing)
        {
            currentElement = newElement;
        }
    }
    else
    {
        if (currentElement != null)
        {
            currentElement.InnerHtml += item;
        }
    }
}
Console.WriteLine(root?.Name);
Console.WriteLine(root?.Children.Count);


Console.ReadLine();

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
