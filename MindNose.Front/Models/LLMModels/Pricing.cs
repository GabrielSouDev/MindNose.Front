﻿using System.Text.Json.Serialization;

namespace MindNose.Front.Models.LLMModels;
public class Pricing
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    [JsonPropertyName("completion")]
    public string Completion { get; set; }

    [JsonPropertyName("request")]
    public string Request { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonPropertyName("web_search")]
    public string WebSearch { get; set; }

    [JsonPropertyName("internal_reasoning")]
    public string InternalReasoning { get; set; }

    [JsonPropertyName("input_cache_read")]
    public string InputCacheRead { get; set; }

    [JsonPropertyName("input_cache_write")]
    public string InputCacheWrite { get; set; }

    [JsonPropertyName("audio")]
    public string Audio { get; set; }

}
