﻿using System.Text.Json.Serialization;

namespace MindNose.Front.Models.LLMModels;
public class Architecture
{
    [JsonPropertyName("modality")]
    public string Modality { get; set; }

    [JsonPropertyName("input_modalities")]
    public List<string> InputModalities { get; set; }

    [JsonPropertyName("output_modalities")]
    public List<string> OutputModalities { get; set; }

    [JsonPropertyName("tokenizer")]
    public string Tokenizer { get; set; }

    [JsonPropertyName("instruct_type")]
    public object? InstructType { get; set; }
}
