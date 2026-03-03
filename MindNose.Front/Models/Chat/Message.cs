using MindNose.Front.Models.Enums;

namespace MindNose.Front.Models.Chat;
public class Message
{
    public string? Text { get; set; } = string.Empty;
    public MessageOrigin Origin { get; set; } = MessageOrigin.System;
}