namespace MindNose.Front.Models.Response.ConversationGuideDTO;

public class ConversationGuideDTO
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public List<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
    public string ActualModel { get; set; } = string.Empty;
}