using HtmlSerializerApp;

string url = "https://www.example.com";
string query = "div p";

HtmlSerializer serializer = new HtmlSerializer();
string html = await serializer.Load(url);
HtmlElement dom = serializer.Serialize(html);

Selector? selector = Selector.Parse(query);
if (selector == null)
{
    Console.WriteLine("Empty query");
    return;
}

var result = dom.Query(selector);

Console.WriteLine($"Found {result.Count} elements:");
foreach (var element in result)
{
    Console.WriteLine(element);
}
