namespace MindNose.Front.Models.Chat;

public class ChatModel
{
    public ChatModel() { }
    public ChatModel(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
