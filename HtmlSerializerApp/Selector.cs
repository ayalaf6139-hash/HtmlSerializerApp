using System.Text.RegularExpressions;

namespace HtmlSerializerApp
{
    public class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }

        public static Selector? Parse(string query)
        {
            var levels = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector? root = null;
            Selector? current = null;

            foreach (var level in levels)
            {
                Selector newSelector = new Selector();
                var parts = Regex.Split(level, @"(?=[#.])").Where(p => p.Length > 0);

                foreach (var part in parts)
                {
                    if (part.StartsWith("#"))
                        newSelector.Id = part.Substring(1);
                    else if (part.StartsWith("."))
                        newSelector.Classes.Add(part.Substring(1));
                    else if (HtmlHelper.Instance.HtmlTags.Contains(part.ToLower()))
                        newSelector.TagName = part.ToLower();
                }

                if (root == null)
                {
                    root = newSelector;
                }
                else
                {
                    newSelector.Parent = current;
                    current!.Child = newSelector;
                }

                current = newSelector;
            }

            return root;
        }
    }
}
