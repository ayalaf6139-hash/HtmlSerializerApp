using System.Text.RegularExpressions;

namespace HtmlSerializerApp
{
    public class HtmlSerializer
    {
        public async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        public HtmlElement Serialize(string html)
        {
            string cleanHtml = Regex.Replace(html, @"\s+", " ");
            var parts = Regex.Split(cleanHtml, @"(<[^>]*>)")
                             .Select(p => p.Trim())
                             .Where(p => p.Length > 0);

            HtmlElement root = new HtmlElement { Name = "root" };
            HtmlElement current = root;

            foreach (var part in parts)
            {
                if (part.StartsWith("<!"))
                    continue;

                if (part.StartsWith("<") && part.EndsWith(">"))
                {
                    string tagContent = part.Substring(1, part.Length - 2).Trim();
                    string firstWord = tagContent.Split(' ')[0];

                    if (firstWord.ToLower() == "/html")
                        break;

                    if (firstWord.StartsWith("/"))
                    {
                        if (current.Parent != null)
                            current = current.Parent;
                        continue;
                    }

                    string tagName = firstWord.TrimEnd('/').ToLower();

                    if (HtmlHelper.Instance.HtmlTags.Contains(tagName))
                    {
                        HtmlElement newElement = new HtmlElement { Name = tagName, Parent = current };
                        string rest = tagContent.Substring(firstWord.Length);
                        ParseAttributes(newElement, rest);
                        current.Children.Add(newElement);

                        bool isSelfClosing = tagContent.EndsWith("/") || HtmlHelper.Instance.HtmlVoidTags.Contains(tagName);
                        if (!isSelfClosing)
                            current = newElement;

                        continue;
                    }
                }

                current.InnerHtml += part;
            }

            return root;
        }

        private void ParseAttributes(HtmlElement element, string attributesText)
        {
            var matches = Regex.Matches(attributesText, @"([\w\-:]+)\s*=\s*(?:""([^""]*)""|'([^']*)')");

            foreach (Match match in matches)
            {
                string name = match.Groups[1].Value.ToLower();
                string value = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[3].Value;
                element.Attributes[name] = value;

                if (name == "class")
                    element.Classes = value.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                else if (name == "id")
                    element.Id = value;
            }
        }
    }
}
