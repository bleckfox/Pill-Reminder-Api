using Infrastructure.Entities.Pills;
using Microsoft.AspNetCore.Mvc;
using ReminderApi.Interfaces;

namespace ReminderApi.Controllers.v1.PillsControllers;

[ApiController]
[Route("api/v1/pills")]
public class PillController : ControllerBase
{
    private readonly ILogger<PillController> _logger;
    private readonly IPillRepository _pillRepository;

    public PillController(ILogger<PillController> logger, IPillRepository pillRepository)
    {
        _logger = logger;
        _pillRepository = pillRepository;
    }

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
    
    [HttpGet("get_all_pill_names")]
    [ProducesResponseType(typeof(List<Pill>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllPillNames()
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var pills = await _pillRepository.GetAllPillNames();

        return Ok(pills);
    }

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

    [HttpPost("create_pill")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePill(string name, string link, string description)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        Pill pill = new Pill { Name = name, Link = link, Description = description };

        var createdPill = await _pillRepository.CreatePill(pill);

        return Ok(createdPill);
    }
    
    [HttpDelete("delete_pill")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePill(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _pillRepository.DeletePill(id);

        return Ok(result);
    }
    
    [HttpPut("update_pill")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePill(Guid id, string name, string link, string description)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        Pill pill = new Pill { Name = name, Link = link, Description = description };

        var updatedPill = await _pillRepository.UpdatePill(id, pill);

        return Ok(updatedPill);
    }
}