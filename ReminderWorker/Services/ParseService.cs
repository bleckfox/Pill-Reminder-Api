namespace ReminderWorker.Services;

public class ParseService
{
    private readonly HttpClient _httpClient;

    private string MainUrl = "https://www.vidal.ru";

    private bool IsLoadEnd { get; set; } = false;

    public ParseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task FetchPillInformationAsync(string pillTableUrl, CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            
        }
    }

    public async Task<List<string>> GetLinksToPill(CancellationToken stoppingToken)
    {
        List<string> links = new List<string>();
        
        if (!stoppingToken.IsCancellationRequested)
        {
            
        }

        return links;
    }

}