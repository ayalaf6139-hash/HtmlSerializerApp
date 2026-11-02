using System.Text.Json;

namespace HtmlSerializerApp
{
    public class HtmlHelper
    {
        private static readonly HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] HtmlTags { get; private set; }
        public string[] HtmlVoidTags { get; private set; }

        private HtmlHelper()
        {
            string folder = Path.Combine(AppContext.BaseDirectory, "seed");
            HtmlTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText(Path.Combine(folder, "HtmlTags.json"))) ?? new string[0];
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText(Path.Combine(folder, "HtmlVoidTags.json"))) ?? new string[0];
        }
    }
}
