using ArsitekturBawang.Application.Resources;
using ArsitekturBawang.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ArsitekturBawang.WebApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResource>> Get(int id, CancellationToken cancellationToken)
    {
        var response = await _userService.Get(id, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResource>>> GetAll(CancellationToken cancellationToken)
    {
        var response = await _userService.GetAll(cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<UserResource>> Create([FromBody] UserResource resource,
        CancellationToken cancellationToken)
    {
        var response = await _userService.Create(resource, cancellationToken);
        return CreatedAtAction(nameof(Get), new {response.Id}, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResource>> Update(int id, [FromBody] UserResource resource,
        CancellationToken cancellationToken)
    {
        resource.Id = id;
        var response = await _userService.Update(resource, cancellationToken);
        return Ok(resource);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserResource>> Update(int id, CancellationToken cancellationToken)
    {
        await _userService.Delete(id, cancellationToken);
        return NoContent();
    }
}

