using Microsoft.AspNetCore.Mvc;
using MasterService.Application.Interfaces;
using MasterService.Domain.Entities;

namespace MasterService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    private readonly IPartRepository _repository;
    private readonly ILogger<PartsController> _logger;

    // The Repository is injected here by the .NET Core DI container
    public PartsController(IPartRepository repository)
    {
        _repository = repository;
    }

    // Inject ILogger via Constructor
    public PartsController(ILogger<PartsController> logger)
    {
        _logger = logger;
    }

    // GET: api/parts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartMaster>>> GetParts()
    {
        _logger.LogInformation("MasterService: Fetching all parts from the database.");
        var parts = await _repository.GetAllAsync();
        return Ok(parts);
    }

    // GET: api/parts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PartMaster>> GetPart(int id)
    {
        var part = await _repository.GetByIdAsync(id);

        if (part == null)
        {
            return NotFound();
        }

        return Ok(part);
    }

    // POST: api/parts
    [HttpPost]
    public async Task<ActionResult<int>> CreatePart([FromBody] PartMaster part)
    {
        if (part == null)
        {
            return BadRequest("Part data is null");
        }

        // Logic check: Ensure Source is provided (1 or 2)
        if (part.Source <= 0)
        {
            return BadRequest("Source ID is required (1 for Manual, 2 for ExactMatch)");
        }

        var newPartId = await _repository.AddAsync(part);

        // Returns 201 Created with the new ID
        return CreatedAtAction(nameof(GetPart), new { id = newPartId }, newPartId);
    }
}