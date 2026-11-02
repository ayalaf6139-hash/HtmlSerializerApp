namespace HtmlSerializerApp
{
    public class HtmlElement
    {
        public string? Id { get; set; }
        public string Name { get; set; } = "";
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; } = "";
        public HtmlElement? Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            foreach (var child in Children)
                queue.Enqueue(child);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                yield return current;
                foreach (var child in current.Children)
                    queue.Enqueue(child);
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement? current = Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public override string ToString()
        {
            string id = Id != null ? $" id=\"{Id}\"" : "";
            string classes = Classes.Count > 0 ? $" class=\"{string.Join(" ", Classes)}\"" : "";
            return $"<{Name}{id}{classes}>";
        }
    }
}
