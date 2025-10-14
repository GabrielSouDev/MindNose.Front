using System.Text.Json.Serialization;

namespace MindNose.Front.Models.LLMModels;
public class TopProvider
{
    [JsonPropertyName("context_length")]
    public int? ContextLength { get; set; }

    [JsonPropertyName("max_completion_tokens")]
    public int? MaxCompletionTokens { get; set; }

    [JsonPropertyName("is_moderated")]
    public bool? IsModerated { get; set; }
}
