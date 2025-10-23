namespace MindNose.Front.Models.Charts;
public class ChartTable
{
    public string Source { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<ChartElement> ElementsTarget { get; set; } = new();
}