namespace MindNose.Front.Models.Response.ConversationGuideDTO;

public class ConversationGuideDisplay
{
    public Guid Id { get; set; }
    public string ActualModel { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModified { get; set; }
}