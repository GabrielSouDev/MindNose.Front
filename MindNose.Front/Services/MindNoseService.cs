using MindNose.Front.Models.Cytoscape;
using MindNose.Front.Models.LLMModels;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;

namespace MindNose.Front.Services;
public class MindNoseService
{
    private readonly HttpClient _httpClient;
    private readonly OpenRouterService _openRouterService;
    private readonly CytoscapeService _cytoscapeService;

    public MindNoseService(HttpClient httpClient, OpenRouterService openRouterService, CytoscapeService cytoscapeService)
    {
        _httpClient = httpClient;
        _openRouterService = openRouterService;
        _cytoscapeService = cytoscapeService;
    }

    public async Task InitializeAsync()
    {
        _openRouterService.SetModels(await GetModels());
        _cytoscapeService.SetCategories(await GetCategories());
    }

    public async Task<ElementsDTO> CreateOrGetLink(object request)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/MindNoseCore/GetOrCreateLink", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            var links = JsonConvert.DeserializeObject<CytoscapeDTO>(responseContent)!;

            return links.Elements;
        }
        else
            throw new Exception();
    }
    private async Task<ModelResponse> GetModels()
    {

        var modelList = await _httpClient.GetFromJsonAsync<ModelResponse>("api/Utils/GetModels");

        if (modelList is not null)
            return modelList;

        throw new Exception();
    }

    private async Task<List<CategoryResponse>> GetCategories()
    {

        var categories = await _httpClient.GetFromJsonAsync<List<CategoryResponse>>("api/Utils/GetCategories");

        if (categories is not null)
            return categories;

        throw new Exception();
    }


}

public class CategoryResponse
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
}
