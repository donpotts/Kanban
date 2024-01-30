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
[Authorize]
[EnableRateLimiting("Fixed")]
public class TasksController(ApplicationDbContext ctx) : ControllerBase
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

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Tasks>> PostAsync(Tasks tasks)
    {
        await ctx.Tasks.AddAsync(tasks);

        await ctx.SaveChangesAsync();

        return Created($"/tasks/{tasks.Id}", tasks);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Tasks>> PutAsync(long key, Tasks update)
    {
        var tasks = await ctx.Tasks.FirstOrDefaultAsync(x => x.Id == key);

        if (tasks == null)
        {
            return NotFound();
        }

        ctx.Entry(tasks).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(tasks);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Tasks>> PatchAsync(long key, Delta<Tasks> delta)
    {
        var tasks = await ctx.Tasks.FirstOrDefaultAsync(x => x.Id == key);

        if (tasks == null)
        {
            return NotFound();
        }

        delta.Patch(tasks);

        await ctx.SaveChangesAsync();

        return Ok(tasks);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var tasks = await ctx.Tasks.FindAsync(key);

        if (tasks != null)
        {
            ctx.Tasks.Remove(tasks);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
