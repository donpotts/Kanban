using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using KanbanTasks.Data;
using KanbanTasks.Shared.Models;

namespace KanbanTasks.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
[EnableRateLimiting("Fixed")]
public class KanbanTasksController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Tasks>> Get()
    {
        return Ok(ctx.Tasks);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Tasks>> GetAsync(long key)
    {
        var tasks = await ctx.Tasks.FirstOrDefaultAsync(x => x.Id == key);

        if (tasks == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(tasks);
        }
    }
}
