namespace MindNose.Front.Models.Cytoscape;
public class EdgeDataDTO
{
    public string Id { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public Dictionary<string, object>? Extra { get; set; } = new();
}