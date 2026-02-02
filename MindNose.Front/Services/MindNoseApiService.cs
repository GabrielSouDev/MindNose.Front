using MindNose.Front.Models.Chat;
using MindNose.Front.Models.Request.User;
using MindNose.Front.Models.Response;
using MindNose.Front.Models.Response.CytoscapeDTO;
using MindNose.Front.Models.Response.LLMModels;
using MindNose.Front.Models.Response.User;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace MindNose.Front.Services;
public class MindNoseApiService
{
    private readonly HttpClient _httpClient;
    private readonly OpenRouterService _openRouterService;
    private readonly CytoscapeService _cytoscapeService;

    public MindNoseApiService(IHttpClientFactory httpClientFactory, OpenRouterService openRouterService, CytoscapeService cytoscapeService)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _openRouterService = openRouterService;
        _cytoscapeService = cytoscapeService;
    }

    public async Task InitializeAsync()
    {
        _openRouterService.SetModels(await GetModels());
        _cytoscapeService.SetCategories(await GetCategories());
    }

    public async Task<bool> Register(RegisterRequest registerRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/Register", registerRequest);

        return response.IsSuccessStatusCode;
    }

    public async Task<LoginResponse?> Login(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/Login", loginRequest);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result;
    }
    
    public async Task<HttpResponseMessage> AddCategoryAsync(string newCategory) =>
        await _httpClient.PostAsJsonAsync("api/Utils/AddCategory", newCategory);

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
            throw new HttpRequestException("Falha ao Criar/Obter Link! => api/MindNoseCore/GetOrCreateLink");
    }

    public async Task<string> SendChatAsync(ChatRequest request)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/MindNoseCore/SendAIChat", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
        else
            throw new HttpRequestException("Falha ao enviar Prompt! => api/MindNoseCore/SendAIChat");
    }

    private async Task<ModelResponse> GetModels()
    {
        var modelList = await _httpClient.GetFromJsonAsync<ModelResponse>("api/Utils/GetModels") 
            ?? throw new HttpRequestException("Falha ao obter Models pela API! => api/Utils/GetModels");
            
        return modelList;
    }

    private async Task<List<CategoryResponse>> GetCategories()
    {
        var categories = await _httpClient.GetFromJsonAsync<List<CategoryResponse>>("api/Utils/GetCategories") 
            ?? throw new HttpRequestException($"Falha ao obter Categorias da API! => api/Utils/GetCategories");

        return categories;        
    }
}
