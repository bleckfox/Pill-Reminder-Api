using System.Text;
using HtmlAgilityPack;
using Infrastructure.Entities.Pills;
using ReminderApi.Interfaces.Pills;

namespace ReminderApi.Repositories.Pills;

public class Parser
{
    private readonly HttpClient _httpClient;

    private readonly string _mainUrl = "https://www.vidal.ru";

    private readonly string _mainUrlToPill = "https://www.vidal.ru/drugs/products";

    private readonly IPillRepository _pillRepository;

    public Parser(HttpClient client, IPillRepository pillRepository)
    {
        _httpClient = client;
        _pillRepository = pillRepository;
    }
    
    /// <summary>
    /// Метод получения информации о лекарстве из таблицы
    /// </summary>
    /// <param name="pillTableUrl">Ссылка на таблицу</param>
    /// <param name="createPill">Метод сохранения информации о лекарстве</param>
    public async Task FetchPillInformationAsync(string pillTableUrl)
    {
        bool isNotEnd = true;

        while (isNotEnd)
        {
            string currentLink = _mainUrl + pillTableUrl;
                
            string content = await _httpClient.GetStringAsync(currentLink);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);
                
            HtmlNode paginationDiv = document.DocumentNode.SelectSingleNode(("//div[contains(@class, 'pagination')]"));
                
            if (paginationDiv != null)
            {
                HtmlNode nextPage = paginationDiv.SelectSingleNode(".//span[contains(@class, 'next')]/a");

                if (nextPage != null)
                {
                    pillTableUrl = nextPage.GetAttributeValue("href", string.Empty);
                }
                else
                {
                    isNotEnd = false;
                }
            }
            else
            {
                isNotEnd = false;
            }
                
            HtmlNode productsTable = document.DocumentNode.SelectSingleNode("//table[contains(@class, 'products-table')]");
            HtmlNodeCollection rows = productsTable?.SelectNodes(".//tr");

            if (rows != null)
            {
                foreach (HtmlNode row in rows)
                {
                    HtmlNode cell = row.SelectSingleNode(".//td[contains(@class, 'products-table-name')]");

                    if (cell != null)
                    {
                        string pillLink = _mainUrl +
                                          cell.SelectSingleNode("./a").GetAttributeValue("href", string.Empty);
                        string pillName = cell.SelectSingleNode("./a").InnerText.Trim();
                        string pillDescription = await GetPillInfo(pillLink);

                        Pill pill = new Pill()
                        {
                            Name = pillName, Link = pillLink, Description = pillDescription
                        };

                        await _pillRepository.CreatePill(pill);
                    }
                }
            }
        }
        
    }
    
    /// <summary>
    /// Метод получения списка ссылок на таблицы с данными о лекарствах
    /// </summary>
    /// <returns>Список ссылок</returns>
    public async Task<List<string>> GetLinksToPill()
    {
        List<string> links = new List<string>();

        // Отправляем GET-запрос к сайту и получаем HTML-контент
        string htmlContent = await _httpClient.GetStringAsync(_mainUrlToPill);
            
        // Создаем объект HtmlDocument и загружаем в него HTML-контент
        HtmlDocument htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlContent);

        // Выбираем все элементы <div> с классом "letters-russian"
        var divElements = htmlDocument.DocumentNode
            .Descendants("div")
            .Where(div => div.Attributes["class"]?.Value == "letters-russian")
            .ToList();
            
        // Для каждого элемента <div> с классом "letters-russian" выбираем все ссылки
        // <a> и получаем значения атрибута href
        foreach (var element in divElements)
        {
            var elementLinks = element.Descendants("a")
                .Select(a => a.Attributes["href"]?.Value)
                .ToList();
                
            // Сохранение значений в общий список
            links.AddRange(elementLinks);
        }

        return links;
    }

    /// <summary>
    /// Метод получения информации об описании лекарства
    /// </summary>
    /// <param name="link">Ссылка на конкретную старницу лекарства</param>
    /// <returns>Описание лекарства в формате html</returns>
    private async Task<string> GetPillInfo(string link)
    {
        string pillContent = await _httpClient.GetStringAsync(link);
        HtmlDocument document = new HtmlDocument();
        document.LoadHtml(pillContent);
            
        HtmlNode schemaDiv = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'schema')]");
        
        HtmlNodeCollection divsWithoutClassOrId = schemaDiv.SelectNodes(".//div[not(@class) and not(@id)]");
        
        if (divsWithoutClassOrId != null)
        {
            foreach (HtmlNode div in divsWithoutClassOrId)
            {
                HtmlNode firm = div.SelectSingleNode(".//div[contains(@class, 'block firms')]");

                if (firm != null)
                {
                    // Создаем новый HtmlDocument и добавляем в него только нужный узел
                    HtmlDocument divDocument = new HtmlDocument();
                    divDocument.DocumentNode.AppendChild(div.Clone());
                    
                    StringBuilder description = new StringBuilder();
                    using (StringWriter writer = new StringWriter(description))
                    {
                        HtmlNode.ElementsFlags.Remove("noindex");
                        divDocument.Save(writer);    
                    }
                        
                    return description.ToString();
                }
            }
        }
        return $"Information not found. Check link -> {link}";
    }
}