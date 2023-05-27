using Infrastructure.AppEnvironment;
using Infrastructure.Entities.Pills;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ReminderApi.Interfaces.Pills;

namespace ReminderApi.Controllers.v1.PillsControllers;

[ApiController]
[Route("api/v1/pills")]
public class PillController : ControllerBase
{
    private readonly ILogger<PillController> _logger;
    private readonly IPillRepository _pillRepository;
    private readonly AppSettings _appSettings;

    public PillController(ILogger<PillController> logger, IPillRepository pillRepository, IOptions<AppSettings> appSettingsOptions)
    {
        _logger = logger;
        _pillRepository = pillRepository;
        _appSettings = appSettingsOptions.Value;
    }

    /// <summary>
    /// Получение всего списка лекарств. Маршрут сервер:порт/api/v1/pills/get_all_pills
    /// </summary>
    /// <returns>Весь список лекарств</returns>
    [HttpGet("get_all_pills")]
    [ProducesResponseType(typeof(List<Pill>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllPills()
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var pills = await _pillRepository.GetAllPill();

        return Ok(pills);
    }

    /// <summary>
    /// Получение всего списка лекарств, у которого похожее наименование. Маршрут сервер:порт/api/v1/pills/get_pills_by_name
    /// </summary>
    /// <param name="name">Наименование лекарства (строка)</param>
    /// <returns>Список лекарств с заданным наименованием</returns>
    [HttpGet("get_pills_by_name")]
    [ProducesResponseType(typeof(List<Pill>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPillsByName(string name)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var pills = await _pillRepository.GetPillByName(name);

        return Ok(pills);
    }
    
    /// <summary>
    /// Получение информации о конкретном лекарстве по идентификатору. Маршрут сервер:порт/api/v1/pills/get_pill
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Информация о конкретном лекарстве</returns>
    [HttpGet("get_pill")]
    [ProducesResponseType(typeof(Pill), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPill(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var pill = await _pillRepository.GetPill(id);

        return Ok(pill);
    }

    /// <summary>
    /// Создание записи о конкретном лекарстве. Маршрут сервер:порт/api/v1/pills/create_pill
    /// </summary>
    /// <param name="name">наименование</param>
    /// <param name="link">ссылка</param>
    /// <param name="description">описание</param>
    /// <returns></returns>
    [HttpPost("create_pill")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private async Task<IActionResult> CreatePill(string name, string link, string description)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        Pill pill = new Pill { Name = name, Link = link, Description = description };

        var createdPill = await _pillRepository.CreatePill(pill);

        return Ok(createdPill);
    }
    
    /// <summary>
    /// Удаление записи о лекарстве. . Маршрут сервер:порт/api/v1/pills/delete_pill
    /// </summary>
    /// <param name="id">идентификатор</param>
    /// <returns></returns>
    [HttpDelete("delete_pill")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private async Task<IActionResult> DeletePill(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _pillRepository.DeletePill(id);

        return Ok(result);
    }
    
    /// <summary>
    /// Метод обновления данных о конкретном лекарстве. Маршрут сервер:порт/api/v1/pills/update_pill
    /// </summary>
    /// <param name="id">идентификатор</param>
    /// <param name="name">наименование</param>
    /// <param name="link">ссылка</param>
    /// <param name="description">описание</param>
    /// <returns>Признак удалось ли обновить информацию (true / false) </returns>
    [HttpPut("update_pill")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private async Task<IActionResult> UpdatePill(Guid id, string name, string link, string description)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        Pill pill = new Pill { Name = name, Link = link, Description = description };

        var updatedPill = await _pillRepository.UpdatePill(id, pill);

        return Ok(updatedPill);
    }
    
    /// <summary>
    /// Запуск парсера данных с сайта в базу. Маршрут сервер:порт/api/v1/pills/fetch_pill
    /// </summary>
    /// <param name="token">Токен администратора</param>
    /// <returns>Признак удалось ли загрузить данные (true / false)</returns>
    [HttpGet("fetch_pill")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FetchPill(string token)
    {
        if (!ModelState.IsValid || _appSettings.AdminToken != token)
            return BadRequest();

        bool result = false;
        
        try
        {
            result = await _pillRepository.FetchPill();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Во время получения данных о лекарствах в базу произошла ошибка: {e}");
            _logger.LogInformation("Во время получения данных о лекарствах в базу произошла ошибка");
        }

        return Ok(result);
    }
}