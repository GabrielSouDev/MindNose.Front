using MindNose.Front.Models.LLMModels;

namespace MindNose.Front.Services;

public class OpenRouterService
{
    private ModelResponse _models = new();
    public void SetModels(ModelResponse models) => _models = models;
    public ModelResponse GetModels() => _models;

}
