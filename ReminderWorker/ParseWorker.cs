using ReminderWorker.Services;

namespace ReminderWorker;

public class ParseWorker : BackgroundService
{
    private readonly ILogger<ParseWorker> _logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int linkCounter = 0;

        ParseService parser = new ParseService(new HttpClient());

        List<string> links = new List<string>();

        if (!stoppingToken.IsCancellationRequested)
        {
            links = await parser.GetLinksToPill(stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested | (linkCounter + 1) <= links.Count)
        {
            // Получаю текущую ссылку на таблицу лекарств (поиск по алфавиту)
            string currentLink = links[linkCounter];
            
            // Получаю данные о лекарствах для текущей ссылки
            await parser.FetchPillInformationAsync(currentLink, stoppingToken);
            
            // Увеличиваю счетчик ссылок (помогает следить, когда нужно прекратить цикл
            linkCounter++;

        }
        
    }
}