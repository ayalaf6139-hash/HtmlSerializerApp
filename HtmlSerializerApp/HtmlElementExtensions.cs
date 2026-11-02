namespace HtmlSerializerApp
{
    public static class HtmlElementExtensions
    {
        public static HashSet<HtmlElement> Query(this HtmlElement element, Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            Search(element, selector, result);
            return result;
        }

        private static void Search(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            var matches = element.Descendants().Where(d => IsMatch(d, selector));

            if (selector.Child == null)
            {
                foreach (var match in matches)
                    result.Add(match);
                return;
            }

            foreach (var match in matches)
                Search(match, selector.Child, result);
        }

        private static bool IsMatch(HtmlElement element, Selector selector)
        {
            if (selector.TagName != null && element.Name != selector.TagName)
                return false;

            if (selector.Id != null && element.Id != selector.Id)
                return false;

            return selector.Classes.All(c => element.Classes.Contains(c));
        }
    }
}
