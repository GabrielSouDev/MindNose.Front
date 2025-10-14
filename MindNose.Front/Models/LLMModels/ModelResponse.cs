using System.Text.Json.Serialization;

namespace MindNose.Front.Models.LLMModels;
public class ModelResponse
{
    [JsonPropertyName("data")]
    public List<Model> Data { get; set; }

}
