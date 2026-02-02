using System.Text.Json.Serialization;

namespace MindNose.Front.Models.Response.LLMModels;
public class ModelResponse
{
    [JsonPropertyName("data")]
    public List<Model> Data { get; set; }

}
