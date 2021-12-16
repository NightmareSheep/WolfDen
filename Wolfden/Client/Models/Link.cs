namespace Wolfden.Client.Models
{
    public struct Link
    {
        public Link(string title)
        {
            Title = title;
            Url = title;
        }

        public Link(string title, string url)
        {
            Title = title;
            Url = url;
        }

        public string Title { get; }
        public string Url { get; }
    }
}
