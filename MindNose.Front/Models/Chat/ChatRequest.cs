namespace MindNose.Front.Models.Chat;

public class ChatRequest
{
    public List<ElementHeader>? ElementsHeader { get; set; } = new();
    public Message Message { get; set; } = new();
    public string Model { get; set; } = string.Empty;
}