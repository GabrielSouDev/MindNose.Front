using System.Globalization;
using MindNose.Front.Models.Response.LLMModels;

namespace MindNose.Front.Services;

public class OpenRouterService
{
    private ModelResponse _models = new();
    public void SetModels(ModelResponse models) => _models = models;
    public ModelResponse GetChatModels()
    {
        var avaibleModels = new ModelResponse
        {
            Data = _models.Data
                            .Where(m =>
                                (ParseDecimalSafe(m.Pricing.Prompt) + ParseDecimalSafe(m.Pricing.Completion)) < 0.0000003m
                            )
                            .ToList()
        };

        return avaibleModels;
    }
    public ModelResponse GetModels() => _models;

    private decimal ParseDecimalSafe(string? value) =>
    decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0;
}
