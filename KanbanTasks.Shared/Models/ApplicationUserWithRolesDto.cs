using System.ComponentModel.DataAnnotations;

namespace KanbanTasks.Shared.Models;

public class ApplicationUserWithRolesDto : ApplicationUserDto
{
    public List<string>? Roles { get; set; }
}
