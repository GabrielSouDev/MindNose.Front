namespace MindNose.Front.Models.IAChat;

public class ModelChat
{
    public ModelChat() { }
    public ModelChat(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
