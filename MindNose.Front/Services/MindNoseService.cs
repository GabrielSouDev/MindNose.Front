namespace MindNose.Front.Services;
public class MindNoseService
{
    private HttpClient _httpClient;

    public MindNoseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
