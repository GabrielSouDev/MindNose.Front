using MindNose.Front.Models.Enums;

namespace MindNose.Front.Models.IAChat;
public class Message
{
    public string? Text { get; set; } = string.Empty;
    public IAChatOrigin Origin { get; set; } = IAChatOrigin.System;
}