namespace MindNose.Front.Models.Response.CytoscapeDTO;
public class NodeDataDTO
{
    public string Id { get; set; } = string.Empty;
    public string? Label { get; set; } = string.Empty;
    public Dictionary<string, object>? Extra { get; set; } = new();
}