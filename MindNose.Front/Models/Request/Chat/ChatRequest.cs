using MindNose.Front.Models.Chat;
using MindNose.Front.Models.Enum;

namespace MindNose.Front.Models.Request.Chat;

public class ChatRequest
{
    public List<ElementHeader>? ElementsHeader { get; set; } = new();
    public Message Message { get; set; } = new();
    public OutputMode OutputMode { get; set; } = OutputMode.Conversational;
    public string Model { get; set; } = string.Empty;
}