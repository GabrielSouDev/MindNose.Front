using MindNose.Front.Models.Enum;
using MindNose.Front.Models.Enums;

namespace MindNose.Front.Models.Response.ConversationGuideDTO;

public class MessageDTO
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public OutputMode OutputMode { get; set; } = OutputMode.Conversational;
    public MessageOrigin Origin { get; set; } = MessageOrigin.System;
}
