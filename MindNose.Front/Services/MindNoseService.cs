using MindNose.Front.Models.Cytoscape;
using MindNose.Front.Models.IAModels;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace MindNose.Front.Services;
public class MindNoseService
{
    private HttpClient _httpClient;

    public MindNoseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ElementsDTO> CreateOrGetLink(object request)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/MindNoseCore/CreateOrGetLink", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            var links = JsonConvert.DeserializeObject<CytoscapeDTO>(responseContent)!;

            return links.Elements;
        }
        else
            throw new Exception();
    }
    public async Task<List<ModelDTO>> GetModels()
    {

        var modelList = await _httpClient.GetFromJsonAsync<List<ModelDTO>>("api/Utils/GetModelsList");

        if (modelList is not null)
            return modelList;

        throw new Exception();
    }
}
