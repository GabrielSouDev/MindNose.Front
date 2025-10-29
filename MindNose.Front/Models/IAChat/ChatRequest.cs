namespace MindNose.Front.Models.IAChat;

public class ChatRequest
{
    public List<ElementHeader>? ElementsHeader { get; set; } = new();
    public Message Message { get; set; } = new();
    public string Model { get; set; } = string.Empty;
}