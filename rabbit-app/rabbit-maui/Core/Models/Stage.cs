namespace rabbit_maui.Core.Models
{
    public class Stage
    {
        public string Id { get; set; } = "";              // "E1", "E2", ...
        public string? Name { get; set; }                 // optional display name
        public List<Segment> Segments { get; set; } = [];
    }
}
